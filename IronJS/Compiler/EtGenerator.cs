﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using IronJS.Extensions;
using IronJS.Runtime;
using IronJS.Runtime.Binders;
using IronJS.Runtime.Js;
using IronJS.Runtime.Utils;
using Microsoft.Scripting.Utils;

namespace IronJS.Compiler
{
    using LambdaExprList = List<Tuple<Expression<Func<IFrame, object>>, List<string>>>;
    using AstUtils = Microsoft.Scripting.Ast.Utils;
    using Et = System.Linq.Expressions.Expression;

    public class EtGenerator
    {
        internal Context Context;
        internal ParameterExpression TableExpr;
        internal ParameterExpression GlobalFrameExpr;
        internal LambdaExprList LambdaExprs;
        internal Stack<FunctionScope> FunctionScopes;

        internal int LambdaId
        {
            get { return LambdaExprs.Count - 1; }
        }

        internal FunctionScope FunctionScope
        {
            get { return FunctionScopes.Peek(); }
        }

        public CodeUnit Build(List<Ast.Node> astNodes, Context context)
        {
            Context = context;
            TableExpr = Et.Parameter(typeof(Table), "#functbl");
            LambdaExprs = new LambdaExprList();
            FunctionScopes = new Stack<FunctionScope>();

            // Enter gobal frame
            EnterFunctionScope();

            // Store the frame expr for global frame
            GlobalFrameExpr = FunctionScope.FrameExpr;

            var globalExprs = new List<Et>();

            foreach (var node in astNodes)
            {
                globalExprs.Add(Generate(node));
            }

            var buildLambdaExprs = new List<Et>();

            buildLambdaExprs.Add(
                Et.Assign(
                    TableExpr,
                    AstUtils.SimpleNewHelper(
                        typeof(Table).GetConstructor(Type.EmptyTypes)
                    )
                )
            );

            //TODO: remove after debugging
            buildLambdaExprs.Add(
                FrameUtils.Push(
                    FunctionScope.FrameExpr, 
                    "functbl", 
                    TableExpr, 
                    VarType.Local
                )
            );

            var tablePushMi = typeof(Table).GetMethod("Push");
            var functionCtor = typeof(Lambda).GetConstructor(
                new[] { 
                    typeof(Func<IFrame, object>), 
                    typeof(List<string>)
                }
            );

            foreach (var lambda in LambdaExprs)
            {
                buildLambdaExprs.Add(
                    Et.Call(
                        TableExpr,
                        tablePushMi,
                        AstUtils.SimpleNewHelper(functionCtor, lambda.V1, Et.Constant(lambda.V2))
                    )
                );
            }

            var allExprs = CollectionUtils.Concat(
                buildLambdaExprs, 
                globalExprs
            );

            var compiledDelegate = Et.Lambda<Action<IFrame>>(
                Et.Block(
                    new[] { TableExpr },
                    allExprs
                ),
                FunctionScope.FrameExpr
            ).Compile();

            return new CodeUnit(compiledDelegate, Context);
        }

        private void EnterFunctionScope()
        {
            FunctionScopes.Push(new FunctionScope());
        }

        private void ExitFunctionScope()
        {
            FunctionScopes.Pop();
        }

        private Et Generate(Ast.Node node)
        {
            if (node is Ast.ILabelableNode)
                (node as Ast.ILabelableNode).Enter(FunctionScope);

            var et = GenerateEt(node);

            if (node is Ast.ILabelableNode)
                (node as Ast.ILabelableNode).Exit(FunctionScope);

            return et;
        }

