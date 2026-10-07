using BackEnd;
using Nucleo.Modelos;

namespace BackEnd;

public class Datos : IDatos
{
    public Dictionary<int, Usuario> Usuarios => throw new NotImplementedException();

    public Dictionary<int, Rol> Roles => throw new NotImplementedException();

    public Dictionary<int, Proyecto> Proyectos => throw new NotImplementedException();

    public List<EntradaLog> Logs => throw new NotImplementedException();

    public void AgregarLog(EntradaLog log)
    {
        throw new NotImplementedException();
    }

    public Proyecto? EliminarProyecto(int idProyecto)
    {
        throw new NotImplementedException();
    }

    public Rol? EliminarRol(int idRol)
    {
        throw new NotImplementedException();
    }

    public Usuario? EliminarUsuario(int idUsuario)
    {
        throw new NotImplementedException();
    }

    public Proyecto? GuardarProyecto(Proyecto proyecto)
    {
        throw new NotImplementedException();
    }

    public Rol? GuardarRol(Rol rol)
    {
        throw new NotImplementedException();
    }

    public Usuario? GuardarUsuario(Usuario usuario)
    {
        throw new NotImplementedException();
    }

    public Proyecto? LeerProyecto(int idProyecto)
    {
        throw new NotImplementedException();
    }

    public Rol? LeerRol(int idRol)
    {
        throw new NotImplementedException();
    }

    public Usuario? LeerUsuario(int idUsuario)
    {
        throw new NotImplementedException();
    }

    public Usuario? LeerUsuario(string eMail)
    {
        throw new NotImplementedException();
    }

    public int LimpiarLog()
    {
        throw new NotImplementedException();
    }

    public void Reiniciar()
    {
        throw new NotImplementedException();
    }
}
