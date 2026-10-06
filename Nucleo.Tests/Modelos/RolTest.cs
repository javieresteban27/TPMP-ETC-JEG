using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Modelos;

namespace Nucleo.Modelos.Tests
{
    [TestClass]
    public class RolTests
    {
        [TestMethod]
        public void NuevoRol_InicializaValoresCorrectamente()
        {
            DateTime antes = DateTime.UtcNow;

            var rol = new Rol
            {
                Id = 1,
                Nombre = "Administrador",
                Descripcion = "Administrador del sistema"
            };

            DateTime despues = DateTime.UtcNow;

            Assert.AreEqual(1, rol.Id);
            Assert.AreEqual("Administrador", rol.Nombre);
            Assert.AreEqual(
                "Administrador del sistema",
                rol.Descripcion);

            Assert.IsTrue(rol.Activo);

            Assert.AreEqual(
                DateTimeKind.Utc,
                rol.FechaCreacionUtc.Kind);

            Assert.IsTrue(
                rol.FechaCreacionUtc >= antes &&
                rol.FechaCreacionUtc <= despues);
        }

        [TestMethod]
        public void Desactivar_SetsActivoFalse()
        {
            var rol = new Rol
            {
                Nombre = "Usuario"
            };

            rol.Desactivar();

            Assert.IsFalse(rol.Activo);
        }

        [TestMethod]
        public void Activar_SetsActivoTrue_WhenInitiallyFalse()
        {
            var rol = new Rol
            {
                Nombre = "Usuario"
            };

            rol.Desactivar();

            Assert.IsFalse(rol.Activo);

            rol.Activar();

            Assert.IsTrue(rol.Activo);
        }

        [TestMethod]
        public void Activar_LeavesActivoTrue_WhenAlreadyTrue()
        {
            var rol = new Rol
            {
                Nombre = "Usuario"
            };

            rol.Activar();

            Assert.IsTrue(rol.Activo);
        }

        [TestMethod]
        public void ToString_DevuelveNombre()
        {
            var rol = new Rol
            {
                Nombre = "Administrador"
            };

            string resultado = rol.ToString();

            Assert.AreEqual("Administrador", resultado);
        }

        [TestMethod]
        public void Descripcion_PuedeSerNull()
        {
            var rol = new Rol
            {
                Nombre = "Usuario"
            };

            Assert.IsNull(rol.Descripcion);
        }
    }
}