using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace wwwTest
{
    [TestClass]
    [DoNotParallelize]
    public class PruebasSelenium
    {
        private IWebDriver driver = null!;

        private const string BaseUrl =
            "https://localhost:7056";

        private const string EmailCorrecto =
            "usuario@test.com";

        private const string PasswordCorrecta =
            "Clave123!";


        [TestInitialize]
        public void Inicializar()
        {
            ChromeOptions opciones =
                new ChromeOptions();

            opciones.AcceptInsecureCertificates = true;

            driver =
                new ChromeDriver(opciones);

            driver.Manage()
                .Timeouts()
                .ImplicitWait =
                TimeSpan.FromSeconds(2);
        }


        [TestCleanup]
        public void Finalizar()
        {
            driver.Quit();
            driver.Dispose();
        }


        // =========================================================
        // LOGIN - PRIMER PASO
        // =========================================================

        [TestMethod]
        public void Test01_Login_CargaLaPagina()
        {
            IrALogin();

            EsperarTexto(
                "Identifícate");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Identifícate"));
        }


        [TestMethod]
        public void Test02_Login_ExisteCampoUsuario()
        {
            IrALogin();

            IWebElement campo =
                EsperarElemento(
                    By.Id("txtUsuario"));

            Assert.IsNotNull(campo);
            Assert.IsTrue(campo.Displayed);
        }


        [TestMethod]
        public void Test03_Login_ExisteBotonSiguiente()
        {
            IrALogin();

            IWebElement boton =
                EsperarElemento(
                    By.Id("btnSiguiente"));

            Assert.IsNotNull(boton);
            Assert.IsTrue(boton.Displayed);
            Assert.IsTrue(boton.Enabled);
        }


        [TestMethod]
        public void Test04_Login_CorreoValido_MuestraPassword()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            EsperarUrlContenga(
                "/Login/Index2");

            Assert.IsTrue(
                password.Displayed);

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login/Index2"),
                $"URL actual: {driver.Url}");
        }


        [TestMethod]
        public void Test05_Login_CorreoVacio_NoAvanza()
        {
            IrALogin();

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();

            EsperarElemento(
                By.Id("txtUsuario"));

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.AreEqual(
                0,
                driver.FindElements(
                    By.Id("txtContraseña"))
                    .Count);
        }


        [TestMethod]
        public void Test06_Login_CorreoFormatoIncorrecto_NoAvanza()
        {
            IrALogin();

            IWebElement usuario =
                driver.FindElement(
                    By.Id("txtUsuario"));

            usuario.Clear();

            usuario.SendKeys(
                "correo-incorrecto");

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();

            EsperarElemento(
                By.Id("txtUsuario"));

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.AreEqual(
                0,
                driver.FindElements(
                    By.Id("txtContraseña"))
                    .Count);
        }


        // =========================================================
        // LOGIN - SEGUNDO PASO
        // =========================================================

        [TestMethod]
        public void Test07_Password_AccesoDirecto_RedireccionaLogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl + "/Login/Index2");

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            EsperarUrlContenga(
                "/Login");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                usuario.Displayed);
        }


        [TestMethod]
        public void Test08_Password_ExisteCampoPassword()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            Assert.IsNotNull(password);
            Assert.IsTrue(password.Displayed);
        }


        [TestMethod]
        public void Test09_Password_ExistenBotonesAceptarYAtras()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement aceptar =
                EsperarElemento(
                    By.Id("btnAceptar"));

            IWebElement atras =
                EsperarElemento(
                    By.Id("btnAtras"));

            Assert.IsTrue(
                aceptar.Displayed);

            Assert.IsTrue(
                atras.Displayed);

            Assert.IsTrue(
                aceptar.Enabled);

            Assert.IsTrue(
                atras.Enabled);
        }


        [TestMethod]
        public void Test10_Password_BotonAtras_VuelveLogin()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            driver.FindElement(
                By.Id("btnAtras"))
                .Click();

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            EsperarUrlContenga(
                "/Login");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                usuario.Displayed);
        }


        [TestMethod]
        public void Test11_Password_UsuarioInexistente_MuestraError()
        {
            IrAPaginaPassword(
                "noexiste@test.com");

            IWebElement password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                "Clave123!");

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            EsperarTexto(
                "incorrectos");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "incorrectos"));

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login/Index2"));
        }


        [TestMethod]
        public void Test12_Password_Incorrecta_MuestraError()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                "PasswordIncorrecta");

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            EsperarTexto(
                "incorrectos");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "incorrectos"));

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login/Index2"));

            // Entramos después correctamente para dejar
            // los intentos erróneos otra vez a cero.

            password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                PasswordCorrecta);

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            EsperarUrlContenga(
                "/Principal");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Principal"),
                $"URL actual: {driver.Url}");
        }


        // =========================================================
        // LOGIN CORRECTO
        // =========================================================

        [TestMethod]
        public void Test13_Login_CredencialesCorrectas_AccedePrincipal()
        {
            IniciarSesion();

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Principal"));
        }


        // =========================================================
        // PAGINA PRINCIPAL
        // =========================================================

        [TestMethod]
        public void Test14_Principal_SinSesion_RedireccionaLogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl + "/Principal");

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            EsperarUrlContenga(
                "/Login");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"),
                $"URL actual: {driver.Url}");

            Assert.IsTrue(
                usuario.Displayed);
        }


        [TestMethod]
        public void Test15_Principal_ConSesion_MuestraZonaTrabajo()
        {
            IniciarSesion();

            EsperarTexto(
                "Zona de trabajo");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Zona de trabajo"));

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Contenido de la página principal"));
        }


        [TestMethod]
        public void Test16_Principal_ConSesion_MuestraMenuUsuario()
        {
            IniciarSesion();

            IWebElement menu =
                EsperarElemento(
                    By.CssSelector(
                        ".dropdown-toggle"));

            Assert.IsTrue(
                menu.Displayed);

            Assert.IsTrue(
                menu.Enabled);
        }


        // =========================================================
        // PERFIL
        // =========================================================

        [TestMethod]
        public void Test17_Perfil_SinSesion_RedireccionaLogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl +
                "/Usuarios/Perfil");

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            EsperarUrlContenga(
                "/Login");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                usuario.Displayed);
        }


        [TestMethod]
        public void Test18_Perfil_ConSesion_MuestraDatosUsuario()
        {
            IniciarSesion();

            driver.Navigate().GoToUrl(
                BaseUrl +
                "/Usuarios/Perfil");

            IWebElement email =
                EsperarElemento(
                    By.Id("EMail"));

            IWebElement nombre =
                EsperarElemento(
                    By.Id("Nombre"));

            IWebElement apellidos =
                EsperarElemento(
                    By.Id("Apellidos"));

            EsperarTexto(
                "Mi perfil");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Mi perfil"));

            Assert.AreEqual(
                EmailCorrecto,
                email.GetAttribute(
                    "value"));

            Assert.AreEqual(
                "Usuario",
                nombre.GetAttribute(
                    "value"));

            Assert.AreEqual(
                "de prueba",
                apellidos.GetAttribute(
                    "value"));
        }


        // =========================================================
        // PROYECTOS
        // =========================================================

        [TestMethod]
        public void Test19_Proyectos_SinSesion_RedireccionaLogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl +
                "/Proyectos/Seleccionar");

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            EsperarUrlContenga(
                "/Login");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                usuario.Displayed);
        }


        [TestMethod]
        public void Test20_Proyectos_ConSesion_MuestraFormulario()
        {
            IniciarSesion();

            driver.Navigate().GoToUrl(
                BaseUrl +
                "/Proyectos/Seleccionar");

            IWebElement nombre =
                EsperarElemento(
                    By.Id("Nombre"));

            IWebElement siglas =
                EsperarElemento(
                    By.Id("Siglas"));

            IWebElement descripcion =
                EsperarElemento(
                    By.Id("Descripcion"));

            EsperarTexto(
                "Gestión de proyectos");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Gestión de proyectos"));

            Assert.IsTrue(
                nombre.Displayed);

            Assert.IsTrue(
                siglas.Displayed);

            Assert.IsTrue(
                descripcion.Displayed);

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Añadir proyecto"));
        }


        // =========================================================
        // METODOS AUXILIARES
        // =========================================================

        private void IrALogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl + "/Login");

            EsperarElemento(
                By.Id("txtUsuario"));
        }


        private void IrAPaginaPassword(
            string email)
        {
            IrALogin();

            IWebElement usuario =
                EsperarElemento(
                    By.Id("txtUsuario"));

            usuario.Clear();

            usuario.SendKeys(
                email);

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();

            EsperarElemento(
                By.Id("txtContraseña"));

            EsperarUrlContenga(
                "/Login/Index2");
        }


        private void IniciarSesion()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                EsperarElemento(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                PasswordCorrecta);

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            EsperarUrlContenga(
                "/Principal");

            EsperarTexto(
                "Zona de trabajo");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Principal"),
                "No se ha accedido a Principal. " +
                $"URL actual: {driver.Url}");
        }


        // =========================================================
        // ESPERAS
        // =========================================================

        private IWebElement EsperarElemento(
            By localizador)
        {
            IWebElement? encontrado = null;

            EsperarHasta(
                () =>
                {
                    try
                    {
                        IWebElement elemento =
                            driver.FindElement(
                                localizador);

                        if (elemento.Displayed)
                        {
                            encontrado =
                                elemento;

                            return true;
                        }
                    }
                    catch (
                        NoSuchElementException)
                    {
                    }
                    catch (
                        StaleElementReferenceException)
                    {
                    }

                    return false;
                });

            if (encontrado is null)
            {
                Assert.Fail(
                    $"No se encontró el elemento {localizador}. " +
                    $"URL actual: {driver.Url}");
            }

            return encontrado!;
        }


        private void EsperarUrlContenga(
            string texto)
        {
            bool correcto =
                EsperarHasta(
                    () =>
                        driver.Url.Contains(
                            texto,
                            StringComparison
                                .OrdinalIgnoreCase));

            Assert.IsTrue(
                correcto,
                $"La URL no contiene '{texto}'. " +
                $"URL actual: {driver.Url}");
        }


        private void EsperarTexto(
            string texto)
        {
            bool correcto =
                EsperarHasta(
                    () =>
                        driver.PageSource.Contains(
                            texto,
                            StringComparison
                                .OrdinalIgnoreCase));

            Assert.IsTrue(
                correcto,
                $"No se encontró el texto '{texto}'. " +
                $"URL actual: {driver.Url}");
        }


        private bool EsperarHasta(
            Func<bool> condicion)
        {
            DateTime limite =
                DateTime.Now.AddSeconds(10);

            while (DateTime.Now < limite)
            {
                try
                {
                    if (condicion())
                    {
                        return true;
                    }
                }
                catch (
                    WebDriverException)
                {
                }

                Thread.Sleep(100);
            }

            return false;
        }
    }
}