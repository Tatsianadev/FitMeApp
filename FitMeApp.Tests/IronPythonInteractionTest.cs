using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace FitMeApp.Tests
{
    [TestFixture]
    class IronPythonInteractionTest
    {
        [Test]
        public void DoArithmeticOperation()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string fullPath = @"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\Python\FitMePythonTest.py";
            //scope.SetVariable("a", 5);
            //scope.SetVariable("b", 10);
            engine.ExecuteFile(fullPath, scope);
            var function = scope.GetVariable("universalFunction");
            var sumResult = function(5, 10, "+");
            var mulResult = function(5, 10, "*");
            var difResult = function(5, 10, "-");
            var divResult = function(5, 10, "/");

            Assert.AreEqual(15, sumResult);
            Assert.AreEqual(50, mulResult);
            Assert.AreEqual(-5, difResult);
            Assert.AreEqual(0.5, divResult);
        }
    }
}
