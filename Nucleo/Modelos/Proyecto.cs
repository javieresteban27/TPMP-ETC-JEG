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
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public string? Codigo { get; set; }

        public DateTime FechaCreacionUtc { get; init; } = DateTime.UtcNow;

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