using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;
using Nucleo.Seguridad;

namespace NucleoTest.Modelos
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void UsuarioNuevo_InicializaFechasYEstadoEsperados()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsTrue(usuario.Activa);
            Assert.AreEqual(0, usuario.IntentosErroneos);
            Assert.AreEqual(DateTime.MinValue, usuario.FechaUltimoAcceso);
            Assert.AreEqual(DateTime.MinValue, usuario.FechaAccesoActual);
            Assert.AreEqual(DateTime.MinValue, usuario.UltimoCambioPassword);
            Assert.AreEqual(DateTime.MinValue, usuario.BloquearAccesoHasta);
            Assert.AreEqual(DateTime.MinValue, usuario.CaducidadCuenta);
            Assert.AreEqual(0, usuario.ListaPasswordsCount);
        }

        [TestMethod]
        public void EstaBloqueado_SinBloqueoActivo_DevuelveFalse()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsFalse(usuario.EstaBloqueado());
            Assert.AreEqual(0, usuario.IntentosErroneos);
            Assert.AreEqual(DateTime.MinValue, usuario.BloquearAccesoHasta);
        }

        [TestMethod]
        public void EstaBloqueado_BloqueoVigente_DevuelveTrue()
        {
            Usuario usuario = CrearUsuario();
            usuario.BloquearAccesoHasta = DateTime.Now.AddMinutes(1);

            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void EstaBloqueado_BloqueoVencido_LimpiaBloqueoEIntentos()
        {
            Usuario usuario = CrearUsuario();
            usuario.IntentosErroneos = 3;
            usuario.BloquearAccesoHasta = DateTime.Now.AddMinutes(-1);

            Assert.IsFalse(usuario.EstaBloqueado());
            Assert.AreEqual(0, usuario.IntentosErroneos);
            Assert.AreEqual(DateTime.MinValue, usuario.BloquearAccesoHasta);
        }

        [TestMethod]
        public void RegistrarFallo_DosIntentos_NoBloqueaLaCuenta()
        {
            Usuario usuario = CrearUsuario();

            usuario.RegistrarFallo();
            usuario.RegistrarFallo();

            Assert.AreEqual(2, usuario.IntentosErroneos);
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

            Assert.AreEqual(3, usuario.IntentosErroneos);
            Assert.IsTrue(usuario.BloquearAccesoHasta >= antes.AddMinutes(1));
            Assert.IsTrue(usuario.BloquearAccesoHasta <= despues.AddMinutes(1));
            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarFallo_CuentaBloqueada_NoIncrementaIntentos()
        {
            Usuario usuario = CrearUsuario();
            usuario.IntentosErroneos = 3;
            usuario.BloquearAccesoHasta = DateTime.Now.AddMinutes(5);

            usuario.RegistrarFallo();

            Assert.AreEqual(3, usuario.IntentosErroneos);
            Assert.IsTrue(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void RegistrarExito_RestableceIntentosYBloqueo()
        {
            Usuario usuario = CrearUsuario();
            usuario.IntentosErroneos = 3;
            usuario.BloquearAccesoHasta = DateTime.Now.AddMinutes(5);

            usuario.RegistrarExito();

            Assert.AreEqual(0, usuario.IntentosErroneos);
            Assert.AreEqual(DateTime.MinValue, usuario.BloquearAccesoHasta);
            Assert.IsFalse(usuario.EstaBloqueado());
        }

        [TestMethod]
        public void CompruebaPassword_CuentaSinCredenciales_NoIncrementaIntentos()
        {
            Usuario usuario = CrearUsuario(credencialesConfiguradas: false);

            bool resultado = usuario.CompruebaPassword("ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(0, usuario.IntentosErroneos);
        }

        [TestMethod]
        public void CompruebaPassword_ContraseñaIncorrecta_IncrementaIntentos()
        {
            Usuario usuario = CrearUsuario();

            bool resultado = usuario.CompruebaPassword("ClaveIncorrecta123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(1, usuario.IntentosErroneos);
        }

        [TestMethod]
        public void CompruebaPassword_ContraseñaCorrecta_RestableceIntentosYActualizaAccesos()
        {
            Usuario usuario = CrearUsuario();
            DateTime accesoAnterior = DateTime.Now.AddDays(-1);
            DateTime antes = DateTime.Now;
            usuario.IntentosErroneos = 2;
            usuario.FechaAccesoActual = accesoAnterior;

            bool resultado = usuario.CompruebaPassword("ClaveInicial123!");

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.AreEqual(0, usuario.IntentosErroneos);
            Assert.AreEqual(DateTime.MinValue, usuario.BloquearAccesoHasta);
            Assert.AreEqual(accesoAnterior, usuario.FechaUltimoAcceso);
            Assert.IsTrue(usuario.FechaAccesoActual >= antes);
            Assert.IsTrue(usuario.FechaAccesoActual <= despues);
        }

        [TestMethod]
        public void CompruebaPassword_CuentaBloqueada_NoCompruebaLaContraseña()
        {
            Usuario usuario = CrearUsuario();
            usuario.IntentosErroneos = 3;
            usuario.BloquearAccesoHasta = DateTime.Now.AddMinutes(1);

            bool resultado = usuario.CompruebaPassword("ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.AreEqual(3, usuario.IntentosErroneos);
        }

        [TestMethod]
        public void CambiaPassword_CuentaSinCredencialesYClaveValida_ConfiguraLaContraseña()
        {
            Usuario usuario = CrearUsuario(credencialesConfiguradas: false);
            DateTime antes = DateTime.Now;

            bool resultado = usuario.CambiaPassword("ClaveNueva456!");

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.IsTrue(usuario.TieneCredencialesConfiguradas);
            Assert.IsTrue(ServicioHash.Verificar("ClaveNueva456!", usuario.Password));
            Assert.IsTrue(usuario.UltimoCambioPassword >= antes);
            Assert.IsTrue(usuario.UltimoCambioPassword <= despues);
            Assert.AreEqual(1, usuario.ListaPasswordsCount);
        }

        [TestMethod]
        public void CambiaPassword_CuentaSinCredencialesYClaveVacia_NoConfiguraLaContraseña()
        {
            Usuario usuario = CrearUsuario(credencialesConfiguradas: false);

            Assert.IsFalse(usuario.CambiaPassword(""));
            Assert.IsFalse(usuario.CambiaPassword("   "));
            Assert.IsFalse(usuario.TieneCredencialesConfiguradas);
            Assert.AreEqual(0, usuario.ListaPasswordsCount);
        }

        [TestMethod]
        public void CambiaPassword_CuentaConCredenciales_NoPermiteLaSobrecargaDeConfiguracionInicial()
        {
            Usuario usuario = CrearUsuario();

            bool resultado = usuario.CambiaPassword("ClaveNueva456!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveInicial123!", usuario.Password));
        }

        [TestMethod]
        public void CambiaPassword_ContraseñaAnteriorCorrecta_CambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();
            DateTime antes = DateTime.Now;

            bool resultado = usuario.CambiaPassword(
                "ClaveInicial123!",
                "ClaveNueva456!");

            DateTime despues = DateTime.Now;

            Assert.IsTrue(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveNueva456!", usuario.Password));
            Assert.IsFalse(ServicioHash.Verificar("ClaveInicial123!", usuario.Password));
            Assert.IsTrue(usuario.UltimoCambioPassword >= antes);
            Assert.IsTrue(usuario.UltimoCambioPassword <= despues);
            Assert.AreEqual(1, usuario.ListaPasswordsCount);
        }

        [TestMethod]
        public void CambiaPassword_NuevaIgualALaAnterior_NoCambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();

            bool resultado = usuario.CambiaPassword(
                "ClaveInicial123!",
                "ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveInicial123!", usuario.Password));
        }

        [TestMethod]
        public void CambiaPassword_NuevaVacia_NoCambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsFalse(usuario.CambiaPassword("ClaveInicial123!", ""));
            Assert.IsFalse(usuario.CambiaPassword("ClaveInicial123!", "   "));
            Assert.IsTrue(ServicioHash.Verificar("ClaveInicial123!", usuario.Password));
        }

        [TestMethod]
        public void CambiaPassword_ContraseñaAnteriorIncorrecta_NoCambiaLaContraseña()
        {
            Usuario usuario = CrearUsuario();

            bool resultado = usuario.CambiaPassword(
                "ClaveIncorrecta123!",
                "ClaveNueva456!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveInicial123!", usuario.Password));
            Assert.AreEqual(1, usuario.IntentosErroneos);
        }

        [TestMethod]
        public void CambiaPassword_ReutilizaLaContraseñaAnterior_NoLaCambia()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsTrue(usuario.CambiaPassword(
                "ClaveInicial123!",
                "ClaveNueva456!"));

            bool resultado = usuario.CambiaPassword(
                "ClaveNueva456!",
                "ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveNueva456!", usuario.Password));
        }

        [TestMethod]
        public void CambiaPassword_ReutilizaUnaContraseñaHistorica_NoLaCambia()
        {
            Usuario usuario = CrearUsuario();

            Assert.IsTrue(usuario.CambiaPassword(
                "ClaveInicial123!",
                "ClaveNueva456!"));
            Assert.IsTrue(usuario.CambiaPassword(
                "ClaveNueva456!",
                "ClaveOtra789!"));

            bool resultado = usuario.CambiaPassword(
                "ClaveOtra789!",
                "ClaveInicial123!");

            Assert.IsFalse(resultado);
            Assert.IsTrue(ServicioHash.Verificar("ClaveOtra789!", usuario.Password));
        }

        [TestMethod]
        public void ToString_IncluyeIdNombreDeUsuarioYCorreo()
        {
            Usuario usuario = CrearUsuario();
            usuario.IdUsuario = 42;

            string resultado = usuario.ToString();

            Assert.AreEqual(
                "Usuario { IdUsuario: 42, Nombre_usuario: usuario_test, Mail: usuario@test.com }",
                resultado);
        }

        private static Usuario CrearUsuario(
            string password = "ClaveInicial123!",
            bool credencialesConfiguradas = true)
        {
            return new Usuario
            {
                Nombre_usuario = "usuario_test",
                Nombre = "Usuario",
                Apellidos = "De Prueba",
                Mail = "usuario@test.com",
                Password = credencialesConfiguradas
                    ? ServicioHash.Generar(password)
                    : new ResultadoHash(),
                TieneCredencialesConfiguradas = credencialesConfiguradas
            };
        }
    }
}