﻿using System;
using System.Text;
using Antlr.Runtime.Tree;
using IronJS.Compiler;

namespace IronJS.Compiler.Ast
{
    using Et = System.Linq.Expressions.Expression;

    public enum NodeType
    {
        Assign, Identifier, Number, Null,
        MemberAccess, Call, If, Eq, Block,
        String, ClrNew, Lambda, While, BinaryOp,
        Object, New, AutoProperty, Return,
        UnaryOp,
        Logical,
        PostfixOperator,
        TypeOf,
        Boolean,
        Void,
        StrictCompare,
        UnsignedRightShift
    }

    abstract public class Node
    {
        public readonly NodeType Type;
        public readonly int Line;
        public readonly int Column;

        public Node(NodeType type, ITree node)
            : this(type)
        {
            Line = node.Line;
            Column = node.CharPositionInLine;
        }
         
        [Obsolete("This constructor is obsolete and will replaced by (NodeType, ITree)")]
        public Node(NodeType type)
        {
            Type = type;
        }

        public string Print()
        {
            var writer = new StringBuilder();

            Print(writer);

            return writer.ToString();
        }

        public virtual void Print(StringBuilder writer, int indent = 0)
        {
            var indentStr = new String(' ', indent * 2);
            writer.AppendLine(indentStr + "(" + Type + ")");
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        #region abstract

        public abstract Et Walk(EtGenerator etgen);

        #endregion
    }
}
