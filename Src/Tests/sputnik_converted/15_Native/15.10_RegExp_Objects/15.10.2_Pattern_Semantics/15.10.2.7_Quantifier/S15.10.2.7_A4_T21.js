// Copyright 2009 the Sputnik authors.  All rights reserved.
// This code is governed by the BSD license found in the LICENSE file.

/**
* @name: S15.10.2.7_A4_T21;
* @section: 15.10.2.7;
* @assertion: The production QuantifierPrefix :: * evaluates by returning the two results 0 and \infty;
* @description: Execute /[xyz]*1/.test('a0.b2.c3') and check results;
*/


// Converted for Test262 from original Sputnik source

ES5Harness.registerTest( {
id: "S15.10.2.7_A4_T21",

path: "TestCases/15_Native/15.10_RegExp_Objects/15.10.2_Pattern_Semantics/15.10.2.7_Quantifier/S15.10.2.7_A4_T21.js",

assertion: "The production QuantifierPrefix :: * evaluates by returning the two results 0 and \\infty",

description: "Execute /[xyz]*1/.test(\'a0.b2.c3\') and check results",

test: function testcase() {
   __executed = /[xyz]*1/.test('a0.b2.c3');

//CHECK#1
if (__executed) {
	$ERROR('#1: /[xyz]*1/.test(\'a0.b2.c3\') === false');
}


 }
});
