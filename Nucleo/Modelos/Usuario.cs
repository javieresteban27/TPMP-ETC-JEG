using Nucleo.Seguridad;

namespace Nucleo.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string Nombre_usuario { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Mail { get; set; }

        // Se almacena el resultado del hash, no la contraseña en texto plano.
        public ResultadoHash Password { get; set; } = new();

        public bool TieneCredencialesConfiguradas { get; set; }
        public bool Activa { get; set; } = true;
        public int IntentosErroneos { get; set; }
        public int ListaPasswordsCount => _listaPasswords.Count;

        // DateTime.MinValue representa una fecha aún no registrada.
        public DateTime FechaUltimoAcceso { get; set; } = DateTime.MinValue;
        public DateTime FechaAccesoActual { get; set; } = DateTime.MinValue;
        public DateTime UltimoCambioPassword { get; set; } = DateTime.MinValue;
        public DateTime BloquearAccesoHasta { get; set; } = DateTime.MinValue;
        public DateTime CaducidadCuenta { get; set; } = DateTime.MinValue;

        private readonly List<ResultadoHash> _listaPasswords = new();

        public bool EstaBloqueado()
        {
            if (BloquearAccesoHasta == DateTime.MinValue)
            {
                return false;
            }

            if (DateTime.Now < BloquearAccesoHasta)
            {
                return true;
            }

            BloquearAccesoHasta = DateTime.MinValue;
            IntentosErroneos = 0;

            return false;
        }

        public void RegistrarFallo()
        {
            if (EstaBloqueado())
            {
                return;
            }

            IntentosErroneos++;

            if (IntentosErroneos >= 3)
            {
                BloquearAccesoHasta = DateTime.Now.AddMinutes(5);
            }
        }

        public void RegistrarExito()
        {
            IntentosErroneos = 0;
            BloquearAccesoHasta = DateTime.MinValue;
        }

        public bool CompruebaPassword(string password)
        {
            if (EstaBloqueado() || !TieneCredencialesConfiguradas)
            {
                return false;
            }

            if (!ServicioHash.Verificar(password, Password))
            {
                RegistrarFallo();
                return false;
            }

            RegistrarExito();
            FechaUltimoAcceso = FechaAccesoActual;
            FechaAccesoActual = DateTime.Now;

            return true;
        }

        public bool CambiaPassword(string nueva)
        {
            if (TieneCredencialesConfiguradas ||
                string.IsNullOrWhiteSpace(nueva))
            {
                return false;
            }

            Password = ServicioHash.Generar(nueva);
            UltimoCambioPassword = DateTime.Now;
            TieneCredencialesConfiguradas = true;

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
            UltimoCambioPassword = DateTime.Now;

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