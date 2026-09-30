using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Seguridad;

namespace NucleoTest.Seguridad
{
    [TestClass]
    public class ServicioHashTest
    {
        [TestMethod]
        public void Generar_TextoNuloOBlanco_LanzaArgumentException()
        {
            try
            {
                ServicioHash.Generar(null!);
                Assert.Fail("Se esperaba ArgumentException al pasar null.");
            }
            catch (ArgumentException) { }

            try
            {
                ServicioHash.Generar(string.Empty);
                Assert.Fail("Se esperaba ArgumentException al pasar cadena vacía.");
            }
            catch (ArgumentException) { }

            try
            {
                ServicioHash.Generar("   ");
                Assert.Fail("Se esperaba ArgumentException al pasar cadena en blanco.");
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void Generar_TextoValido_RetornaHashYSalNoVacios()
        {
            var resultado = ServicioHash.Generar("contraseñaSegura123!");
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Hash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultado.Sal));
        }

        [TestMethod]
        public void Verificar_ConHashYSalCorrectos_DevuelveTrue()
        {
            var resultado = ServicioHash.Generar("miPass123");
            Assert.IsTrue(ServicioHash.Verificar("miPass123", resultado.Hash, resultado.Sal));
            Assert.IsTrue(ServicioHash.Verificar("miPass123", resultado));
        }

        [TestMethod]
        public void Verificar_ConTextoIncorrecto_DevuelveFalse()
        {
            var resultado = ServicioHash.Generar("miPass123");
            Assert.IsFalse(ServicioHash.Verificar("otraPass", resultado.Hash, resultado.Sal));
            Assert.IsFalse(ServicioHash.Verificar("otraPass", resultado));
        }

        [TestMethod]
        public void Verificar_EntradasNulasOBlancas_DevuelveFalse()
        {
            var resultado = ServicioHash.Generar("algo");

            Assert.IsFalse(ServicioHash.Verificar(null!, resultado.Hash, resultado.Sal));
            Assert.IsFalse(ServicioHash.Verificar(string.Empty, resultado.Hash, resultado.Sal));
            Assert.IsFalse(ServicioHash.Verificar("  ", resultado.Hash, resultado.Sal));

            Assert.IsFalse(ServicioHash.Verificar(null!, resultado));
            Assert.IsFalse(ServicioHash.Verificar(string.Empty, resultado));
        }

        [TestMethod]
        public void Verificar_HashOSalNoBase64_DevuelveFalse()
        {
            Assert.IsFalse(ServicioHash.Verificar("x", "no-es-base64", "no-es-base64"));
            var malFormado = new ResultadoHash { Hash = "no-base64", Sal = "no-base64" };
            Assert.IsFalse(ServicioHash.Verificar("x", malFormado));
        }

        [TestMethod]
        public void Generar_MismoTexto_DiferentesSaltYHash()
        {
            var a = ServicioHash.Generar("igualPassword");
            var b = ServicioHash.Generar("igualPassword");

            // Es extremadamente improbable que coincidan ambos valores.
            Assert.AreNotEqual(a.Sal, b.Sal);
            Assert.AreNotEqual(a.Hash, b.Hash);
        }
    }
}