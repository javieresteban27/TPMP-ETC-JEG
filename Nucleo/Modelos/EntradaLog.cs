namespace Nucleo.Modelos
{
    public enum AccionLog
    {
        UsuarioCreado,
        UsuarioEliminado,
        MailAñadido,
        MailEliminado,
        DatosUsuarioActualizados,
        CuentaBloqueada,
        CuentaDesbloqueada
    }

    public class EntradaLog
    {
        public int Id { get; set; }

        // Fecha de creación del evento en UTC.
        public DateTime FechaUtc { get; init; } = DateTime.UtcNow;

        public required AccionLog Accion { get; init; }

        // Usuario al que afecta el evento. Se conserva como dato, no necesariamente
        // como clave foránea, para que el registro sobreviva al borrado del usuario.
        public int? UsuarioId { get; init; }
        public string? NombreUsuario { get; init; }

        // Usuario que ejecutó la acción; null si fue una operación automática.
        public int? ActorUsuarioId { get; init; }

        // Útiles, por ejemplo, para auditar cambios de correo.
        public string? ValorAnterior { get; init; }
        public string? ValorNuevo { get; init; }

        public string? Detalles { get; init; }
    }
}