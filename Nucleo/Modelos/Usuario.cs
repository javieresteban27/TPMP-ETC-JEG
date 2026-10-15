using Nucleo.Seguridad;

namespace Nucleo.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string Nombre_usuario { get; set; }
        public required string Mail { get; set; }

        // Se almacena el resultado del hash, no la contraseña en texto plano.
        public ResultadoHash Password { get; set; } = new();

        public int Intentos_fallidos { get; set; }
        public DateTime? Bloqueado_hasta { get; set; }

        // Propiedades usadas por los métodos de contraseña.
        public bool SinContraseña { get; set; }
        public DateTime? Ultimo_cambio_password { get; set; }
        public DateTime? Fecha_ultimo_acceso { get; set; }
        public DateTime? Fecha_acceso_actual { get; set; }

        private readonly List<ResultadoHash> _listaPasswords = new();

        public bool EstaBloqueado()
        {
            if (Bloqueado_hasta.HasValue &&
                DateTime.Now < Bloqueado_hasta.Value)
            {
                return true;
            }

            if (Bloqueado_hasta.HasValue)
            {
                Bloqueado_hasta = null;
                Intentos_fallidos = 0;
            }

            return false;
        }

        public void RegistrarFallo()
        {
            if (EstaBloqueado())
            {
                return;
            }

            Intentos_fallidos++;

            if (Intentos_fallidos >= 3)
            {
                Bloqueado_hasta = DateTime.Now.AddMinutes(5);
            }
        }

        public void RegistrarExito()
        {
            Intentos_fallidos = 0;
            Bloqueado_hasta = null;
        }

        public bool CompruebaPassword(string password)
        {
            if (EstaBloqueado() || SinContraseña)
            {
                return false;
            }

            if (!ServicioHash.Verificar(password, Password))
            {
                RegistrarFallo();
                return false;
            }

            RegistrarExito();
            Fecha_ultimo_acceso = Fecha_acceso_actual;
            Fecha_acceso_actual = DateTime.Now;

            return true;
        }

        public bool CambiaPassword(string nueva)
        {
            if (!SinContraseña || string.IsNullOrWhiteSpace(nueva))
            {
                return false;
            }

            Password = ServicioHash.Generar(nueva);
            Ultimo_cambio_password = DateTime.Now;
            SinContraseña = false;

            if (_listaPasswords.Count > 10)
            {
                _listaPasswords.RemoveAt(0);
            }

            _listaPasswords.Add(Password);
            return true;
        }

        public bool CambiaPassword(string anterior, string nueva)
        {
            if (string.IsNullOrWhiteSpace(nueva) ||
                anterior == nueva ||
                !CompruebaPassword(anterior))
            {
                return false;
            }

            bool puedeCambiar = true;

            foreach (ResultadoHash passwordAnterior in _listaPasswords)
            {
                puedeCambiar &= !ServicioHash.Verificar(nueva, passwordAnterior);
            }

            if (!puedeCambiar)
            {
                return false;
            }

            ResultadoHash passwordActual = Password;
            Password = ServicioHash.Generar(nueva);
            Ultimo_cambio_password = DateTime.Now;

            if (_listaPasswords.Count > 10)
            {
                _listaPasswords.RemoveAt(0);
            }

            _listaPasswords.Add(passwordActual);
            return true;
        }

        public override string ToString()
        {
            return $"Usuario {{ IdUsuario: {IdUsuario}, Nombre_usuario: {Nombre_usuario}, Mail: {Mail} }}";
        }

        
    }
}