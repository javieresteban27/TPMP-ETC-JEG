namespace Nucleo.Modelos
{
    public class Rol
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public bool Activo { get; private set; } = true;

        public DateTime FechaCreacionUtc { get; init; } = DateTime.UtcNow;

        public void Activar()
        {
            Activo = true;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}