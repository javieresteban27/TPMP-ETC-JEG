using System.Reflection.Metadata.Ecma335;

namespace Nucleo.Modelos
{
    public class EntradaLog
    {
        public EntradaLog(DateTime fechaHora, string nivel, string mensaje, string usuario)
        {
            FechaHora = fechaHora;
            Nivel = nivel;
            Mensaje = mensaje;
            Usuario = usuario;
        }

        public EntradaLog(string mensaje)
        {
            FechaHora = DateTime.Now;
            Nivel = "INFO";
            Mensaje = mensaje;
            Usuario = null;
        }
        public DateTime FechaHora { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }

        public override string ToString()
        {
            string usuario = Usuario?.ToString() ?? "Sistema";
            return $"{FechaHora:dd/MM/yyyy HH:mm:ss} [{Nivel}] {Mensaje} - {usuario}";
        }
    }
}