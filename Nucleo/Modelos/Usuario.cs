using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleo.Modelos
{
    public class Usuario
    {
        // Atributos referentes al usuario
        public int id { get; set; }
        public string nombre_usuario { get; set; }
        public string mail { get; set; }
        public string password;

        // Atributos referentes al control de bloqueo
        public int intentos_fallidos { get; set; } = 0;
        public DateTime? bloqueado_hasta { get; set; } = null;

    }
}
