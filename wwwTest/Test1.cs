using System;
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
                TimeSpan.FromSeconds(5);
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

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Identifícate"));
        }


        [TestMethod]
        public void Test02_Login_ExisteCampoUsuario()
        {
            IrALogin();

            IWebElement campo =
                driver.FindElement(
                    By.Id("txtUsuario"));

            Assert.IsNotNull(campo);
            Assert.IsTrue(campo.Displayed);
        }


        [TestMethod]
        public void Test03_Login_ExisteBotonSiguiente()
        {
            IrALogin();

            IWebElement boton =
                driver.FindElement(
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

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login/Index2"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtContraseña"))
                    .Displayed);
        }


        [TestMethod]
        public void Test05_Login_CorreoVacio_NoAvanza()
        {
            IrALogin();

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();

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

            usuario.SendKeys(
                "correo-incorrecto");

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();

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

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtUsuario"))
                    .Displayed);
        }


        [TestMethod]
        public void Test08_Password_ExisteCampoPassword()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                driver.FindElement(
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
                driver.FindElement(
                    By.Id("btnAceptar"));

            IWebElement atras =
                driver.FindElement(
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

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtUsuario"))
                    .Displayed);
        }


        [TestMethod]
        public void Test11_Password_UsuarioInexistente_MuestraError()
        {
            IrAPaginaPassword(
                "noexiste@test.com");

            IWebElement password =
                driver.FindElement(
                    By.Id("txtContraseña"));

            password.SendKeys(
                "Clave123!");

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "incorrectos"));
        }


        [TestMethod]
        public void Test12_Password_Incorrecta_MuestraError()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                driver.FindElement(
                    By.Id("txtContraseña"));

            password.SendKeys(
                "PasswordIncorrecta");

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "incorrectos"));

            // Se entra correctamente después
            // para restablecer los intentos fallidos.
            password =
                driver.FindElement(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                PasswordCorrecta);

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Principal"));
        }


        // =========================================================
        // INICIO DE SESION CORRECTO
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

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtUsuario"))
                    .Displayed);
        }


        [TestMethod]
        public void Test15_Principal_ConSesion_MuestraZonaTrabajo()
        {
            IniciarSesion();

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
                driver.FindElement(
                    By.CssSelector(
                        ".dropdown-toggle"));

            Assert.IsTrue(
                menu.Displayed);

            Assert.IsTrue(
                menu.Enabled);
        }


        // =========================================================
        // PERFIL DE USUARIO
        // =========================================================

        [TestMethod]
        public void Test17_Perfil_SinSesion_RedireccionaLogin()
        {
            driver.Navigate().GoToUrl(
                BaseUrl + "/Usuarios/Perfil");

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtUsuario"))
                    .Displayed);
        }


        [TestMethod]
        public void Test18_Perfil_ConSesion_MuestraDatosUsuario()
        {
            IniciarSesion();

            driver.Navigate().GoToUrl(
                BaseUrl + "/Usuarios/Perfil");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Mi perfil"));

            IWebElement email =
                driver.FindElement(
                    By.Id("EMail"));

            IWebElement nombre =
                driver.FindElement(
                    By.Id("Nombre"));

            IWebElement apellidos =
                driver.FindElement(
                    By.Id("Apellidos"));

            Assert.AreEqual(
                EmailCorrecto,
                email.GetAttribute("value"));

            Assert.AreEqual(
                "Usuario",
                nombre.GetAttribute("value"));

            Assert.AreEqual(
                "de prueba",
                apellidos.GetAttribute("value"));
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

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Login"));

            Assert.IsTrue(
                driver.FindElement(
                    By.Id("txtUsuario"))
                    .Displayed);
        }


        [TestMethod]
        public void Test20_Proyectos_ConSesion_MuestraFormulario()
        {
            IniciarSesion();

            driver.Navigate().GoToUrl(
                BaseUrl +
                "/Proyectos/Seleccionar");

            Assert.IsTrue(
                driver.PageSource.Contains(
                    "Gestión de proyectos"));

            IWebElement nombre =
                driver.FindElement(
                    By.Id("Nombre"));

            IWebElement siglas =
                driver.FindElement(
                    By.Id("Siglas"));

            IWebElement descripcion =
                driver.FindElement(
                    By.Id("Descripcion"));

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
        }


        private void IrAPaginaPassword(
            string email)
        {
            IrALogin();

            IWebElement usuario =
                driver.FindElement(
                    By.Id("txtUsuario"));

            usuario.Clear();
            usuario.SendKeys(email);

            driver.FindElement(
                By.Id("btnSiguiente"))
                .Click();
        }


        private void IniciarSesion()
        {
            IrAPaginaPassword(
                EmailCorrecto);

            IWebElement password =
                driver.FindElement(
                    By.Id("txtContraseña"));

            password.Clear();

            password.SendKeys(
                PasswordCorrecta);

            driver.FindElement(
                By.Id("btnAceptar"))
                .Click();

            Assert.IsTrue(
                driver.Url.Contains(
                    "/Principal"),
                "No se ha accedido a la página Principal.");
        }
    }
}