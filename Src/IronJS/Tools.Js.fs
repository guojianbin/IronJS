﻿module IronJS.Tools.Js

open IronJS
open IronJS.Utils
open IronJS.Tools

let assign (left:Et) (right:Et) =
  Expr.assign left (Expr.cast left.Type right)

let box (expr:Et) =
  if expr.Type = Types.ClrVoid
    then Et.Block(expr, Expr.objDefault) :> Et 
    else Et.Convert(expr, typeof<obj>) :> Et

let doReturn label (value:Et) =
  Expr.gotoReturn label (box value)

let index (left:Et) (i:int64) =
  Et.ArrayIndex(left, Expr.constant i) :> Et