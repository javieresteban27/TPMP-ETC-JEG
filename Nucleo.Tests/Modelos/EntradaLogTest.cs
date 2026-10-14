using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;

namespace NucleoTest.Modelos
{
    [TestClass]
    public class EntradaLogSeleccionadaTests
    {
        [TestMethod]
        public void ConstructorCompleto_ConservaLosValoresRecibidos()
        {
            var fecha = new DateTime(2026, 9, 30, 12, 30, 0);
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_prueba",
                Mail = "usuario@example.com"
            };

            var entrada = new EntradaLog(fecha, "WARN", "Mensaje de prueba", usuario);

            Assert.AreEqual(fecha, entrada.FechaHora);
            Assert.AreEqual("WARN", entrada.Nivel);
            Assert.AreEqual("Mensaje de prueba", entrada.Mensaje);
            Assert.AreSame(usuario, entrada.Usuario);
        }

        [TestMethod]
        public void ConstructorConMensaje_UsaValoresPredeterminados()
        {
            var inicio = DateTime.Now;

            var entrada = new EntradaLog("Mensaje de prueba");

            var fin = DateTime.Now;
            Assert.IsTrue(entrada.FechaHora >= inicio && entrada.FechaHora <= fin);
            Assert.AreEqual("INFO", entrada.Nivel);
            Assert.AreEqual("Mensaje de prueba", entrada.Mensaje);
            Assert.IsNull(entrada.Usuario);
        }

        [TestMethod]
        public void ToString_SinUsuario_IndicaSistema()
        {
            var fecha = new DateTime(2026, 9, 30, 12, 30, 0);
            var entrada = new EntradaLog(fecha, "INFO", "Inicio", null);

            var resultado = entrada.ToString();

            Assert.AreEqual($"{fecha:dd/MM/yyyy HH:mm:ss} [INFO] Inicio - Sistema", resultado);
        }

        [TestMethod]
        public void ToString_ConUsuario_IncluyeSuRepresentacion()
        {
            var fecha = new DateTime(2026, 9, 30, 12, 30, 0);
            var usuario = new Usuario
            {
                Nombre_usuario = "usuario_prueba",
                Mail = "usuario@example.com"
            };
            var entrada = new EntradaLog(fecha, "ERROR", "Fallo", usuario);

            var resultado = entrada.ToString();

            Assert.AreEqual($"{fecha:dd/MM/yyyy HH:mm:ss} [ERROR] Fallo - {usuario}", resultado);
        }
    }
}