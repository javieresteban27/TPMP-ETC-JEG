using BackEnd;
using Nucleo.Modelos;

namespace Nucleo.Tests;

[TestClass]
public class DatosTests
{
    [TestMethod]
    public void Constructor_CreaUsuarioDePruebaYColeccionesVacias()
    {
        var datos = new Datos();

        Assert.AreEqual(1, datos.Usuarios.Count);
        Assert.AreEqual(0, datos.Roles.Count);
        Assert.AreEqual(0, datos.Proyectos.Count);
        Assert.AreEqual(0, datos.Logs.Count);

        Usuario usuario = datos.LeerUsuario(1)!;
        Assert.IsNotNull(usuario);
        Assert.AreEqual("usuario@test.com", usuario.Mail);
        Assert.AreSame(usuario, datos.LeerUsuario("USUARIO@TEST.COM"));
    }

    [TestMethod]
    public void GuardarUsuario_SinIdAsignaIdsDisponiblesYRegistraLaOperacion()
    {
        var datos = new Datos();
        var primero = CrearUsuario(-1, "ana@example.com");
        var segundo = CrearUsuario(-1, "bea@example.com");

        Usuario? guardadoPrimero = datos.GuardarUsuario(primero);
        Usuario? guardadoSegundo = datos.GuardarUsuario(segundo);

        Assert.AreSame(primero, guardadoPrimero);
        Assert.AreEqual(2, primero.IdUsuario);
        Assert.AreEqual(3, segundo.IdUsuario);
        Assert.AreSame(primero, datos.LeerUsuario(2));
        Assert.AreSame(segundo, datos.LeerUsuario("BEA@example.com"));
        Assert.AreEqual("Usuario añadido: ana@example.com", datos.Logs[0].Mensaje);
        Assert.AreEqual("Usuario añadido: bea@example.com", datos.Logs[1].Mensaje);
    }

    [TestMethod]
    public void GuardarUsuario_ActualizaYAvanzaElSiguienteId()
    {
        var datos = new Datos();
        var usuarioExistente = CrearUsuario(10, "ana@example.com");

        datos.GuardarUsuario(usuarioExistente);

        var actualizado = CrearUsuario(10, "nueva@example.com");
        datos.GuardarUsuario(actualizado);

        var nuevo = CrearUsuario(-1, "bea@example.com");
        datos.GuardarUsuario(nuevo);

        Assert.AreSame(actualizado, datos.LeerUsuario(10));
        Assert.AreEqual(11, nuevo.IdUsuario);
        Assert.AreEqual("Usuario actualizado: ana@example.com", datos.Logs[0].Mensaje);
        Assert.AreEqual("Usuario actualizado: nueva@example.com", datos.Logs[1].Mensaje);
        Assert.AreEqual("Usuario añadido: bea@example.com", datos.Logs[2].Mensaje);
    }

    [TestMethod]
    public void EliminarUsuario_EliminaYRegistraLaOperacion()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(10, "ana@example.com");
        datos.GuardarUsuario(usuario);

        Usuario? eliminado = datos.EliminarUsuario(10);

