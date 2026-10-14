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
        Usuario? resultado = datos.GuardarUsuario(actualizado);

        Assert.AreSame(actualizado, resultado);
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
    public void EliminarUsuario_InexistenteDevuelveNullYSinAgregarLog()
    {
        var datos = new Datos();

        Assert.IsNull(datos.EliminarUsuario(123));
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void LeerUsuario_IdExistenteDevuelveElUsuario()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(-1, "ana@example.com");
        datos.GuardarUsuario(usuario);

        Assert.AreSame(usuario, datos.LeerUsuario(usuario.IdUsuario));
    }

    [TestMethod]
    public void LeerUsuario_IdNoExistenteDevuelveNull()
    {
        var datos = new Datos();

        Assert.IsNull(datos.LeerUsuario(123));
    }

    [TestMethod]
    public void LeerUsuario_CorreoIgnoraMayusculasYMinusculas()
    {
        var datos = new Datos();
        var usuario = CrearUsuario(-1, "ana@example.com");
        datos.GuardarUsuario(usuario);

        Assert.AreSame(usuario, datos.LeerUsuario("ANA@EXAMPLE.COM"));
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
    public void GuardarRolConIdNegativo_AsignaNuevoId()
    {
        var datos = new Datos();
        var rol = new Rol { Id = -1, Nombre = "Lector" };

        Rol guardado = datos.GuardarRol(rol)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(rol, datos.LeerRol(guardado.Id));
        Assert.AreEqual("Rol añadido: Lector", datos.Logs.Single().Mensaje);
    }

    [TestMethod]
    public void GuardarRolExistente_ActualizaRegistro()
    {
        var datos = new Datos();
        var original = new Rol { Nombre = "Lector" };
        datos.GuardarRol(original);

        var actualizado = new Rol { Id = original.Id, Nombre = "Editor" };
        Rol? resultado = datos.GuardarRol(actualizado);

        Assert.AreSame(actualizado, resultado);
        Assert.AreSame(actualizado, datos.LeerRol(original.Id));
        Assert.AreEqual("Rol actualizado: Editor", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void GuardarRolConIdPersonalizado_AjustaElSiguienteId()
    {
        var datos = new Datos();
        datos.GuardarRol(new Rol { Id = 5, Nombre = "Personalizado" });

        Rol siguiente = datos.GuardarRol(new Rol { Nombre = "Siguiente" })!;

        Assert.AreEqual(6, siguiente.Id);
    }

    [TestMethod]
    public void GuardarRol_SaltaIdsYaOcupados()
    {
        var datos = new Datos();
        datos.Roles.Add(1, new Rol { Id = 1, Nombre = "Existente" });

        Rol guardado = datos.GuardarRol(new Rol { Nombre = "Nuevo" })!;

        Assert.AreEqual(2, guardado.Id);
        Assert.AreSame(guardado, datos.LeerRol(2));
    }

    [TestMethod]
    public void EliminarRol_DevuelveYEliminaElRol()
    {
        var datos = new Datos();
        var rol = new Rol { Nombre = "Administrador" };
        datos.GuardarRol(rol);

        Rol? eliminado = datos.EliminarRol(rol.Id);

        Assert.AreSame(rol, eliminado);
        Assert.IsNull(datos.LeerRol(rol.Id));
        Assert.AreEqual("Rol eliminado: Administrador", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void EliminarRol_InexistenteDevuelveNullYSinAgregarLog()
    {
        var datos = new Datos();

        Assert.IsNull(datos.EliminarRol(123));
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void LeerRol_IdExistenteDevuelveElRol()
    {
        var datos = new Datos();
        var rol = new Rol { Nombre = "Administrador" };
        datos.GuardarRol(rol);

        Assert.AreSame(rol, datos.LeerRol(rol.Id));
    }

    [TestMethod]
    public void LeerRol_IdNoExistenteDevuelveNull()
    {
        var datos = new Datos();

        Assert.IsNull(datos.LeerRol(123));
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
    public void GuardarProyectoConIdNegativo_AsignaNuevoId()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Id = -1, Nombre = "Proyecto A" };

        Proyecto guardado = datos.GuardarProyecto(proyecto)!;

        Assert.AreEqual(1, guardado.Id);
        Assert.AreSame(proyecto, datos.LeerProyecto(guardado.Id));
        Assert.AreEqual("Proyecto añadido: Proyecto A", datos.Logs.Single().Mensaje);
    }

    [TestMethod]
    public void GuardarProyectoExistente_ActualizaRegistro()
    {
        var datos = new Datos();
        var original = new Proyecto { Nombre = "Proyecto A" };
        datos.GuardarProyecto(original);

        var actualizado = new Proyecto { Id = original.Id, Nombre = "Proyecto actualizado" };
        Proyecto? resultado = datos.GuardarProyecto(actualizado);

        Assert.AreSame(actualizado, resultado);
        Assert.AreSame(actualizado, datos.LeerProyecto(original.Id));
        Assert.AreEqual("Proyecto actualizado: Proyecto actualizado", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void GuardarProyectoConIdPersonalizado_AjustaElSiguienteId()
    {
        var datos = new Datos();
        datos.GuardarProyecto(new Proyecto { Id = 5, Nombre = "Personalizado" });

        Proyecto siguiente = datos.GuardarProyecto(new Proyecto { Nombre = "Siguiente" })!;

        Assert.AreEqual(6, siguiente.Id);
    }

    [TestMethod]
    public void GuardarProyecto_SaltaIdsYaOcupados()
    {
        var datos = new Datos();
        datos.Proyectos.Add(1, new Proyecto { Id = 1, Nombre = "Existente" });

        Proyecto guardado = datos.GuardarProyecto(new Proyecto { Nombre = "Nuevo" })!;

        Assert.AreEqual(2, guardado.Id);
        Assert.AreSame(guardado, datos.LeerProyecto(2));
    }

    [TestMethod]
    public void EliminarProyecto_DevuelveYEliminaElProyecto()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Nombre = "Proyecto A" };
        datos.GuardarProyecto(proyecto);

        Proyecto? eliminado = datos.EliminarProyecto(proyecto.Id);

        Assert.AreSame(proyecto, eliminado);
        Assert.IsNull(datos.LeerProyecto(proyecto.Id));
        Assert.AreEqual("Proyecto eliminado: Proyecto A", datos.Logs.Last().Mensaje);
    }

    [TestMethod]
    public void EliminarProyecto_InexistenteDevuelveNullYSinAgregarLog()
    {
        var datos = new Datos();

        Assert.IsNull(datos.EliminarProyecto(123));
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void LeerProyecto_IdExistenteDevuelveElProyecto()
    {
        var datos = new Datos();
        var proyecto = new Proyecto { Nombre = "Proyecto A" };
        datos.GuardarProyecto(proyecto);

        Assert.AreSame(proyecto, datos.LeerProyecto(proyecto.Id));
    }

    [TestMethod]
    public void LeerProyecto_IdNoExistenteDevuelveNull()
    {
        var datos = new Datos();

        Assert.IsNull(datos.LeerProyecto(123));
    }

    [TestMethod]
    public void AgregarLog_AñadeLaEntrada()
    {
        var datos = new Datos();
        var entrada = new EntradaLog("Entrada de prueba");

        datos.AgregarLog(entrada);

        Assert.AreEqual(1, datos.Logs.Count);
        Assert.AreSame(entrada, datos.Logs.Single());
        Assert.AreEqual("Entrada de prueba", datos.Logs.Single().Mensaje);
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
    public void LimpiarLog_SinEntradasDevuelveCero()
    {
        var datos = new Datos();

        int cantidadEliminada = datos.LimpiarLog();

        Assert.AreEqual(0, cantidadEliminada);
        Assert.AreEqual(0, datos.Logs.Count);
    }

    [TestMethod]
    public void Reiniciar_VaciaLosDatosYRestableceLosIds()
    {
        var datos = new Datos();
        datos.GuardarUsuario(CrearUsuario(-1, "ana@example.com"));
        datos.GuardarRol(new Rol { Nombre = "Administrador" });
        datos.GuardarProyecto(new Proyecto { Nombre = "Proyecto A" });
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