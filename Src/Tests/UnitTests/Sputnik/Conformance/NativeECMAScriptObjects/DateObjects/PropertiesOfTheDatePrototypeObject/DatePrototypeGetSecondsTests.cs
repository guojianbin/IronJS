// <auto-generated />
namespace IronJS.Tests.UnitTests.Sputnik.Conformance.NativeECMAScriptObjects.DateObjects.PropertiesOfTheDatePrototypeObject
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class DatePrototypeGetSecondsTests : SputnikTestFixture
    {
        public DatePrototypeGetSecondsTests()
            : base(@"Conformance\15_Native_ECMA_Script_Objects\15.9_Date_Objects\15.9.5_Properties_of_the_Date_Prototype_Object\15.9.5.22_Date.prototype.getSeconds")
        {
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 15.9.5.22")]
        [TestCase("S15.9.5.22_A1_T1.js", Description = "The Date.prototype property \"getSeconds\" has { DontEnum } attributes")]
        [TestCase("S15.9.5.22_A1_T2.js", Description = "The Date.prototype property \"getSeconds\" has { DontEnum } attributes")]
        [TestCase("S15.9.5.22_A1_T3.js", Description = "The Date.prototype property \"getSeconds\" has { DontEnum } attributes")]
        public void TheDatePrototypePropertyGetSecondsHasDontEnumAttributes(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 15.9.5.22")]
        [TestCase("S15.9.5.22_A2_T1.js", Description = "The \"length\" property of the \"getSeconds\" is 0")]
        public void TheLengthPropertyOfTheGetSecondsIs0(string file)
        {
            RunFile(file);
        }

        [Test]
        [Category("Sputnik Conformance")]
        [Category("ECMA 15.9.5.22")]
        [TestCase("S15.9.5.22_A3_T1.js", Description = "The Date.prototype.getSeconds property \"length\" has { ReadOnly, DontDelete, DontEnum } attributes")]
        [TestCase("S15.9.5.22_A3_T2.js", Description = "The Date.prototype.getSeconds property \"length\" has { ReadOnly, DontDelete, DontEnum } attributes")]
        [TestCase("S15.9.5.22_A3_T3.js", Description = "The Date.prototype.getSeconds property \"length\" has { ReadOnly, DontDelete, DontEnum } attributes")]
        public void TheDatePrototypeGetSecondsPropertyLengthHasReadOnlyDontDeleteDontEnumAttributes(string file)
        {
            RunFile(file);
        }
    }
}