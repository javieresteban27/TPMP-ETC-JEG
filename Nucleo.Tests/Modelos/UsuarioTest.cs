using System;
using System.Collections.Generic;
using System.Text;
using Nucleo.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nucleo.Tests.Modelos
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void UsuarioNuevo_NoDebeEstarBloqueado()
        {
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_test",
                Mail = "usuario@test.com",
                Password = "1234@AEiou"
            };

            // Asserts
            Assert.IsFalse(usuario.EstaBloqueado());
            Assert.AreEqual(0, usuario.Intentos_fallidos);
        }

        [TestMethod]
        public void RegistrarFallo_IncrementaIntentosSinBloquearInmediatamente()
        {
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_test",
                Mail = "usuario@test.com",
                Password = "1234@AEiou"
            };

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();

            Assert.AreEqual(2, usuario.Intentos_fallidos);
            Assert.IsFalse(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarFallo_TresIntentos_Fallidos_BloqueaLaCuenta()
        {
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_test",
                Mail = "usuario@test.com",
                Password = "1234@AEiou"
            };

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();
            usuario.RegistrarFallo(); // Llega a 3 y debería bloquear

            // Assert
            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarExito_ReseteaIntentosYBloqueo()
        {
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_test",
                Mail = "usuario@test.com",
                Password = "1234@AEiou"
            };

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();
            usuario.RegistrarExito();

            Assert.AreEqual(0, usuario.Intentos_fallidos);
            Assert.IsFalse(usuario.EstaBloqueado());
        }
    }
}