        private Et GenerateEt(Ast.Node node)
        {
            if (node == null)
                return AstUtils.Empty();

            switch (node.Type)
            {
                case Ast.NodeType.Assign:
                    return GenerateAssign((Ast.AssignNode)node);

                case Ast.NodeType.Lambda:
                    return GenerateLambda((Ast.LambdaNode)node);

                case Ast.NodeType.Block:
                    return GenerateBlock((Ast.BlockNode)node);

                case Ast.NodeType.Call:
                    return GenerateCall((Ast.CallNode)node);

                case Ast.NodeType.Identifier:
                    return GenerateIdentifier((Ast.IdentifierNode)node);

                case Ast.NodeType.BinaryOp:
                    return GenerateBinaryOp((Ast.BinaryOpNode)node);

                case Ast.NodeType.UnaryOp:
                    return GenerateUnaryOp((Ast.UnaryOpNode)node);

                case Ast.NodeType.Return:
                    return GenerateReturn((Ast.ReturnNode)node);

                case Ast.NodeType.New:
                    return GenerateNew((Ast.NewNode)node);

                case Ast.NodeType.MemberAccess:
                    return GenerateMemberAccess((Ast.MemberAccessNode)node);

                case Ast.NodeType.Logical:
                    return GenerateLogical((Ast.LogicalNode)node);

                case Ast.NodeType.PostfixOperator:
                    return GeneratePostFixOp((Ast.PostfixOperatorNode)node);

                case Ast.NodeType.TypeOf:
                    return GenerateTypeOf((Ast.TypeOfNode)node);

                case Ast.NodeType.Boolean:
                    return GenerateBoolean((Ast.BooleanNode)node);

                case Ast.NodeType.Void:
                    return GenerateVoid((Ast.VoidNode)node);

                case Ast.NodeType.If:
                    return GenerateIf((Ast.IfNode)node);

                case Ast.NodeType.StrictCompare:
                    return GenerateStrictCompare((Ast.StrictCompareNode)node);

                case Ast.NodeType.UnsignedRightShift:
                    return GenerateUnsignedRightShift((Ast.UnsignedRightShiftNode)node);

                case Ast.NodeType.While:
                    return GenerateWhile((Ast.WhileNode)node);

                case Ast.NodeType.ForStep:
                    return GenerateForStep((Ast.ForStepNode)node);

                case Ast.NodeType.Break:
                    return GenerateBreak((Ast.BreakNode)node);

                case Ast.NodeType.Continue:
                    return GenerateContinue((Ast.ContinueNode)node);

                case Ast.NodeType.With:
                    return GenerateWith((Ast.WithNode)node);

                case Ast.NodeType.Try:
                    return GenerateTry((Ast.TryNode)node);

                case Ast.NodeType.Throw:
                    return GenerateThrow((Ast.ThrowNode)node);

                case Ast.NodeType.IndexAccess:
                    return GenerateIndexAccess((Ast.IndexAccessNode)node);

                #region Constants

                case Ast.NodeType.Number:
                    return GenerateNumber((Ast.NumberNode)node);

                case Ast.NodeType.String:
                    return GenerateString((Ast.StringNode)node);

                case Ast.NodeType.Null:
                    return GenerateNull((Ast.NullNode)node);

                #endregion

                default:
                    throw new Compiler.CompilerError("Unsupported AST node '" + node.Type + "'");
            }
        }

        private Et GenerateIndexAccess(Ast.IndexAccessNode node)
        {
            return Et.Dynamic(
                Context.CreateGetIndexBinder(new CallInfo(1)),
                typeof(object),
                Generate(node.Target),
                Generate(node.Index)
            );
        }

        private Et GenerateThrow(Ast.ThrowNode node)
        {
            return Et.Throw(
                Generate(node.Target)
            );
        }

        private Et GenerateTry(Ast.TryNode node)
        {
            // try ... finally
            if (node.Catch == null)
            {
                return Et.TryFinally(
                    Generate(node.Body),
                    Generate(node.Finally)
                );
            }
            else
            {
                var catchParam = Et.Parameter(typeof(object), "#catch");

                var catchBody = Et.Block(
                    BuildAssign(
                        node.Catch.Target, 
                        catchParam
                    ),
                    Generate(node.Catch.Body)
                );

                var catchBlock = Et.Catch(
                    catchParam, 
                    catchBody
                );

                var tryBody = EtUtils.Box(Generate(node.Body));

                // try ... catch 
                if (node.Finally == null)
                {
                    return Et.TryCatch(
                        tryBody,
                        catchBlock
                    );
                }
                // try ... catch ... finally
                else
                {
                    return Et.TryCatchFinally(
                        tryBody,
                        Generate(node.Finally),
                        catchBlock
                    );
                }
            }
        }

        // with(...) { ... }
        private Et GenerateWith(Ast.WithNode node)
        {
            return Et.Block(
                Et.Assign(FunctionScope.FrameExpr, 
                    AstUtils.SimpleNewHelper(
                        typeof(WithFrame).GetConstructor(
                            new[] { 
                                typeof(Obj), 
                                typeof(IFrame)
                            }
                        ),
                        Generate(node.Target),
                        FunctionScope.FrameExpr
                    )
                ),
                Generate(node.Body),
                FrameUtils.Exit(
                    FunctionScope.FrameExpr,
                    FunctionScope.FrameExpr
                )
            );
        }

        // continue
        private Et GenerateContinue(Ast.ContinueNode node)
        {
            if (node.Label == null)
                return Et.Continue(FunctionScope.LabelScope.Continue());

            return Et.Continue(FunctionScope.LabelScope.Continue(node.Label));
        }

