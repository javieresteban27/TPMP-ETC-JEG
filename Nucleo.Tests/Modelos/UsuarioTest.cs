using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;
using Nucleo.Seguridad;

namespace NucleoTest.Modelos
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void UsuarioNuevo_NoEstaBloqueado()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsFalse(usuario.EstaBloqueado());
            Assert.AreEqual(0, usuario.Intentos_fallidos);
            Assert.IsNull(usuario.Bloqueado_hasta);
        }

        [TestMethod]
        public void EstaBloqueado_BloqueoVencido_LimpiaElBloqueoYLosIntentos()
        {
            Usuario usuario = CrearUsuario();
            usuario.Intentos_fallidos = 3;
            usuario.Bloqueado_hasta = DateTime.Now.AddMinutes(-1);

            Assert.IsFalse(usuario.EstaBloqueado());
            Assert.AreEqual(0, usuario.Intentos_fallidos);
            Assert.IsNull(usuario.Bloqueado_hasta);
        }

        [TestMethod]
        public void RegistrarFallo_DosIntentos_NoBloqueaLaCuenta()
        {
            Usuario usuario = CrearUsuario();

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();

            Assert.AreEqual(2, usuario.Intentos_fallidos);
            Assert.IsFalse(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarFallo_TercerIntento_BloqueaLaCuentaDuranteCincoMinutos()
        {
            Usuario usuario = CrearUsuario();
            DateTime antes = DateTime.Now;

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();
            usuario.RegistrarFallo();

            DateTime despues = DateTime.Now;

            Assert.AreEqual(3, usuario.Intentos_fallidos);
            Assert.IsTrue(usuario.Bloqueado_hasta >= antes.AddMinutes(5));
            Assert.IsTrue(usuario.Bloqueado_hasta <= despues.AddMinutes(5));
            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarFallo_CuentaBloqueada_NoIncrementaIntentos()
        {
            Usuario usuario = CrearUsuario();
            usuario.Intentos_fallidos = 3;
            usuario.Bloqueado_hasta = DateTime.Now.AddMinutes(5);

            usuario.RegistrarFallo();

            Assert.AreEqual(3, usuario.Intentos_fallidos);
            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarExito_RestableceIntentosYBloqueo()
        {
            Usuario usuario = CrearUsuario();
            usuario.Intentos_fallidos = 3;
            usuario.Bloqueado_hasta = DateTime.Now.AddMinutes(5);

            usuario.RegistrarExito();

            Assert.AreEqual(0, usuario.Intentos_fallidos);
            Assert.IsNull(usuario.Bloqueado_hasta);
            Assert.IsFalse(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void CompruebaPassword_ContraseñaIncorrecta_IncrementaIntentos()
        {
            Usuario usuario = CrearUsuario("ClaveCorrecta123!");

            bool resultado = usuario.CompruebaPassword("ClaveIncorrecta123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(1, usuario.Intentos_fallidos);
        }

        [TestMethod]
        public void CompruebaPassword_ContraseñaCorrecta_RestableceIntentosYActualizaAccesos()
        {
            Usuario usuario = CrearUsuario("ClaveCorrecta123!");
            DateTime accesoAnterior = DateTime.Now.AddDays(-1);
            DateTime antesDeComprobar = DateTime.Now;
            usuario.Intentos_fallidos = 2;
            usuario.Fecha_acceso_actual = accesoAnterior;

            bool resultado = usuario.CompruebaPassword("ClaveCorrecta123!");

            DateTime despuesDeComprobar = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.AreEqual(0, usuario.Intentos_fallidos);
            Assert.IsNull(usuario.Bloqueado_hasta);
            Assert.AreEqual(accesoAnterior, usuario.Fecha_ultimo_acceso);
            Assert.IsTrue(usuario.Fecha_acceso_actual >= antesDeComprobar);
            Assert.IsTrue(usuario.Fecha_acceso_actual <= despuesDeComprobar);
        }

        [TestMethod]
        public void CompruebaPassword_CuentaSinContraseña_NoVerificaLaContraseña()
        {
            Usuario usuario = CrearUsuario("ClaveCorrecta123!");
            usuario.SinContraseña = true;

            bool resultado = usuario.CompruebaPassword("ClaveCorrecta123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(0, usuario.Intentos_fallidos);
        }

        [TestMethod]
        public void CompruebaPassword_CuentaBloqueada_NoVerificaLaContraseña()
        {
            Usuario usuario = CrearUsuario("ClaveCorrecta123!");
            usuario.Intentos_fallidos = 3;
            usuario.Bloqueado_hasta = DateTime.Now.AddMinutes(5);

            bool resultado = usuario.CompruebaPassword("ClaveCorrecta123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(3, usuario.Intentos_fallidos);
        }

        [TestMethod]
        public void CambiaPassword_SinContraseñaYContraseñaValida_CambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();
            usuario.SinContraseña = true;
            DateTime antes = DateTime.Now;

            bool resultado = usuario.CambiaPassword("NuevaClave123!");

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.IsFalse(usuario.SinContraseña);
            Assert.IsTrue(ServicioHash.Verificar("NuevaClave123!", usuario.Password));
            Assert.IsTrue(usuario.Ultimo_cambio_password >= antes);
            Assert.IsTrue(usuario.Ultimo_cambio_password <= despues);
        }

        [TestMethod]
        public void CambiaPassword_SinContraseñaYTextoVacio_NoCambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();
            usuario.SinContraseña = true;

            Assert.IsFalse(usuario.CambiaPassword(""));
            Assert.IsFalse(usuario.CambiaPassword("   "));
            Assert.IsTrue(usuario.SinContraseña);
        }

        [TestMethod]
        public void CambiaPassword_ConContraseñaAnteriorCorrecta_CambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario("ClaveAnterior123!");
            DateTime antes = DateTime.Now;

            bool resultado = usuario.CambiaPassword(
                "ClaveAnterior123!",
                "ClaveNueva456!");

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveNueva456!", usuario.Password));
            Assert.IsFalse(ServicioHash.Verificar("ClaveAnterior123!", usuario.Password));
            Assert.IsTrue(usuario.Ultimo_cambio_password >= antes);
            Assert.IsTrue(usuario.Ultimo_cambio_password <= despues);
        }

        [TestMethod]
        public void CambiaPassword_ContraseñaNuevaIgualALaAnterior_NoLaCambia()
        {
            Usuario usuario = CrearUsuario("ClaveActual123!");

            bool resultado = usuario.CambiaPassword(
                "ClaveActual123!",
                "ClaveActual123!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveActual123!", usuario.Password));
        }

        [TestMethod]
        public void CambiaPassword_ContraseñaAnteriorIncorrecta_NoLaCambia()
        {
            Usuario usuario = CrearUsuario("ClaveActual123!");

            bool resultado = usuario.CambiaPassword(
                "ClaveIncorrecta123!",
                "ClaveNueva456!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveActual123!", usuario.Password));
            Assert.AreEqual(1, usuario.Intentos_fallidos);
        }

        [TestMethod]
        public void CambiaPassword_ReutilizaContraseñaAnterior_NoLaCambia()
        {
            Usuario usuario = CrearUsuario("ClaveInicial123!");
            Assert.IsTrue(usuario.CambiaPassword("ClaveInicial123!", "ClaveNueva456!"));

            bool resultado = usuario.CambiaPassword(
                "ClaveNueva456!",
                "ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveNueva456!", usuario.Password));
        }

        private static Usuario CrearUsuario(string password = "ClaveInicial123!")
        {
            return new Usuario
            {
                Nombre_usuario = "usuario_test",
                Mail = "usuario@test.com",
                Password = ServicioHash.Generar(password)
            };
        }
    }
}