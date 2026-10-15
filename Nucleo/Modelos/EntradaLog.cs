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
            Usuario = "Sistema";
        }

        public DateTime FechaHora { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Usuario { get; set; } = "Sistema";

        public override string ToString()
        {
            return $"{FechaHora:dd/MM/yyyy HH:mm:ss} [{Nivel}] {Mensaje} - {Usuario}";
        }
    }
}