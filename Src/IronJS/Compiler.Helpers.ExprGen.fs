﻿namespace IronJS.Compiler.Helpers

open IronJS
open IronJS.Utils
open IronJS.Tools
open IronJS.Compiler.Types

(*ExprGen helper module containing common funcitons*)

module ExprGen =
  let newFunction closureType args =
    Dlr.Expr.newGenericArgs Runtime.Function.functionTypeDef [closureType] args

  let callFunction func (args:Et list) =
    let binder = new Runtime.Binders.Invoke(new System.Dynamic.CallInfo(args.Length + 1))
    Dlr.Expr.dynamic binder Constants.clrDynamic (func :: args)