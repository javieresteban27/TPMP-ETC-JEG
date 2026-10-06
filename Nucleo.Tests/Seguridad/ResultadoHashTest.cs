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
            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            Assert.IsTrue(resultado.Equals(resultado));
        }

        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            Assert.IsFalse(resultado.Equals((ResultadoHash?)null));
        }

        [TestMethod]
        public void Equals_SameHashAndSalDifferentCreacion_ReturnsTrue()
        {
            var a = new ResultadoHash
            {
                Hash = "h",
                Sal = "s",
                Creacion = DateTime.Now.AddDays(-1)
            };

            var b = new ResultadoHash
            {
                Hash = "h",
                Sal = "s",
                Creacion = DateTime.Now
            };

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
            Assert.IsTrue(a == b);
            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void Equals_DifferentHash_ReturnsFalse()
        {
            var a = new ResultadoHash
            {
                Hash = "h1",
                Sal = "s"
            };

            var b = new ResultadoHash
            {
                Hash = "h2",
                Sal = "s"
            };

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void Equals_DifferentSal_ReturnsFalse()
        {
            var a = new ResultadoHash
            {
                Hash = "h",
                Sal = "s1"
            };

            var b = new ResultadoHash
            {
                Hash = "h",
                Sal = "s2"
            };

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void Equals_ObjectSameValues_ReturnsTrue()
        {
            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            object otro = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            Assert.IsTrue(resultado.Equals(otro));
        }

        [TestMethod]
        public void Equals_ObjectDifferentType_ReturnsFalse()
        {
            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            object otro = "texto";

            Assert.IsFalse(resultado.Equals(otro));
        }

        [TestMethod]
        public void Equals_ObjectNull_ReturnsFalse()
        {
            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            object? otro = null;

            Assert.IsFalse(resultado.Equals(otro));
        }

        [TestMethod]
        public void GetHashCode_EqualObjects_SameHashCode()
        {
            var a = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            var b = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

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

            var b = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void OperatorEquality_RightNullLeftNotNull_ReturnsFalse()
        {
            var a = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            ResultadoHash? b = null;

            Assert.IsFalse(a == b);
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void Creacion_PorDefecto_SeInicializaConFechaActual()
        {
            DateTime antes = DateTime.Now;

            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s"
            };

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado.Creacion >= antes);
            Assert.IsTrue(resultado.Creacion <= despues);
        }

        [TestMethod]
        public void Creacion_ValorAsignado_SeConserva()
        {
            DateTime fecha = new DateTime(
                2026, 10, 6, 12, 30, 0);

            var resultado = new ResultadoHash
            {
                Hash = "h",
                Sal = "s",
                Creacion = fecha
            };

            Assert.AreEqual(fecha, resultado.Creacion);
        }

        [TestMethod]
        public void ValoresPorDefecto_HashYSalSonCadenasVacias()
        {
            var resultado = new ResultadoHash();

            Assert.AreEqual(string.Empty, resultado.Hash);
            Assert.AreEqual(string.Empty, resultado.Sal);
        }
    }
}