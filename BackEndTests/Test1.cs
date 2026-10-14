using BackEnd;
using Nucleo.Modelos;

namespace Nucleo.Tests;

[TestClass]
public class DatosTests
{
    [TestMethod]
    public void Constructor_InicializaColeccionesVacias()
    {
        var datos = new Datos();

        Assert.AreEqual(0, datos.Usuarios.Count);
        Assert.AreEqual(0, datos.Roles.Count);
        Assert.AreEqual(0, datos.Proyectos.Count);
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void GuardarUsuarioNuevo_AsignaIdYPermiteBuscarPorIdYCorreo()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(-1, "ana@example.com");

        Usuario guardado = datos.GuardarUsuario(usuario)!;

        Assert.AreEqual(1, guardado.IdUsuario);
        Assert.AreSame(usuario, datos.LeerUsuario(guardado.IdUsuario));
        Assert.AreSame(usuario, datos.LeerUsuario("ANA@EXAMPLE.COM"));
        Assert.AreEqual("Usuario añadido: ana@example.com", datos.Logs.Single().Mensaje);
    }

    [TestMethod]
    public void GuardarUsuarioExistente_ActualizaRegistro()
    {
        var datos = new Datos();
        var original = CrearUsuario(-1, "ana@example.com");
        datos.GuardarUsuario(original);

        var actualizado = CrearUsuario(original.IdUsuario, "nueva@example.com");
        datos.GuardarUsuario(actualizado);

        Assert.AreSame(actualizado, datos.LeerUsuario(original.IdUsuario));
        Assert.AreEqual("Usuario actualizado: nueva@example.com", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void EliminarUsuario_DevuelveYEliminaElUsuario()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(-1, "ana@example.com");
        datos.GuardarUsuario(usuario);

        Usuario? eliminado = datos.EliminarUsuario(usuario.IdUsuario);

        Assert.AreSame(usuario, eliminado);
        Assert.IsNull(datos.LeerUsuario(usuario.IdUsuario));
        Assert.AreEqual("Usuario eliminado: ana@example.com", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void LeerUsuario_CorreoNoExistenteDevuelveNull()
    {
        var datos = new Datos();

        Assert.IsNull(datos.LeerUsuario("inexistente@example.com"));
    }

    [TestMethod]
    public void GuardarRolNuevo_AsignaIdYPermiteBuscarlo()
    {
        var datos = new Datos();
        var rol = new Rol { Nombre = "Administrador" };

        Rol guardado = datos.GuardarRol(rol)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(rol, datos.LeerRol(guardado.Id));
        Assert.AreEqual("Rol añadido: Administrador", datos.Logs.Single().Mensaje);
    }

    [TestMethod]
    public void GuardarRolExistente_ActualizaRegistro()
    {
        var datos = new Datos();
        var original = new Rol { Nombre = "Lector" };
        datos.GuardarRol(original);

        var actualizado = new Rol { Id = original.Id, Nombre = "Editor" };
        datos.GuardarRol(actualizado);

        Assert.AreSame(actualizado, datos.LeerRol(original.Id));
        Assert.AreEqual("Rol actualizado: Editor", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void EliminarRol_InexistenteDevuelveNull()
    {
        var datos = new Datos();

        Assert.IsNull(datos.EliminarRol(123));
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void GuardarProyectoNuevo_AsignaIdYPermiteBuscarlo()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Nombre = "Proyecto A" };

        Proyecto guardado = datos.GuardarProyecto(proyecto)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(proyecto, datos.LeerProyecto(guardado.Id));
        Assert.AreEqual("Proyecto añadido: Proyecto A", datos.Logs.Single().Mensaje);
    }

    [TestMethod]
    public void GuardarProyectoConIdExistente_ActualizaRegistro()
    {
        var datos = new Datos();
        var original = new Proyecto { Nombre = "Proyecto A" };
        datos.GuardarProyecto(original);

        var actualizado = new Proyecto { Id = original.Id, Nombre = "Proyecto actualizado" };
        datos.GuardarProyecto(actualizado);

        Assert.AreSame(actualizado, datos.LeerProyecto(original.Id));
        Assert.AreEqual("Proyecto actualizado: Proyecto actualizado", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void LimpiarLog_DevuelveCantidadEliminadaYVaciaLaLista()
    {
        var datos = new Datos();
        datos.AgregarLog(new EntradaLog("Primero"));
        datos.AgregarLog(new EntradaLog("Segundo"));

        int cantidadEliminada = datos.LimpiarLog();

        Assert.AreEqual(2, cantidadEliminada);
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void Reiniciar_VaciaLosDatosYRestableceLosIds()
    {
        var datos = new Datos();
        datos.GuardarUsuario(CrearUsuario(-1, "ana@example.com"));
        datos.GuardarRol(new Rol { Nombre = "Administrador" });
        datos.GuardarProyecto(new Proyecto { Nombre = "Proyecto A" });

        datos.Reiniciar();

        Assert.AreEqual(0, datos.Usuarios.Count);
        Assert.AreEqual(0, datos.Roles.Count);
        Assert.AreEqual(0, datos.Proyectos.Count);
        Assert.AreEqual(0, datos.Logs.Count);

        Assert.AreEqual(1, datos.GuardarUsuario(CrearUsuario(-1, "reinicio@example.com"))!.IdUsuario);
        Assert.AreEqual(1, datos.GuardarRol(new Rol { Nombre = "Nuevo rol" })!.Id);
        Assert.AreEqual(1, datos.GuardarProyecto(new Proyecto { Nombre = "Nuevo proyecto" })!.Id);
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