/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.4/15.4.4/15.4.4.17/15.4.4.17-4-9.js
 * @description Array.prototype.some - side effects produced by step 3 are visible when an exception occurs
 */


function testcase() {

        var obj = { 0: 11, 1: 12 };

        var accessed = false;

        Object.defineProperty(obj, "length", {
            get: function () {
                return {
                    toString: function () {
                        accessed = true;
                        return "2";
                    }
                };
            },
            configurable: true
        });

        try {
            Array.prototype.some.call(obj, null);
            return false;
        } catch (ex) {
            return ex instanceof TypeError && accessed;
        }
    }
runTestCase(testcase);