        // 12.8
        // break, break <label>
        private Et GenerateBreak(Ast.BreakNode node)
        {
            if (node.Label == null)
                return Et.Break(FunctionScope.LabelScope.Break());

           return Et.Break(FunctionScope.LabelScope.Break(node.Label));
        }

        // 12.6.3
        // for(init, test, incr) { <expr>, <expr> ... <expr> }
        private Et GenerateForStep(Ast.ForStepNode node)
        {
            var body = Generate(node.Body);
            var init = Generate(node.Setup);
            var incr = Generate(node.Incr);
            var test =
                Et.Dynamic(
                    Context.CreateConvertBinder(typeof(bool)),
                    typeof(bool),
                    Generate(node.Test)
                );

            var loop = AstUtils.Loop(
                test,
                incr,
                body,
                null,
                FunctionScope.LabelScope.Break(),
                FunctionScope.LabelScope.Continue()
            );

            return Et.Block(
                init,
                loop
            );
        }

        // 12.6.1
        // 12.6.2
        // while(test) { <expr>, <expr> ... <expr> }
        // do { <expr>, <expr> ... <expr> } while(test);
        private Et GenerateWhile(Ast.WhileNode node)
        {
            Et loop;

            var test = Et.Dynamic(
                Context.CreateConvertBinder(typeof(bool)),
                typeof(bool),
                Generate(node.Test)
            );

            // while
            if (node.Loop == Ast.WhileType.While)
            {
                var body = Generate(node.Body);

                loop = AstUtils.While(
                    test,
                    body,
                    null,
                    FunctionScope.LabelScope.Break(),
                    FunctionScope.LabelScope.Continue()
                );
            }
            // do ... while
            else if (node.Loop == Ast.WhileType.Do)
            {
                var bodyExprs = new List<Et>();

                bodyExprs.Add(Generate(node.Body));

                // test last, instead of first
                bodyExprs.Add(
                    Et.IfThenElse(
                        test,
                        Et.Continue(FunctionScope.LabelScope.Continue()),
                        Et.Break(FunctionScope.LabelScope.Break())
                    )
                );

                loop = Et.Loop(
                    Et.Block(
                        bodyExprs
                    ),
                    FunctionScope.LabelScope.Break(),
                    FunctionScope.LabelScope.Continue()
                );
            }
            else
            {
                throw new NotImplementedException();
            }

            return loop;
        }

        // -14 >>> 2
        // 11.7.3
        private Et GenerateUnsignedRightShift(Ast.UnsignedRightShiftNode node)
        {
            //TODO: to much boxing/conversion going on
            return EtUtils.Box(
                Et.Convert(
                    Et.Call(
                        typeof(BuiltIns).GetMethod("UnsignedRightShift"),

                        Et.Convert(
                            Et.Dynamic(
                                Context.CreateConvertBinder(typeof(double)),
                                typeof(double),
                                Generate(node.Left)
                            ),
                            typeof(int)
                        ),

                        Et.Convert(
                            Et.Dynamic(
                                Context.CreateConvertBinder(typeof(double)),
                                typeof(double),
                                Generate(node.Right)
                            ),
                            typeof(int)
                        )

                    ),
                    typeof(double)
                )
            );
        }

        // 11.9.4
        // 11.9.5
        // foo === bar, foo !== bar
        private Et GenerateStrictCompare(Ast.StrictCompareNode node)
        {
            // for both
            Et expr = Et.Call(
                typeof(BuiltIns).GetMethod("StrictEquality"),
                Generate(node.Left),
                Generate(node.Right)
            );

            // specific to 11.9.5
            if(node.Op == ExpressionType.NotEqual)
                expr = Et.Not(Et.Convert(expr, typeof(bool)));

            return expr;
        }

        // 11.12
        // 12.4
        // if (test) { TrueBranch } else { ElseBranch }
        private Et GenerateIf(Ast.IfNode node)
        {
            return Et.Condition(
                Et.Dynamic(
                    Context.CreateConvertBinder(typeof(bool)),
                    typeof(bool),
                    Generate(node.Test)
                ),
                Generate(node.TrueBranch),
                Generate(node.ElseBranch)
            );
        }

        // 11.4.2
        // void foo
        private Et GenerateVoid(Ast.VoidNode node)
        {
            // 11.4.2
            return Et.Block(
                Generate(node.Target),
                Undefined.Expr
            );
        }

        // 8.3
        // true, false
        private Et GenerateBoolean(Ast.BooleanNode node)
        {
            return Et.Constant(
                node.Value, 
                typeof(object)
            );
        }

