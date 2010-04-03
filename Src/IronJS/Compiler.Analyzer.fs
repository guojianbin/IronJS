﻿module IronJS.Compiler.Analyzer

open IronJS
open IronJS.Utils
open IronJS.Types
open IronJS.Tools
open IronJS.Ast.Types

(*Checks if a local always will result in a Dynamic type*)
let private isDynamic (loc:Local) =
  match loc.UsedAs with
  | JsTypes.Double 
  | JsTypes.String
  | JsTypes.Object -> true && loc.InitUndefined
  | _ -> true

(*Checks if a local variable never is assigned to from another variable*)
let private isNotAssignedTo (var:Local) =
  var.UsedWith.Count = 0

(*Sets the Expr and UsedAs attributes of a variable*)
let private setType (name:string) (var:Local) (typ:JsTypes) =
  let expr = Expr.param name (match var.ClosureAccess with
                                 | Read | Write -> StrongBoxType.MakeGenericType(ToClr typ)
                                 | None -> ToClr typ)
  { var with UsedAs = typ; Expr = expr }

(*Get the type of a variable, evaluating it if necessary*)
let private getType name (vars:LocalMap) =

  let rec getType name (vars:LocalMap) (exclude:string Set) =
    let var = vars.[name]
    if exclude.Contains name then JsTypes.None
    elif not(var.Expr = null) then var.UsedAs 
    else var.UsedWith
          |> Set.map  (fun var -> getType var vars (exclude.Add name))
          |> Set.fold (fun typ state -> typ ||| state) var.UsedAs

  getType name vars Set.empty

(*Resolves the type of a variable and updates the map with it*)
let inline private resolveType name (vars:LocalMap) =
  Map.add name (setType name vars.[name] (getType name vars)) vars

(*Analyzes a scope *)
let analyze (scope:Scope) (types:ClrType list) = 
  { scope with 
      Locals =
        scope.Locals 
          |> Map.map (fun name var -> 
            if var.IsParameter then
              if var.ParamIndex < types.Length 
                then { var with UsedAs = var.UsedAs ||| ToJs types.[var.ParamIndex] } // We got an argument for this parameter
                else { setType name var JsTypes.Dynamic with ParamIndex = -1; InitUndefined = true; } // We didn't, means make it dynamic and demote to a normal local
            else 
              if   isDynamic var then setType name var JsTypes.Dynamic  // No need to resolve type, force it here
              elif isNotAssignedTo var then setType name var var.UsedAs // If it's not assigned from any variables
              else var // Needs to be resolved
            )
          |> fix (fun next locals -> 
              match Map.tryFindKey (fun _ var -> var.Expr = null) locals with
              | Option.None       -> locals // All variables have Exprs
              | Option.Some(name) -> next (resolveType name locals) // Key found, resolve its type
            )
  }
