using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

        // Método para comprobar si la cuenta está bloqueada
        public bool estaBloqueado()
        {
            if (bloqueado_hasta.HasValue && DateTime.Now < bloqueado_hasta.Value)
            {
                return true; // Sigue bloqueado
            }
            return false; // El bloqueo ya expiró o nunca se bloqueó

        }

        // Método que se ejecuta al fallar la contraseña
        public void registrarFallo()
        {
            intentos_fallidos++;

            // Si llega a 3 intentos fallidos, bloqueamos la cuenta por 5 minutos
            if (intentos_fallidos >= 3)
            {
                bloqueado_hasta = DateTime.Now.AddMinutes(5);
                intentos_fallidos = 0; // Reiniciamos el contador de intentos fallidos
                bloqueado_hasta = null; // Reiniciamos el bloqueo después de que expire
            }
        }
    }
}