        Assert.AreSame(usuario, eliminado);
        Assert.IsNull(datos.LeerUsuario(10));
        Assert.IsNull(datos.LeerUsuario("ana@example.com"));
        Assert.AreEqual("Usuario eliminado: ana@example.com", datos.Logs[1].Mensaje);
        Assert.IsNull(datos.EliminarUsuario(10));
    }

    [TestMethod]
    public void Rol_PermiteGuardarBuscarActualizarYEliminar()
    {
        var datos = new Datos();
        var rol = new Rol { Id = 10, Nombre = "Lector" };

        datos.GuardarRol(rol);

        Assert.AreSame(rol, datos.LeerRol(10));
        Assert.AreEqual("Rol actualizado: Lector", datos.Logs[0].Mensaje);

        var actualizado = new Rol { Id = 10, Nombre = "Editor" };
        datos.GuardarRol(actualizado);

        Assert.AreSame(actualizado, datos.LeerRol(10));
        Assert.AreEqual("Rol actualizado: Editor", datos.Logs[1].Mensaje);

        var siguiente = new Rol { Nombre = "Administrador" };
        datos.GuardarRol(siguiente);

        Assert.AreEqual(11, siguiente.Id);
        Assert.AreSame(siguiente, datos.LeerRol(11));
        Assert.AreEqual("Rol añadido: Administrador", datos.Logs[2].Mensaje);

        Rol? eliminado = datos.EliminarRol(10);

        Assert.AreSame(actualizado, eliminado);
        Assert.IsNull(datos.LeerRol(10));
        Assert.AreEqual("Rol eliminado: Editor", datos.Logs[3].Mensaje);
        Assert.IsNull(datos.EliminarRol(10));
        Assert.IsNull(datos.LeerRol(999));
    }

    [TestMethod]
    public void Proyecto_PermiteGuardarBuscarActualizarYEliminar()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Id = 10, Nombre = "Proyecto A" };

        datos.GuardarProyecto(proyecto);

        Assert.AreSame(proyecto, datos.LeerProyecto(10));
        Assert.AreEqual("Proyecto actualizado: Proyecto A", datos.Logs[0].Mensaje);

        var actualizado = new Proyecto
        {
            Id = 10,
            Nombre = "Proyecto actualizado"
        };
        datos.GuardarProyecto(actualizado);

        Assert.AreSame(actualizado, datos.LeerProyecto(10));
        Assert.AreEqual(
            "Proyecto actualizado: Proyecto actualizado",
            datos.Logs[1].Mensaje);

        var siguiente = new Proyecto { Nombre = "Proyecto B" };
        datos.GuardarProyecto(siguiente);

        Assert.AreEqual(11, siguiente.Id);
        Assert.AreSame(siguiente, datos.LeerProyecto(11));
        Assert.AreEqual("Proyecto añadido: Proyecto B", datos.Logs[2].Mensaje);

        Proyecto? eliminado = datos.EliminarProyecto(10);

        Assert.AreSame(actualizado, eliminado);
        Assert.IsNull(datos.LeerProyecto(10));
        Assert.AreEqual("Proyecto eliminado: Proyecto actualizado", datos.Logs[3].Mensaje);
        Assert.IsNull(datos.EliminarProyecto(10));
        Assert.IsNull(datos.LeerProyecto(999));
    }

    [TestMethod]
    public void AgregarLogYLimpiarLog_DevuelveLaCantidadEliminada()
    {
        var datos = new Datos();
        var primero = new EntradaLog("Primero");
        var segundo = new EntradaLog("Segundo");

        datos.AgregarLog(primero);
        datos.AgregarLog(segundo);

        Assert.AreSame(primero, datos.Logs[0]);
        Assert.AreSame(segundo, datos.Logs[1]);
        Assert.AreEqual(2, datos.LimpiarLog());
        Assert.AreEqual(0, datos.Logs.Count);
        Assert.AreEqual(0, datos.LimpiarLog());
    }

    [TestMethod]
    public void Reiniciar_VaciaLosDatosYRestableceLosIds()
    {
        var datos = new Datos();

        datos.GuardarUsuario(CrearUsuario(5, "ana@example.com"));
        datos.GuardarRol(new Rol { Id = 5, Nombre = "Administrador" });
        datos.GuardarProyecto(new Proyecto { Id = 5, Nombre = "Proyecto A" });
        datos.AgregarLog(new EntradaLog("Log adicional"));

        datos.Reiniciar();

        Assert.AreEqual(0, datos.Usuarios.Count);
        Assert.AreEqual(0, datos.Roles.Count);
        Assert.AreEqual(0, datos.Proyectos.Count);
        Assert.AreEqual(0, datos.Logs.Count);

        Assert.AreEqual(
            1,
            datos.GuardarUsuario(CrearUsuario(-1, "reinicio@example.com"))!.IdUsuario);
        Assert.AreEqual(
            1,
            datos.GuardarRol(new Rol { Nombre = "Nuevo rol" })!.Id);
        Assert.AreEqual(
            1,
            datos.GuardarProyecto(new Proyecto { Nombre = "Nuevo proyecto" })!.Id);
    }

    private static Usuario CrearUsuario(int id, string correo)
    {
        return new Usuario
        {
            IdUsuario = id,
            Nombre_usuario = "Usuario de prueba",
            Nombre = "Usuario",
            Apellidos = "de prueba",
            Mail = correo
        };
    }
}