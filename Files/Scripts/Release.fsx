#light
#time
#r @"../../Lib/Antlr3.Runtime.dll"
#r @"../../Lib/CLR4/Xebic.ES3.dll"
#r @"../../Lib/CLR4/Microsoft.Dynamic.dll"
#r @"../../Src/Dependencies/FSKit/Src/bin/Release/FSKit.dll"
#r @"../../Src/Core/IronJS/bin/Release/IronJS.dll"
#r @"../../Src/Core/IronJS.Compiler/bin/Release/IronJS.Compiler.dll"
#r @"../../Src/Core/IronJS.Runtime/bin/Release/IronJS.Runtime.dll"

open System
open System.Reflection
open IronJS
open FSKit

let ctx = Hosting.Context.Create()

let print0 = fun (box:Box) -> printfn "%s" (Api.TypeConverter.toString(box))
let print1 = new Action<IronJS.Box>(print0)
let print2 = IronJS.Api.HostFunction.create ctx.Environment print1
ctx.PutGlobal("print", print2)

Debug.exprPrinters.Add (new Action<Dlr.Expr>(Dlr.Utils.printDebugView))

let sw = new System.Diagnostics.Stopwatch();
sw.Start();
let result = ctx.Execute @"(function(){
function fannkuch(n) {
   var check = 0;
   var perm = Array(n);
   var perm1 = Array(n);
   var count = Array(n);
   var maxPerm = Array(n);
   var maxFlipsCount = 0;
   var m = n - 1;

   for (var i = 0; i < n; i++) perm1[i] = i;
   var r = n;

   while (true) {
      // write-out the first 30 permutations
      if (check < 30){
         var s = '';
         for(var i=0; i<n; i++) s += (perm1[i]+1).toString();
         check++;
      }

      while (r != 1) { count[r - 1] = r; r--; }
      if (!(perm1[0] == 0 || perm1[m] == m)) {
         for (var i = 0; i < n; i++) perm[i] = perm1[i];

         var flipsCount = 0;
         var k;

         while (!((k = perm[0]) == 0)) {
            var k2 = (k + 1) >> 1;
            for (var i = 0; i < k2; i++) {
               var temp = perm[i]; perm[i] = perm[k - i]; perm[k - i] = temp;
            }
            flipsCount++;
         }

         if (flipsCount > maxFlipsCount) {
            maxFlipsCount = flipsCount;
            for (var i = 0; i < n; i++) maxPerm[i] = perm1[i];
         }
      }

      while (true) {
         if (r == n) return maxFlipsCount;
         var perm0 = perm1[0];
         var i = 0;
         while (i < r) {
            var j = i + 1;
            perm1[i] = perm1[j];
            i = j;
         }
         perm1[r] = perm0;

         count[r] = count[r] - 1;
         if (count[r] > 0) break;
         r++;
      }
   }
}

var n = 8;
var ret = fannkuch(n);
})();
"
sw.Stop();

printfn "%i" sw.ElapsedMilliseconds