namespace Nucleo.Modelos
{
    public enum EstadoProyecto
    {
        Planificado,
        EnCurso,
        Finalizado,
        Cancelado
    }

    public class Proyecto
    {
        private int _id;
        private DateTime _fechaCreacionUtc = DateTime.UtcNow;

        public int IdProyecto
        {
            get => _id;
            set => _id = value;
        }

        // Alias para mantener la compatibilidad con el código existente.
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public string? Codigo { get; set; }

        public string? Siglas { get; set; }

        public DateTime FechaCreacion
        {
            get => _fechaCreacionUtc;
            init => _fechaCreacionUtc = value;
        }

        // Alias para mantener la compatibilidad con el código existente.
        public DateTime FechaCreacionUtc
        {
            get => _fechaCreacionUtc;
            init => _fechaCreacionUtc = value;
        }

        public DateTime FechaInicioUtc { get; set; } = DateTime.UtcNow;

        public DateTime? FechaFinUtc { get; private set; }

        public bool Activo { get; private set; } = true;

        public EstadoProyecto Estado { get; private set; }
            = EstadoProyecto.Planificado;

        public void Iniciar()
        {
            Activo = true;
            Estado = EstadoProyecto.EnCurso;
            FechaFinUtc = null;
        }

        public void Finalizar()
        {
            FechaFinUtc = DateTime.UtcNow;
            Estado = EstadoProyecto.Finalizado;
            Activo = false;
        }

        public void Cancelar()
        {
            FechaFinUtc = DateTime.UtcNow;
            Estado = EstadoProyecto.Cancelado;
            Activo = false;
        }

        public void Reactivar()
        {
            FechaFinUtc = null;
            Estado = EstadoProyecto.EnCurso;
            Activo = true;
        }

        public bool EstaFinalizado()
        {
            return Estado == EstadoProyecto.Finalizado;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}