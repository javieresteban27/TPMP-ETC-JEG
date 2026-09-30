using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;

namespace NucleoTest.Modelos
{
    [TestClass]
    public class EntradaLogTest
    {
        [TestMethod]
        public void EntradaNueva_InicializaValoresPredeterminados()
        {
            var antes = DateTime.UtcNow;

            var entrada = new EntradaLog
            {
                Accion = AccionLog.UsuarioCreado
            };

            var despues = DateTime.UtcNow;

            Assert.AreEqual(0, entrada.Id);
            Assert.AreEqual(AccionLog.UsuarioCreado, entrada.Accion);
            Assert.AreEqual(DateTimeKind.Utc, entrada.FechaUtc.Kind);
            Assert.IsTrue(entrada.FechaUtc >= antes && entrada.FechaUtc <= despues);
            Assert.IsNull(entrada.UsuarioId);
            Assert.IsNull(entrada.NombreUsuario);
            Assert.IsNull(entrada.ActorUsuarioId);
            Assert.IsNull(entrada.ValorAnterior);
            Assert.IsNull(entrada.ValorNuevo);
            Assert.IsNull(entrada.Detalles);
        }

        [TestMethod]
        public void EntradaNueva_ConservaLosValoresAsignados()
        {
            var fecha = new DateTime(2026, 9, 30, 12, 30, 0, DateTimeKind.Utc);

            var entrada = new EntradaLog
            {
                Id = 42,
                FechaUtc = fecha,
                Accion = AccionLog.MailAñadido,
                UsuarioId = 7,
                NombreUsuario = "usuario_prueba",
                ActorUsuarioId = 3,
                ValorAnterior = "anterior@example.com",
                ValorNuevo = "nuevo@example.com",
                Detalles = "Actualización de correo"
            };

            Assert.AreEqual(42, entrada.Id);
            Assert.AreEqual(fecha, entrada.FechaUtc);
            Assert.AreEqual(AccionLog.MailAñadido, entrada.Accion);
            Assert.AreEqual(7, entrada.UsuarioId);
            Assert.AreEqual("usuario_prueba", entrada.NombreUsuario);
            Assert.AreEqual(3, entrada.ActorUsuarioId);
            Assert.AreEqual("anterior@example.com", entrada.ValorAnterior);
            Assert.AreEqual("nuevo@example.com", entrada.ValorNuevo);
            Assert.AreEqual("Actualización de correo", entrada.Detalles);
        }
    }
}