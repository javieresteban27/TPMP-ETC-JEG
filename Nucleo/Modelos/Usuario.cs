using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Nucleo.Modelos
{
    public class Usuario
    {
        // Atributos referentes al usuario
        public int Id { get; set; }
        public required string Nombre_usuario { get; set; }
        public required string Mail { get; set; }
        public required string Password { get; set; }

        // Atributos referentes al control de bloqueo
        public int Intentos_fallidos { get; set; } = 0;
        public DateTime? Bloqueado_hasta { get; set; } = null;

        // Método para comprobar si la cuenta está bloqueada
        public bool EstaBloqueado()
        {
            if (Bloqueado_hasta.HasValue && DateTime.Now < Bloqueado_hasta.Value)
            {
                return true; // Sigue bloqueado
            }

            // Si el tiempo de bloqueo ya pasó, limpiamos el estado automáticamente
            if (Bloqueado_hasta.HasValue && DateTime.Now >= Bloqueado_hasta.Value)
            {
                Bloqueado_hasta = null; // Limpiamos el estado de bloqueo
                Intentos_fallidos = 0; // Reiniciamos el contador de intentos fallidos
            }

            return false; // El bloqueo ya expiró o nunca se bloqueó
        }

        // Método que se ejecuta al fallar la contraseña
        public void RegistrarFallo()
        {
            // Si ya está bloqueado pero expiró, nos aseguramos de resetear antes de sumar
            if (EstaBloqueado()) return;

            Intentos_fallidos++;

            // Si llega a 3 intentos fallidos, bloqueamos la cuenta 5 minutos
            if (Intentos_fallidos >= 3)
            {
                Bloqueado_hasta = DateTime.Now.AddMinutes(5);
            }
        }

        // Método opcional para cuando el usuario acierta la contraseña
        public void RegistrarExito()
        {
            Intentos_fallidos = 0;
            Bloqueado_hasta = null;
        }
    }
}
