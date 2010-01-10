﻿using System;

namespace IronJS.Runtime
{
    public static class BuiltIns
    {
        public static object Add(double left, double right)
        {
            return (double) left + (double) right;
        }

        public static object Print(object o)
        {
            Console.WriteLine(o);
            return o;
        }

        public static object UnsignedRightShift(int left, int right)
        {
            // 11.7.3
            return (double)(((uint)left) >> right);
        }

        public static object StrictEquality(object left, object right)
        {
            // 11.9.6

            // step 1
            if (left.GetType() != right.GetType())
                return false;

            // step 2
            if (left is Js.Undefined)
                return true;

            // step 3
            if (left == null && right == null) return true;
            if (left == null) return false;
            if (right == null) return false;

            // step 5-10
            if (left is double)
                return (double)left == (double)right;

            // step 11
            if (left is string)
                return (string)left == (string)right;

            // step 12
            if (left is bool)
                return (bool)left == (bool)right;

            // step 13
            return object.ReferenceEquals(left, right);
        }

        public static object TypeOf(object value)
        {
            // 11.4.3

            if (value == null)
                return "object";

            if (value is Js.Undefined)
                return "undefined";

            if (value is bool)
                return "boolean";

            if (value is double)
                return "number";

            if (value is string)
                return "string";

            if (value is Js.Obj)
            {
                if (((Js.Obj)value).Class == Js.ObjClass.Function)
                    return "function";

                return "object";
            }

            return "dotnet";
        }
    }
}
