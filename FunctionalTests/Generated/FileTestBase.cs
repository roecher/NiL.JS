﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NiL.JS.Core;

namespace NiL.JS.Test.Generated
{
    public abstract class FileTestBase
    {
        private string _sta;

        protected void LoadSta(string path)
        {
            using (var f = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(f))
                _sta = sr.ReadToEnd();
        }

        protected void RunFile(string fileName)
        {
            string code;
            using (var f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(f))
                code = sr.ReadToEnd();

            var globalContext = new GlobalContext();

            try
            {
                globalContext.ActivateInCurrentThread();

                var output = new StringBuilder();
                var oldOutput = Console.Out;
                Console.SetOut(new StringWriter(output));
                var pass = true;
                Module module;
                var moduleName = fileName.Split(new[] { '/', '\\' }).Last();
                if (!string.IsNullOrEmpty(_sta))
                {
                    module = new Module(moduleName, _sta);
                    module.Run();
                }
                else
                {
                    module = new Module(moduleName, "");
                }

                var preambleEnd = 0;
                var preambleEndTemp = 0;
                do
                {
                    preambleEnd = preambleEndTemp;
                    try
                    {
                        preambleEndTemp = Parser.SkipComment(code, preambleEndTemp, true);
                    }
                    catch
                    {
                        break;
                    }
                }
                while (preambleEnd < preambleEndTemp);

                if (code.IndexOf("@negative") != -1)
                    System.Diagnostics.Debugger.Break();

                var negative = code.IndexOf("* @negative", 0, preambleEnd) != -1;
                var strict = code.IndexOf("* @onlyStrict", 0, preambleEnd) != -1;

                try
                {
                    try
                    {
                        module.Context.Eval(code, !strict);
                    }
                    finally
                    {
                        pass ^= negative;
                    }
                }
                catch (JSException e)
                {
                    pass = negative;
                    if (!pass)
                        output.Append(e);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debugger.Break();
                    output.Append(e);
                    pass = false;
                }
                finally
                {
                    Module.RemoveFromModuleCache(moduleName);
                    Console.SetOut(oldOutput);
                }

                Assert.IsTrue(pass, output.ToString());
                Assert.AreEqual(string.Empty, output.ToString().Trim());
            }
            finally
            {
                globalContext.Deactivate();
            }
        }
    }
}
