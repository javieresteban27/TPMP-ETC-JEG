namespace Nucleo.Modelos
{
    public class Proyecto
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public DateTime FechaInicioUtc { get; set; } = DateTime.UtcNow;

        public DateTime? FechaFinUtc { get; set; }

        public bool Activo { get; set; } = true;
    }
}