        // 11.4.3
        // typeof foo
        private Et GenerateTypeOf(Ast.TypeOfNode node)
        {
            return Et.Call(
                typeof(BuiltIns).GetMethod("TypeOf"),
                Generate(node.Target)
            );
        }

        // 11.3
        // foo++, foo--
        private Et GeneratePostFixOp(Ast.PostfixOperatorNode node)
        {
            var target = Generate(node.Target);
            var tmp = Et.Parameter(typeof(double), "#tmp");

            return Et.Block(
                new[] { tmp },

                // the value we will return
                Et.Assign(
                    tmp, 
                    Et.Dynamic(
                        Context.CreateConvertBinder(typeof(double)),
                        typeof(double),
                        target
                    )
                ),

                // calc new value
                BuildAssign(
                    node.Target,
                    EtUtils.Box(
                        Et.Add(
                            tmp, 
                            Et.Constant(
                                node.Op == ExpressionType.PostIncrementAssign
                                         ? 1.0    // 11.3.1
                                         : -1.0,  // 11.3.2
                                typeof(double)
                            )
                        )
                    )
                ),

                tmp // return the old value
            );
        }
        
        // 11.11
        // foo || bar, foo && bar
        private Et GenerateLogical(Ast.LogicalNode node)
        {
            var tmp = Et.Parameter(typeof(object), "#tmp");

            return Et.Block(
                new[] { tmp },
                Et.Assign(tmp, Generate(node.Left)),
                Et.Condition(
                    Et.Dynamic(
                        Context.CreateConvertBinder(typeof(bool)),
                        typeof(bool),
                        tmp
                    ),
                    node.Op == ExpressionType.AndAlso 
                             ? Generate(node.Right)  // &&
                             : tmp,                  // ||
                    node.Op == ExpressionType.AndAlso 
                             ? tmp                   // &&
                             : Generate(node.Right)  // ||
                )
            );
        }

        // 11.4.6
        // 11.4.7
        // 11.4.8
        // 11.4.9
        // !foo, -foo, etc.
        private Et GenerateUnaryOp(Ast.UnaryOpNode node)
        {
            return Et.Dynamic(
                Context.CreateUnaryOpBinder(node.Op),
                typeof(object),
                Generate(node.Target)
            );
        }

        // 8.2
        // null
        private Et GenerateNull(Ast.NullNode node)
        {
            return Et.Default(typeof(object));
        }

        // 11.2.1
        // foo.bar
        private Et GenerateMemberAccess(Ast.MemberAccessNode node)
        {
            return Et.Dynamic(
                Context.CreateGetMemberBinder(node.Name),
                typeof(object),
                Generate(node.Target)
            );
        }

        // 11.2.2
        // 13.2.2
        // foo = {}
        // foo = new X
        // foo = new X()
        private Et GenerateNew(Ast.NewNode node)
        {
            var target = Generate(node.Target);
            var args = node.Args.ToEtArray(x => Generate(x));
            var tmp = Et.Variable(typeof(IObj), "#tmp");
            var exprs = new List<Et>();

            
            exprs.Add(
                Et.Assign(
                    tmp,
                    EtUtils.Cast<IObj>(
                        Et.Dynamic(
                            Context.CreateInstanceBinder(
                                new CallInfo(args.Length)
                            ),
                            typeof(object),
                            ArrayUtils.Insert(
                                target,
                                args
                            )
                        )
                    )
                )
            );
            
            // this handles properties defined
            // in the shorthand json-style object
            // expression: { foo: 1, bar: 2 }
            if (node.Properties != null)
            {
                foreach (var propNode in node.Properties)
                {
                    exprs.Add(
                        Et.Call(
                            tmp,
                            typeof(IObj).GetMethod("Put"),
                            Et.Constant(propNode.Name, typeof(object)),
                            EtUtils.Box(Generate(propNode.Value))
                        )
                    );
                }
            }

            exprs.Add(
                EtUtils.Box(tmp)
            );

            return Et.Block(
                new[] { tmp },
                exprs
            );
        }

        // 12.8
        private Et GenerateReturn(Ast.ReturnNode node)
        {
            // return <expr>
            return Et.Return(FunctionScope.ReturnLabel, Generate(node.Value), typeof(object));
        }

        // 8.4
        private Et GenerateString(Ast.StringNode node)
        {
            // 'foo'
            return Et.Constant(node.Value, typeof(object));
        }

        // 8.5
        private Et GenerateNumber(Ast.NumberNode node)
        {
            // 1.0
            return Et.Constant(node.Value, typeof(object));
        }

