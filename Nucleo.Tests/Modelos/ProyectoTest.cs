using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;

namespace Nucleo.Modelos.Tests
{
    [TestClass]
    public class ProyectoTests
    {
        [TestMethod]
        public void EstadoProyecto_Enum_MembersExist()
        {
            var names = Enum.GetNames(typeof(EstadoProyecto));
            Assert.IsTrue(Array.IndexOf(names, "Planificado") >= 0, "Falta Planificado");
            Assert.IsTrue(Array.IndexOf(names, "EnCurso") >= 0, "Falta EnCurso");
            Assert.IsTrue(Array.IndexOf(names, "Finalizado") >= 0, "Falta Finalizado");
            Assert.IsTrue(Array.IndexOf(names, "Cancelado") >= 0, "Falta Cancelado");
        }

        [TestMethod]
        public void NuevoProyecto_TieneEstado_Planificado()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            Assert.AreEqual(EstadoProyecto.Planificado, p.Estado);
        }

        [TestMethod]
        public void Iniciar_SeteaEstadoEnCurso_ActivoTrue_FechaFinNull()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            p.Iniciar();

            Assert.AreEqual(EstadoProyecto.EnCurso, p.Estado);
            Assert.IsTrue(p.Activo);
            Assert.IsNull(p.FechaFinUtc);
        }

        [TestMethod]
        public void Finalizar_SeteaEstadoFinalizado_ActivoFalse_FechaFinNotNull()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            p.Finalizar();

            Assert.AreEqual(EstadoProyecto.Finalizado, p.Estado);
            Assert.IsFalse(p.Activo);
            Assert.IsNotNull(p.FechaFinUtc);
        }

        [TestMethod]
        public void Cancelar_SeteaEstadoCancelado_ActivoFalse_FechaFinNotNull()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            p.Cancelar();

            Assert.AreEqual(EstadoProyecto.Cancelado, p.Estado);
            Assert.IsFalse(p.Activo);
            Assert.IsNotNull(p.FechaFinUtc);
        }

        [TestMethod]
        public void Reactivar_DesdeFinalizado_OCancelado_VuelveEnCurso_ActivoTrue_FechaFinNull()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            p.Finalizar();
            Assert.AreEqual(EstadoProyecto.Finalizado, p.Estado);

            p.Reactivar();
            Assert.AreEqual(EstadoProyecto.EnCurso, p.Estado);
            Assert.IsTrue(p.Activo);
            Assert.IsNull(p.FechaFinUtc);
        }

        [TestMethod]
        public void EstaFinalizado_DevuelveTrueSoloCuandoFinalizado()
        {
            var p = new Proyecto { Nombre = "Prueba" };
            Assert.IsFalse(p.EstaFinalizado());

            p.Finalizar();
            Assert.IsTrue(p.EstaFinalizado());

            p.Iniciar();
            Assert.IsFalse(p.EstaFinalizado());
        }
    }
}