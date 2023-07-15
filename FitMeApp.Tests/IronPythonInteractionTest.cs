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
            //var resultGoal = scope.GetVariable("goal");


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

        [Test]
        public void FindProductNameByStartWith()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string fullPath = @"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\Python\DietJournal.py";
            string findByLetters = "ba";
            engine.ExecuteFile(fullPath, scope);
            string allProductsFile = scope.GetVariable("path");
            var function = scope.GetVariable("findNamesByStartWith");
            var result = function(allProductsFile, findByLetters);
            string name = result[0];

            Assert.AreEqual("banana", name);
        }

        [Test]
        public void FindProductByName()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string fullPath = @"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\Python\DietJournal.py";
            string nameToFind = "Banana";
            engine.ExecuteFile(fullPath, scope);
            string allProductsFile = scope.GetVariable("path");
            var function = scope.GetVariable("findProduct");
            var result = function(allProductsFile, nameToFind);
            var name = result["name"];

            Assert.AreEqual("banana", name);
        }
    }
}