        // 7.6
        private Et GenerateIdentifier(Ast.IdentifierNode node, Runtime.Js.GetType type = Runtime.Js.GetType.Value)
        {
            // foo
            return FrameUtils.Pull(FunctionScope.FrameExpr, node.Name, type);
        }

        // ???
        private Et GenerateBlock(Ast.BlockNode node)
        {
            // { <expr>, <expr>, ... <expr> }
            if (node.Nodes.Count == 0)
                return Et.Default(typeof(object));

            return Et.Block(
                node.Nodes.Select(x => Generate(x))
            );
        }

        // 11.5
        // 11.6
        // 11.7
        // 11.8
        // 11.9 (not all, strict compare is implemented in GenerateStrictCompare)
        // 11.10
        //TODO: implement 'instanceof' and  'in' operators
        private Et GenerateBinaryOp(Ast.BinaryOpNode node)
        {
            // left @ right
            return Et.Dynamic(
                Context.CreateBinaryOpBinder(node.Op),
                typeof(object),
                Generate(node.Left),
                Generate(node.Right)
            );
        }

        // 13.2.1
        private Et GenerateCall(Ast.CallNode node)
        {
            var args = node.Args.ToEtArray(x => Generate(x));

            // foo();
            if (node.Target is Ast.IdentifierNode)
            {
                var target = GenerateIdentifier(
                    (Ast.IdentifierNode)node.Target, 
                    Runtime.Js.GetType.Name
                );

                return Et.Dynamic(
                    Context.CreateInvokeBinder(
                        new CallInfo(args.Length)
                    ),
                    typeof(object),
                    ArrayUtils.Insert(
                        target,
                        args
                    )
                );
            }

            // foo.bar();
            else if(node.Target is Ast.MemberAccessNode)
            {
                var target = (Ast.MemberAccessNode)node.Target;

                return Et.Dynamic(
                    Context.CreateInvokeMemberBinder(
                        target.Name,
                        new CallInfo(args.Length + 1)
                    ),
                    typeof(object),
                    ArrayUtils.Insert(
                        Generate(target.Target),
                        args
                    )
                );
            }

            throw new NotImplementedException();
        }

        // 13
        private Et GenerateLambda(Ast.LambdaNode node)
        {
            EnterFunctionScope();

            var bodyNode = (Ast.BlockNode)node.Body;
            var argsList = node.Args.Select(x => x.Name).ToList();
            var bodyExprs = bodyNode.Nodes.Select(x => Generate(x)).ToList();

            bodyExprs.Add(
                Et.Label(
                    FunctionScope.ReturnLabel, 
                    Undefined.Expr // 12.9
                )
            );

            LambdaExprs.Add(
                Tuple.Create(
                    Et.Lambda<Func<IFrame, object>>(
                        Et.Block(bodyExprs),
                        FunctionScope.FrameExpr
                    ), 
                    argsList
                )
            );

            ExitFunctionScope();

            /*
             * 1) Create a new object with the current frame and current lambda as params
             * 2) Assign the #FunctionPrototype object to it's Prototype field
             * 3) return it
             */

            return Et.Call(
                Et.Constant(Context),
                typeof(Context).GetMethod("CreateFunction"),
                FunctionScope.FrameExpr,
                Et.Call(
                    TableExpr,
                    typeof(Table).GetMethod("Pull"),
                    Et.Constant(LambdaId)
                )
            );
        }

        // 11.13.1
        private Et GenerateAssign(Ast.AssignNode node)
        {
            return BuildAssign(node.Target, Generate(node.Value));
        }

        // 11.13.1
        private Et BuildAssign(Ast.Node target, Et value)
        {
            if (target is Ast.IdentifierNode)
            {
                var idNode = (Ast.IdentifierNode)target;

                return FrameUtils.Push(
                    FunctionScope.FrameExpr,
                    idNode.Name,
                    value,
                    idNode.IsLocal ? VarType.Local : VarType.Global
                );
            }
            else if (target is Ast.MemberAccessNode)
            {
                var maNode = (Ast.MemberAccessNode)target;

                return Et.Dynamic(
                    Context.CreateSetMemberBinder(maNode.Name),
                    typeof(object),
                    Generate(maNode.Target),
                    value
                );
            }
            else if (target is Ast.IndexAccessNode)
            {
                var ixNode = (Ast.IndexAccessNode)target;

                return Et.Dynamic(
                    Context.CreateSetIndexBinder(new CallInfo(1)),
                    typeof(object),
                    Generate(ixNode.Target),
                    Generate(ixNode.Index),
                    value
                );
            }

            throw new NotImplementedException();
        }
    }
}