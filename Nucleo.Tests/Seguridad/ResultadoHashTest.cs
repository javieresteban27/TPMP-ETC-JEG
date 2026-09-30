using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Seguridad;

namespace NucleoTest.Seguridad
{
    [TestClass]
    public class ResultadoHashTest
    {
        [TestMethod]
        public void Equals_SameReference_ReturnsTrue()
        {
            var r = new ResultadoHash { Hash = "h", Sal = "s" };
            Assert.IsTrue(r.Equals(r));
        }

        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            var r = new ResultadoHash { Hash = "h", Sal = "s" };
            Assert.IsFalse(r.Equals(null));
        }

        [TestMethod]
        public void Equals_SameHashAndSalDifferentCreacion_ReturnsTrue()
        {
            var a = new ResultadoHash { Hash = "h", Sal = "s" };
            var b = new ResultadoHash { Hash = "h", Sal = "s" };
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void Equals_DifferentHash_ReturnsFalse()
        {
            var a = new ResultadoHash { Hash = "h1", Sal = "s" };
            var b = new ResultadoHash { Hash = "h2", Sal = "s" };
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void Equals_DifferentSal_ReturnsFalse()
        {
            var a = new ResultadoHash { Hash = "h", Sal = "s1" };
            var b = new ResultadoHash { Hash = "h", Sal = "s2" };
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_EqualObjects_SameHashCode()
        {
            var a = new ResultadoHash { Hash = "h", Sal = "s" };
            var b = new ResultadoHash { Hash = "h", Sal = "s" };
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void OperatorEquality_BothNull_ReturnsTrue()
        {
            ResultadoHash? a = null;
            ResultadoHash? b = null;
            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void OperatorEquality_LeftNullRightNotNull_ReturnsFalse()
        {
            ResultadoHash? a = null;
            var b = new ResultadoHash { Hash = "h", Sal = "s" };
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void OperatorEquality_RightNullLeftNotNull_ReturnsFalse()
        {
            var a = new ResultadoHash { Hash = "h", Sal = "s" };
            ResultadoHash? b = null;
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }
    }
}