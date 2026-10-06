using System;
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

            Assert.AreEqual(32, clave.Length);
        }

        [TestMethod]
        public void GenerarClave_DosLlamadas_GeneranClavesDistintas()
        {
            string clave1 = ServicioCifrado.GenerarClave();
            string clave2 = ServicioCifrado.GenerarClave();

            Assert.AreNotEqual(clave1, clave2);
        }

        [TestMethod]
        public void CifrarYDescifrar_RetornaTextoOriginal()
        {
            string clave = ServicioCifrado.GenerarClave();

            string textoOriginal =
                "Texto de prueba con caracteres: ñ á é í ó ú";

            string cifrado =
                ServicioCifrado.Cifrar(textoOriginal, clave);

            string descifrado =
                ServicioCifrado.Descifrar(cifrado, clave);

            Assert.AreEqual(textoOriginal, descifrado);
        }

        [TestMethod]
        public void Cifrar_MismoTextoDosVeces_GeneraResultadosDistintos()
        {
            string clave = ServicioCifrado.GenerarClave();
            string texto = "mensaje secreto";

            string cifrado1 =
                ServicioCifrado.Cifrar(texto, clave);

            string cifrado2 =
                ServicioCifrado.Cifrar(texto, clave);

            Assert.AreNotEqual(cifrado1, cifrado2);
        }

        [TestMethod]
        public void Cifrar_TextoNulo_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarTextoNulo);
        }

        [TestMethod]
        public void Cifrar_TextoVacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarTextoVacio);
        }

        [TestMethod]
        public void Cifrar_TextoEspacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarTextoEspacio);
        }

        [TestMethod]
        public void Cifrar_TextoTabulador_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarTextoTabulador);
        }

        [TestMethod]
        public void Cifrar_ClaveNula_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveNula);
        }

        [TestMethod]
        public void Cifrar_ClaveVacia_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveVacia);
        }

        [TestMethod]
        public void Cifrar_ClaveEspacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveEspacio);
        }

        [TestMethod]
        public void Cifrar_ClaveTabulador_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveTabulador);
        }

        [TestMethod]
        public void Cifrar_ClaveFormatoInvalido_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveFormatoInvalido);
        }

        [TestMethod]
        public void Cifrar_ClaveBase64ConLongitudIncorrecta_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                CifrarClaveLongitudIncorrecta);
        }

        [TestMethod]
        public void Descifrar_ContenidoNulo_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoNulo);
        }

        [TestMethod]
        public void Descifrar_ContenidoVacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoVacio);
        }

        [TestMethod]
        public void Descifrar_ContenidoEspacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoEspacio);
        }

        [TestMethod]
        public void Descifrar_ContenidoTabulador_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoTabulador);
        }

        [TestMethod]
        public void Descifrar_ContenidoNoBase64_LanzaFormatException()
        {
            Assert.ThrowsExactly<FormatException>(
                DescifrarContenidoNoBase64);
        }

        [TestMethod]
        public void Descifrar_ContenidoDemasiadoCorto_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoDemasiadoCorto);
        }

        [TestMethod]
        public void Descifrar_ContenidoConSoloNonceYEtiqueta_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarContenidoSoloNonceYEtiqueta);
        }

        [TestMethod]
        public void Descifrar_ClaveNula_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveNula);
        }

        [TestMethod]
        public void Descifrar_ClaveVacia_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveVacia);
        }

        [TestMethod]
        public void Descifrar_ClaveEspacio_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveEspacio);
        }

        [TestMethod]
        public void Descifrar_ClaveTabulador_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveTabulador);
        }

        [TestMethod]
        public void Descifrar_ClaveFormatoInvalido_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveFormatoInvalido);
        }

        [TestMethod]
        public void Descifrar_ClaveBase64ConLongitudIncorrecta_LanzaArgumentException()
        {
            Assert.ThrowsExactly<ArgumentException>(
                DescifrarClaveLongitudIncorrecta);
        }

        [TestMethod]
        public void Descifrar_DatosCorruptos_LanzaAuthenticationTagMismatchException()
        {
            Assert.ThrowsExactly<AuthenticationTagMismatchException>(
                DescifrarDatosCorruptos);
        }

        [TestMethod]
        public void Descifrar_ClaveDistinta_LanzaAuthenticationTagMismatchException()
        {
            Assert.ThrowsExactly<AuthenticationTagMismatchException>(
                DescifrarClaveDistinta);
        }


        private static void CifrarTextoNulo()
        {
            string clave = ServicioCifrado.GenerarClave();

            ServicioCifrado.Cifrar(null!, clave);
        }

        private static void CifrarTextoVacio()
        {
            string clave = ServicioCifrado.GenerarClave();

            ServicioCifrado.Cifrar("", clave);
        }

        private static void CifrarTextoEspacio()
        {
            string clave = ServicioCifrado.GenerarClave();

            ServicioCifrado.Cifrar(" ", clave);
        }

        private static void CifrarTextoTabulador()
        {
            string clave = ServicioCifrado.GenerarClave();

            ServicioCifrado.Cifrar("\t", clave);
        }

        private static void CifrarClaveNula()
        {
            ServicioCifrado.Cifrar(
                "mensaje",
                null!);
        }

        private static void CifrarClaveVacia()
        {
            ServicioCifrado.Cifrar(
                "mensaje",
                "");
        }

        private static void CifrarClaveEspacio()
        {
            ServicioCifrado.Cifrar(
                "mensaje",
                " ");
        }

        private static void CifrarClaveTabulador()
        {
            ServicioCifrado.Cifrar(
                "mensaje",
                "\t");
        }

        private static void CifrarClaveFormatoInvalido()
        {
            ServicioCifrado.Cifrar(
                "mensaje",
                "no-es-base64!!");
        }

        private static void CifrarClaveLongitudIncorrecta()
        {
            string claveCorta =
                Convert.ToBase64String(new byte[16]);

            ServicioCifrado.Cifrar(
                "mensaje",
                claveCorta);
        }

        private static void DescifrarContenidoNulo()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            ServicioCifrado.Descifrar(
                null!,
                clave);
        }

        private static void DescifrarContenidoVacio()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            ServicioCifrado.Descifrar(
                "",
                clave);
        }

        private static void DescifrarContenidoEspacio()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            ServicioCifrado.Descifrar(
                " ",
                clave);
        }

        private static void DescifrarContenidoTabulador()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            ServicioCifrado.Descifrar(
                "\t",
                clave);
        }

        private static void DescifrarContenidoNoBase64()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            ServicioCifrado.Descifrar(
                "no-es-base64!!",
                clave);
        }

        private static void DescifrarContenidoDemasiadoCorto()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            string contenido =
                Convert.ToBase64String(
                    new byte[] { 1, 2, 3, 4 });

            ServicioCifrado.Descifrar(
                contenido,
                clave);
        }

        private static void DescifrarContenidoSoloNonceYEtiqueta()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            string contenido =
                Convert.ToBase64String(
                    new byte[TamanoNonce + TamanoEtiqueta]);

            ServicioCifrado.Descifrar(
                contenido,
                clave);
        }

        private static void DescifrarClaveNula()
        {
            string claveValida =
                ServicioCifrado.GenerarClave();

            string cifrado =
                ServicioCifrado.Cifrar(
                    "mensaje",
                    claveValida);

            ServicioCifrado.Descifrar(
                cifrado,
                null!);
        }

        private static void DescifrarClaveVacia()
        {
            DescifrarConClaveInvalida("");
        }

        private static void DescifrarClaveEspacio()
        {
            DescifrarConClaveInvalida(" ");
        }

        private static void DescifrarClaveTabulador()
        {
            DescifrarConClaveInvalida("\t");
        }

        private static void DescifrarClaveFormatoInvalido()
        {
            DescifrarConClaveInvalida(
                "!!!clave-no-valida-base64");
        }

        private static void DescifrarClaveLongitudIncorrecta()
        {
            string claveCorta =
                Convert.ToBase64String(new byte[16]);

            DescifrarConClaveInvalida(claveCorta);
        }

        private static void DescifrarConClaveInvalida(
            string claveInvalida)
        {
            string claveValida =
                ServicioCifrado.GenerarClave();

            string cifrado =
                ServicioCifrado.Cifrar(
                    "mensaje",
                    claveValida);

            ServicioCifrado.Descifrar(
                cifrado,
                claveInvalida);
        }

        private static void DescifrarDatosCorruptos()
        {
            string clave =
                ServicioCifrado.GenerarClave();

            string cifrado =
                ServicioCifrado.Cifrar(
                    "mensaje secreto",
                    clave);

            byte[] datos =
                Convert.FromBase64String(cifrado);

            int indiceTextoCifrado =
                TamanoNonce + TamanoEtiqueta;

            datos[indiceTextoCifrado] ^= 0xFF;

            string contenidoCorrupto =
                Convert.ToBase64String(datos);

            ServicioCifrado.Descifrar(
                contenidoCorrupto,
                clave);
        }

        private static void DescifrarClaveDistinta()
        {
            string clave1 =
                ServicioCifrado.GenerarClave();

            string clave2 =
                ServicioCifrado.GenerarClave();

            string cifrado =
                ServicioCifrado.Cifrar(
                    "otro mensaje",
                    clave1);

            ServicioCifrado.Descifrar(
                cifrado,
                clave2);
        }
    }
}