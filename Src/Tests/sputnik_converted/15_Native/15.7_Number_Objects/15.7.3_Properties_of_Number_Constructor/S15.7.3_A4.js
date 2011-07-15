// Copyright 2009 the Sputnik authors.  All rights reserved.
// This code is governed by the BSD license found in the LICENSE file.

/**
 * @name: S15.7.3_A4;
 * @section: 15.7.3;
 * @assertion: The Number constructor has the property "NaN";
 * @description: Checking existence of the property "NaN";
 */


// Converted for Test262 from original Sputnik source

ES5Harness.registerTest( {
id: "S15.7.3_A4",

path: "TestCases/15_Native/15.7_Number_Objects/15.7.3_Properties_of_Number_Constructor/S15.7.3_A4.js",

assertion: "The Number constructor has the property \"NaN\"",

description: "Checking existence of the property \"NaN\"",

test: function testcase() {
   if(!Number.hasOwnProperty("NaN")){
  $ERROR('#1: The Number constructor has the property "NaN"');
}


 }
});
