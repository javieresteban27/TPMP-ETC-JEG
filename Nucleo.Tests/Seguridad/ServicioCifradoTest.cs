using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Seguridad;

namespace NucleoTest.Seguridad
{
    [TestClass]
    public class ServicioCifradoTest
    {
        private const int TamanoNonce = 12;
        private const int TamanoEtiqueta = 16;

        [TestMethod]
        public void GenerarClave_RetornaBase64De32Bytes()
        {
            string claveBase64 = ServicioCifrado.GenerarClave();

            byte[] clave = Convert.FromBase64String(claveBase64);

            Assert.HasCount(32, clave, "La clave debe tener 32 bytes (AES-256).");
        }

        [TestMethod]
        public void CifrarYDescifrar_RetornoTextoOriginal()
        {
            string clave = ServicioCifrado.GenerarClave();
            string textoOriginal = "Texto de prueba con caracteres: ñ á é í ó ú 😊";

            string cifrado = ServicioCifrado.Cifrar(textoOriginal, clave);
            string descifrado = ServicioCifrado.Descifrar(cifrado, clave);

            Assert.AreEqual(textoOriginal, descifrado);
        }

        [TestMethod]
        public void Cifrar_TextoVacio_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();

            try
            {
                ServicioCifrado.Cifrar("", clave);
                Assert.Fail("Se esperaba ArgumentException al cifrar texto vacío.");
            }
            catch (ArgumentException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Cifrar_ClaveInvalida_LanzaArgumentException()
        {
            string texto = "algún texto";
            string claveInvalida = "no-es-base64!!";

            try
            {
                ServicioCifrado.Cifrar(texto, claveInvalida);
                Assert.Fail("Se esperaba ArgumentException al usar una clave inválida.");
            }
            catch (ArgumentException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Descifrar_ContenidoVacio_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();

            try
            {
                ServicioCifrado.Descifrar("", clave);
                Assert.Fail("Se esperaba ArgumentException al descifrar contenido vacío.");
            }
            catch (ArgumentException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Descifrar_ContenidoFormatoInvalido_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();

            // Base64 válido pero contenido demasiado corto para incluir nonce+etiqueta
            string contenidoCorto = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 });

            try
            {
                ServicioCifrado.Descifrar(contenidoCorto, clave);
                Assert.Fail("Se esperaba ArgumentException por formato inválido del contenido cifrado.");
            }
            catch (ArgumentException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Descifrar_ClaveInvalida_LanzaArgumentException()
        {
            string claveValida = ServicioCifrado.GenerarClave();
            string texto = "prueba";
            string cifrado = ServicioCifrado.Cifrar(texto, claveValida);

            string claveInvalida = "!!!clave-no-valida-base64";

            try
            {
                ServicioCifrado.Descifrar(cifrado, claveInvalida);
                Assert.Fail("Se esperaba ArgumentException al usar una clave de formato inválido.");
            }
            catch (ArgumentException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Descifrar_DatosCorruptos_LanzaCryptographicException()
        {
            string clave = ServicioCifrado.GenerarClave();
            string texto = "mensaje secreto";

            string cifradoBase64 = ServicioCifrado.Cifrar(texto, clave);
            byte[] datos = Convert.FromBase64String(cifradoBase64);

            // Corromper un byte dentro del texto cifrado (después de nonce+etiqueta)
            int indiceParaCorromper = TamanoNonce + TamanoEtiqueta;
            if (indiceParaCorromper >= datos.Length)
            {
                // Si por alguna razón no hay suficiente longitud, invertir el primer byte válido
                indiceParaCorromper = datos.Length - 1;
            }

            datos[indiceParaCorromper] ^= 0xFF;
            string corruptoBase64 = Convert.ToBase64String(datos);

            try
            {
                ServicioCifrado.Descifrar(corruptoBase64, clave);
                Assert.Fail("Se esperaba CryptographicException al descifrar datos corruptos.");
            }
            catch (CryptographicException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Descifrar_ClaveDistinta_LanzaCryptographicException()
        {
            string clave1 = ServicioCifrado.GenerarClave();
            string clave2 = ServicioCifrado.GenerarClave();
            string texto = "otro mensaje";

            string cifrado = ServicioCifrado.Cifrar(texto, clave1);

            try
            {
                ServicioCifrado.Descifrar(cifrado, clave2);
                Assert.Fail("Se esperaba CryptographicException al usar una clave distinta.");
            }
            catch (CryptographicException)
            {
                // Éxito
            }
        }

        [TestMethod]
        public void Cifrar_TextoNulo_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Cifrar(null!, clave));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        public void Cifrar_TextoEnBlanco_LanzaArgumentException(string texto)
        {
            string clave = ServicioCifrado.GenerarClave();

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Cifrar(texto, clave));
        }

        [TestMethod]
        public void Cifrar_ClaveNula_LanzaArgumentException()
        {
            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Cifrar("mensaje", null!));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        public void Cifrar_ClaveEnBlanco_LanzaArgumentException(string clave)
        {
            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Cifrar("mensaje", clave));
        }

        [TestMethod]
        public void Cifrar_ClaveBase64ConLongitudIncorrecta_LanzaArgumentException()
        {
            string claveCorta = Convert.ToBase64String(new byte[16]);

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Cifrar("mensaje", claveCorta));
        }

        [TestMethod]
        public void Descifrar_ContenidoNulo_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Descifrar(null!, clave));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        public void Descifrar_ContenidoEnBlanco_LanzaArgumentException(
            string contenido)
        {
            string clave = ServicioCifrado.GenerarClave();

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Descifrar(contenido, clave));
        }

        [TestMethod]
        public void Descifrar_ContenidoNoBase64_LanzaFormatException()
        {
            string clave = ServicioCifrado.GenerarClave();

            VerificarExcepcion<FormatException>(
                () => ServicioCifrado.Descifrar("no-es-base64!!", clave));
        }

        [TestMethod]
        public void Descifrar_ContenidoConSoloNonceYEtiqueta_LanzaArgumentException()
        {
            string clave = ServicioCifrado.GenerarClave();
            string contenido = Convert.ToBase64String(
                new byte[TamanoNonce + TamanoEtiqueta]);

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Descifrar(contenido, clave));
        }

        [TestMethod]
        public void Descifrar_ClaveBase64ConLongitudIncorrecta_LanzaArgumentException()
        {
            string claveCorta = Convert.ToBase64String(new byte[16]);
            string contenido = ServicioCifrado.Cifrar(
                "mensaje",
                ServicioCifrado.GenerarClave());

            VerificarExcepcion<ArgumentException>(
                () => ServicioCifrado.Descifrar(contenido, claveCorta));
        }

        private static void VerificarExcepcion<TExcepcion>(Action accion)
            where TExcepcion : Exception
        {
            try
            {
                accion();
            }
            catch (TExcepcion)
            {
                return;
            }

            Assert.Fail(
                $"Se esperaba una excepción de tipo {typeof(TExcepcion).Name}.");
        }
    }
}