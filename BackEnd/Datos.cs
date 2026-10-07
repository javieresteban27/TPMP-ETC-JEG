using BackEnd;
using Nucleo.Modelos;

namespace BackEnd;

public class Datos : IDatos
{
    public Dictionary<int, Usuario> Usuarios => throw new NotImplementedException();

    public Dictionary<int, Rol> Roles => throw new NotImplementedException();

    public Dictionary<int, Proyecto> Proyectos => throw new NotImplementedException();

    public List<EntradaLog> Logs => throw new NotImplementedException();

    private int SiguienteIdUsuario = 1;
    private int SiguienteIdRol = 1;
    private int SiguienteIdProyecto = 1;

    #region USUARIOS
    public Usuario? GuardarUsuario(Usuario usuario)
    {
        if (usuario.IdUsuario < 0)
        {
            usuario.IdUsuario = SiguienteIdUsuario++;
            Usuarios.Add(usuario.IdUsuario, usuario);
            AgregarLog(new EntradaLog($"Usuario añadido: {usuario.Mail}"));
        }
        else
        {
            Usuarios[usuario.IdUsuario] = usuario;
            AgregarLog(new EntradaLog($"Usuario actualizado: {usuario.Mail}"));
        }

        return usuario;

    }

    public Usuario? EliminarUsuario(int idUsuario)
    {
        if (!Usuarios.Remove(idUsuario, out Usuario? usuario))
        {
            return null;
        }

        AgregarLog(new EntradaLog($"Usuario eliminado: {usuario.Mail}"));
        return usuario;
    }

    public Usuario? LeerUsuario(int idUsuario)
    {
        return Usuarios.TryGetValue(idUsuario, out Usuario? usuario)
            ? usuario
            : null;
    }

    public Usuario? LeerUsuario(string eMail)
    {
        foreach (Usuario usuario in Usuarios.Values)
        {
            if (string.Equals(usuario.Mail, eMail, StringComparison.OrdinalIgnoreCase))
            {
                return usuario;
            }
        }

        return null;
    }

    #endregion

    #region LOGS
    public void AgregarLog(EntradaLog log)
    {
        Logs.Add(log);
    }

    public int LimpiarLog()
    {
        int cantidadEliminada = Logs.Count;
        Logs.Clear();
        return cantidadEliminada;
    }

    #endregion

    #region PROYECTOS
    public Proyecto? EliminarProyecto(int idProyecto)
    {
        if (!Proyectos.Remove(idProyecto, out Proyecto? proyecto))
        {
            return null;
        }

        AgregarLog(new EntradaLog($"Proyecto eliminado: {proyecto.Nombre}"));
        return proyecto;
    }

    public Proyecto? GuardarProyecto(Proyecto proyecto)
    {
        if (proyecto.Id <= 0)
        {
            while (Proyectos.ContainsKey(SiguienteIdProyecto))
            {
                SiguienteIdProyecto++;
            }

            proyecto.Id = SiguienteIdProyecto++;
            Proyectos.Add(proyecto.Id, proyecto);
            AgregarLog(new EntradaLog($"Proyecto añadido: {proyecto.Nombre}"));
        }
        else
        {
            Proyectos[proyecto.Id] = proyecto;
            SiguienteIdProyecto = Math.Max(SiguienteIdProyecto, proyecto.Id + 1);
            AgregarLog(new EntradaLog($"Proyecto actualizado: {proyecto.Nombre}"));
        }

        return proyecto;
    }

    public Proyecto? LeerProyecto(int idProyecto)
    {
        return Proyectos.TryGetValue(idProyecto, out Proyecto? proyecto)
            ? proyecto
            : null;
    }

    #endregion

    #region ROLES
    public Rol? EliminarRol(int idRol)
    {
        if (!Roles.Remove(idRol, out Rol? rol))
        {
            return null;
        }

        AgregarLog(new EntradaLog($"Rol eliminado: {rol.Nombre}"));
        return rol;
    }

    public Rol? GuardarRol(Rol rol)
    {
        if (rol.Id <= 0)
        {
            while (Roles.ContainsKey(SiguienteIdRol))
            {
                SiguienteIdRol++;
            }

            rol.Id = SiguienteIdRol++;
            Roles.Add(rol.Id, rol);
            AgregarLog(new EntradaLog($"Rol añadido: {rol.Nombre}"));
        }
        else
        {
            Roles[rol.Id] = rol;
            SiguienteIdRol = Math.Max(SiguienteIdRol, rol.Id + 1);
            AgregarLog(new EntradaLog($"Rol actualizado: {rol.Nombre}"));
        }

        return rol;
    }

    public Rol? LeerRol(int idRol)
    {
        return Roles.TryGetValue(idRol, out Rol? rol)
               ? rol
               : null;
    }

    #endregion

    public void Reiniciar()
    {
        throw new NotImplementedException();
    }
}
