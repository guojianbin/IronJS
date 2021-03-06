// <auto-generated />
namespace IronJS.Tests.UnitTests.Sputnik.Conformance.Expressions.EqualityOperators
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TheEqualsOperatorTests : SputnikTestFixture
    {
        public TheEqualsOperatorTests()
            : base(@"Conformance\11_Expressions\11.9_Equality_Operators\11.9.1_The_Equals_Operator")
        {
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 7.2")]
        [Category("ECMA 7.3")]
        [TestCase("S11.9.1_A1.js", Description = "White Space and Line Terminator between EqualityExpression and \"==\" or between \"==\" and RelationalExpression are allowed")]
        public void WhiteSpaceAndLineTerminatorBetweenEqualityExpressionAndOrBetweenAndRelationalExpressionAreAllowed(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [TestCase("S11.9.1_A2.1_T1.js", Description = "Operator x == y uses GetValue")]
        [TestCase("S11.9.1_A2.1_T2.js", Description = "Operator x == y uses GetValue")]
        [TestCase("S11.9.1_A2.1_T3.js", Description = "Operator x == y uses GetValue")]
        public void OperatorXYUsesGetValue(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [TestCase("S11.9.1_A2.4_T1.js", Description = "First expression is evaluated first, and then second expression")]
        [TestCase("S11.9.1_A2.4_T2.js", Description = "First expression is evaluated first, and then second expression")]
        [TestCase("S11.9.1_A2.4_T3.js", Description = "First expression is evaluated first, and then second expression")]
        public void FirstExpressionIsEvaluatedFirstAndThenSecondExpression(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A3.1.js", Description = "Return true, if x and y are both true or both false")]
        public void ReturnTrueIfXAndYAreBothTrueOrBothFalse(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A3.2.js", Description = "If Type(x) is Boolean and Type(y) is Number, return the result of comparison ToNumber(x) == y")]
        public void IfTypeXIsBooleanAndTypeYIsNumberReturnTheResultOfComparisonToNumberXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A3.3.js", Description = "If Type(y) is Number and Type(y) is Boolean, return the result of comparison x == ToNumber(y)")]
        public void IfTypeYIsNumberAndTypeYIsBooleanReturnTheResultOfComparisonXToNumberY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A4.1_T1.js", Description = "If x or y is NaN, return false")]
        [TestCase("S11.9.1_A4.1_T2.js", Description = "If x or y is NaN, return false")]
        public void IfXOrYIsNaNReturnFalse(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A4.2.js", Description = "If x is +0(-0) and y is -0(+0), return true")]
        public void IfXIs00AndYIs00ReturnTrue(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A4.3.js", Description = "Type(x) and Type(y) are Number-s minus NaN, +0, -0. Return true, if x is the same number value as y")]
        public void TypeXAndTypeYAreNumberSMinusNaN00ReturnTrueIfXIsTheSameNumberValueAsY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A5.1.js", Description = "Type(x) and Type(y) are String-s. Return true, if x and y are exactly the same sequence of characters")]
        public void TypeXAndTypeYAreStringSReturnTrueIfXAndYAreExactlyTheSameSequenceOfCharacters(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A5.2.js", Description = "If Type(x) is Number and Type(y) is String, return the result of comparison x == ToNumber(y)")]
        public void IfTypeXIsNumberAndTypeYIsStringReturnTheResultOfComparisonXToNumberY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A5.3.js", Description = "If Type(x) is String and Type(y) is Number, return the result of comparison ToNumber(x) == y")]
        public void IfTypeXIsStringAndTypeYIsNumberReturnTheResultOfComparisonToNumberXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A6.1.js", Description = "If Type(x) as well as Type(y) is undefined or null, return true")]
        public void IfTypeXAsWellAsTypeYIsUndefinedOrNullReturnTrue(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A6.2_T1.js", Description = "If one expression is undefined or null and another is not, return false")]
        [TestCase("S11.9.1_A6.2_T2.js", Description = "If one expression is undefined or null and another is not, return false")]
        public void IfOneExpressionIsUndefinedOrNullAndAnotherIsNotReturnFalse(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.1.js", Description = "Type(x) and Type(y) are Object-s. Return true, if x and y are references to the same Object")]
        public void TypeXAndTypeYAreObjectSReturnTrueIfXAndYAreReferencesToTheSameObject(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.2.js", Description = "If Type(x) is Object and Type(y) is Boolean, return ToPrimitive(x) == y")]
        public void IfTypeXIsObjectAndTypeYIsBooleanReturnToPrimitiveXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.3.js", Description = "If Type(x) is Boolean and Type(y) is Object, return x == ToPrimitive(y)")]
        public void IfTypeXIsBooleanAndTypeYIsObjectReturnXToPrimitiveY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.4.js", Description = "If Type(x) is Object and Type(y) is Number, return ToPrimitive(x) == y")]
        public void IfTypeXIsObjectAndTypeYIsNumberReturnToPrimitiveXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.5.js", Description = "If Type(x) is Number and Type(y) is Object, return x == ToPrimitive(y)")]
        public void IfTypeXIsNumberAndTypeYIsObjectReturnXToPrimitiveY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.6.js", Description = "If Type(x) is Object and Type(y) is String, return ToPrimitive(x) == y")]
        public void IfTypeXIsObjectAndTypeYIsStringReturnToPrimitiveXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.7.js", Description = "If Type(x) is String and Type(y) is Object, return x == ToPrimitive(y)")]
        public void IfTypeXIsStringAndTypeYIsObjectReturnXToPrimitiveY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.8.js", Description = "If Type(x) is Object and Type(y) is primitive type, return ToPrimitive(x) == y")]
        public void IfTypeXIsObjectAndTypeYIsPrimitiveTypeReturnToPrimitiveXY(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 11.9.1")]
        [Category("ECMA 11.9.3")]
        [TestCase("S11.9.1_A7.9.js", Description = "If Type(x) is primitive type and Type(y) is Object, return x == ToPrimitive(y)")]
        public void IfTypeXIsPrimitiveTypeAndTypeYIsObjectReturnXToPrimitiveY(string file)
        {
            RunFile(file);
        }
    }
}