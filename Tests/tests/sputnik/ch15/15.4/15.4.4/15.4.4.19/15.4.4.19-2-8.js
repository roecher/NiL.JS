/// Copyright (c) 2012 Ecma International.  All rights reserved. 
/**
 * @path ch15/15.4/15.4.4/15.4.4.19/15.4.4.19-2-8.js
 * @description Array.prototype.map - applied to Array-like object, 'length' is an own accessor property that overrides an inherited data property
 */


function testcase() {
        function callbackfn(val, idx, obj) {
            return val > 10;
        }

        var proto = { length: 3 };

        var Con = function () { };
        Con.prototype = proto;

        var child = new Con();

        Object.defineProperty(child, "length", {
            get: function () {
                return 2;
            },
            configurable: true
        });

        child[0] = 12;
        child[1] = 11;
        child[2] = 9;

        var testResult = Array.prototype.map.call(child, callbackfn);

        return testResult.length === 2;
    }
runTestCase(testcase);
