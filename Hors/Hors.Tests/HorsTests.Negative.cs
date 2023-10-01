using System;
using System.Linq;
using Hors.Models;
using NUnit.Framework;

namespace Hors.Tests
{
    public partial class Tests
    {
        [Test]
        public void TestOnlyDigit()
        {
            var parser = new HorsTextParser();
            var result = parser.Parse("9", new DateTime(2019, 9, 2));
            
            Assert.AreEqual(0, result.Dates.Count);
        }
        
        [Test]
        public void TestNoTimeDigitsInText()
        {
            var parser = new HorsTextParser();
            var result = parser.Parse("Завтра в 12 перекинуть 10 рублей", new DateTime(2019, 9, 2));
            
            Assert.AreEqual(0, result.Dates.Count);
        }
    }
}