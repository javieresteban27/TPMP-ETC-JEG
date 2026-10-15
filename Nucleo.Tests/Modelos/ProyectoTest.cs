using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;

namespace Nucleo.Tests.Modelos
{
    [TestClass]
    public class ProyectoTests
    {
        [TestMethod]
        public void Constructor_InicializaValoresPredeterminados()
        {
            var antes = DateTime.UtcNow;
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };
            var despues = DateTime.UtcNow;

            Assert.AreEqual(0, proyecto.IdProyecto);
            Assert.AreEqual(0, proyecto.Id);
            Assert.AreEqual("Proyecto de prueba", proyecto.Nombre);
            Assert.IsTrue(proyecto.Activo);
            Assert.AreEqual(EstadoProyecto.Planificado, proyecto.Estado);
            Assert.IsNull(proyecto.FechaFinUtc);
            Assert.IsTrue(proyecto.FechaCreacionUtc >= antes);
            Assert.IsTrue(proyecto.FechaCreacionUtc <= despues);
            Assert.IsTrue(proyecto.FechaInicioUtc >= antes);
            Assert.IsTrue(proyecto.FechaInicioUtc <= despues);
        }

        [TestMethod]
        public void IdYIdProyecto_CompartenElMismoValor()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };

            proyecto.IdProyecto = 42;
            Assert.AreEqual(42, proyecto.Id);

            proyecto.Id = 17;
            Assert.AreEqual(17, proyecto.IdProyecto);
        }

        [TestMethod]
        public void FechaCreacionYFechaCreacionUtc_CompartenElMismoValor()
        {
            var fecha = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            var proyecto = new Proyecto
            {
                Nombre = "Proyecto de prueba",
                FechaCreacion = fecha
            };

            Assert.AreEqual(fecha, proyecto.FechaCreacionUtc);

            var otraFecha = fecha.AddDays(1);
            proyecto = new Proyecto
            {
                Nombre = "Proyecto de prueba",
                FechaCreacionUtc = otraFecha
            };

            Assert.AreEqual(otraFecha, proyecto.FechaCreacion);
        }

        [TestMethod]
        public void Iniciar_ActivaElProyectoYLimpiaLaFechaFin()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };
            proyecto.Finalizar();

            proyecto.Iniciar();

            Assert.IsTrue(proyecto.Activo);
            Assert.AreEqual(EstadoProyecto.EnCurso, proyecto.Estado);
            Assert.IsNull(proyecto.FechaFinUtc);
        }

        [TestMethod]
        public void Finalizar_MarcaElProyectoComoFinalizado()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };
            var antes = DateTime.UtcNow;

            proyecto.Finalizar();

            var despues = DateTime.UtcNow;
            Assert.IsFalse(proyecto.Activo);
            Assert.AreEqual(EstadoProyecto.Finalizado, proyecto.Estado);
            Assert.IsNotNull(proyecto.FechaFinUtc);
            Assert.IsTrue(proyecto.FechaFinUtc.Value >= antes);
            Assert.IsTrue(proyecto.FechaFinUtc.Value <= despues);
        }

        [TestMethod]
        public void Cancelar_MarcaElProyectoComoCancelado()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };
            var antes = DateTime.UtcNow;

            proyecto.Cancelar();

            var despues = DateTime.UtcNow;
            Assert.IsFalse(proyecto.Activo);
            Assert.AreEqual(EstadoProyecto.Cancelado, proyecto.Estado);
            Assert.IsNotNull(proyecto.FechaFinUtc);
            Assert.IsTrue(proyecto.FechaFinUtc.Value >= antes);
            Assert.IsTrue(proyecto.FechaFinUtc.Value <= despues);
        }

        [TestMethod]
        public void Reactivar_ActivaElProyectoYEliminaLaFechaFin()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };
            proyecto.Cancelar();

            proyecto.Reactivar();

            Assert.IsTrue(proyecto.Activo);
            Assert.AreEqual(EstadoProyecto.EnCurso, proyecto.Estado);
            Assert.IsNull(proyecto.FechaFinUtc);
        }

        [TestMethod]
        public void EstaFinalizado_DevuelveTrueSoloCuandoElEstadoEsFinalizado()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };

            Assert.IsFalse(proyecto.EstaFinalizado());

            proyecto.Cancelar();
            Assert.IsFalse(proyecto.EstaFinalizado());

            proyecto.Finalizar();
            Assert.IsTrue(proyecto.EstaFinalizado());
        }

        [TestMethod]
        public void ToString_DevuelveElNombre()
        {
            var proyecto = new Proyecto { Nombre = "Proyecto de prueba" };

            Assert.AreEqual("Proyecto de prueba", proyecto.ToString());
        }
    }
}