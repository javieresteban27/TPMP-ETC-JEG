using BackEnd;
using Nucleo.Modelos;

namespace Nucleo.Tests;

[TestClass]
public class DatosTests
{
    [TestMethod]
    public void Constructor_InicializaLasColeccionesVacias()
    {
        var datos = new Datos();

        Assert.AreEqual(0, datos.Usuarios.Count);
        Assert.AreEqual(0, datos.Roles.Count);
        Assert.AreEqual(0, datos.Proyectos.Count);
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void Usuario_PermiteGuardarBuscarActualizarYEliminar()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(-1, "ana@example.com");

        Usuario guardado = datos.GuardarUsuario(usuario)!;

        Assert.AreEqual(1, guardado.IdUsuario);
        Assert.AreSame(usuario, datos.LeerUsuario(guardado.IdUsuario));
        Assert.AreSame(usuario, datos.LeerUsuario("ANA@EXAMPLE.COM"));
        Assert.AreEqual("Usuario añadido: ana@example.com", datos.Logs[0].Mensaje);

        var actualizado = CrearUsuario(guardado.IdUsuario, "nueva@example.com");
        datos.GuardarUsuario(actualizado);

        Assert.AreSame(actualizado, datos.LeerUsuario(guardado.IdUsuario));
        Assert.AreEqual("Usuario actualizado: nueva@example.com", datos.Logs[1].Mensaje);

        Usuario? eliminado = datos.EliminarUsuario(guardado.IdUsuario);

        Assert.AreSame(actualizado, eliminado);
        Assert.IsNull(datos.LeerUsuario(guardado.IdUsuario));
        Assert.IsNull(datos.LeerUsuario("nueva@example.com"));
        Assert.AreEqual("Usuario eliminado: nueva@example.com", datos.Logs[2].Mensaje);
        Assert.IsNull(datos.EliminarUsuario(guardado.IdUsuario));
    }

    [TestMethod]
    public void Rol_PermiteGuardarBuscarActualizarYEliminar()
    {
        var datos = new Datos();
        var rol = new Rol { Nombre = "Lector" };

        Rol guardado = datos.GuardarRol(rol)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(rol, datos.LeerRol(guardado.Id));
        Assert.AreEqual("Rol añadido: Lector", datos.Logs[0].Mensaje);

        var actualizado = new Rol { Id = guardado.Id, Nombre = "Editor" };
        datos.GuardarRol(actualizado);

        Assert.AreSame(actualizado, datos.LeerRol(guardado.Id));
        Assert.AreEqual("Rol actualizado: Editor", datos.Logs[1].Mensaje);

        Rol? eliminado = datos.EliminarRol(guardado.Id);

        Assert.AreSame(actualizado, eliminado);
        Assert.IsNull(datos.LeerRol(guardado.Id));
        Assert.AreEqual("Rol eliminado: Editor", datos.Logs[2].Mensaje);
        Assert.IsNull(datos.EliminarRol(guardado.Id));
    }

    [TestMethod]
    public void Proyecto_PermiteGuardarBuscarActualizarYEliminar()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Nombre = "Proyecto A" };

        Proyecto guardado = datos.GuardarProyecto(proyecto)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(proyecto, datos.LeerProyecto(guardado.Id));
        Assert.AreEqual("Proyecto añadido: Proyecto A", datos.Logs[0].Mensaje);

        var actualizado = new Proyecto
        {
            Id = guardado.Id,
            Nombre = "Proyecto actualizado"
        };
        datos.GuardarProyecto(actualizado);

        Assert.AreSame(actualizado, datos.LeerProyecto(guardado.Id));
        Assert.AreEqual(
            "Proyecto actualizado: Proyecto actualizado",
            datos.Logs[1].Mensaje);

        Proyecto? eliminado = datos.EliminarProyecto(guardado.Id);

        Assert.AreSame(actualizado, eliminado);
        Assert.IsNull(datos.LeerProyecto(guardado.Id));
        Assert.AreEqual(
            "Proyecto eliminado: Proyecto actualizado",
            datos.Logs[2].Mensaje);
        Assert.IsNull(datos.EliminarProyecto(guardado.Id));
    }

    [TestMethod]
    public void LimpiarLog_DevuelveLaCantidadEliminadaYVaciaLaLista()
    {
        var datos = new Datos();
        datos.AgregarLog(new EntradaLog("Primero"));
        datos.AgregarLog(new EntradaLog("Segundo"));

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
            Mail = correo
        };
    }
}