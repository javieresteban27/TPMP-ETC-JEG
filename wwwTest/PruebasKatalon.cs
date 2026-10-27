using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestClass]
    [DoNotParallelize]
    public class PruebasKatalon
    {
        private IWebDriver driver = null!;

        [TestInitialize]
        public void InitializeTest()
        {
            ChromeOptions opciones = new ChromeOptions();
            opciones.AcceptInsecureCertificates = true;
            driver = new ChromeDriver(opciones);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignorar errores al cerrar Chrome.
            }
        }

        [TestMethod]
        public void The01LoginCargaLaPaginaTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
            Assert.IsTrue(IsElementPresent(By.Id("btnSiguiente")));
        }

        [TestMethod]
        public void The02LoginCorreoVacioNoAvanzaTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
            Assert.IsFalse(IsElementPresent(By.Id("txtContraseña")));
        }

        [TestMethod]
        public void The03LoginCorreoIncorrectoNoAvanzaTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("correo-incorrecto");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
            Assert.IsFalse(IsElementPresent(By.Id("txtContraseña")));
        }

        [TestMethod]
        public void The04LoginPasaAPasswordTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtContraseña")));
            Assert.IsTrue(IsElementPresent(By.Id("btnAceptar")));
            Assert.IsTrue(IsElementPresent(By.Id("btnAtras")));
        }

        [TestMethod]
        public void The05PasswordBotonAtrasVuelveAlUsuarioTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("btnAtras"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("btnAtras")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
        }

        [TestMethod]
        public void The06PasswordUsuarioInexistenteMuestraErrorTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("noexiste@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".error"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("Usuario o contraseña incorrectos.", driver.FindElement(By.CssSelector(".error")).Text);
        }

        [TestMethod]
        public void The07PasswordIncorrectaMuestraErrorTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("PasswordIncorrecta");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".error"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("Usuario o contraseña incorrectos.", driver.FindElement(By.CssSelector(".error")).Text);
        }

        [TestMethod]
        public void The08LoginCorrectoEntraEnPrincipalTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("Zona de trabajo", driver.FindElement(By.CssSelector("h1")).Text);
            Assert.IsTrue(IsElementPresent(By.CssSelector(".dropdown-toggle")));
        }

        [TestMethod]
        public void The09MenuUsuarioMuestraOpcionesTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.XPath("//a[contains(normalize-space(.),'Ver perfil')]"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.XPath("//a[contains(normalize-space(.),'Ver perfil')]")));
            Assert.IsTrue(IsElementPresent(By.XPath("//a[contains(normalize-space(.),'Log de eventos')]")));
            Assert.IsTrue(IsElementPresent(By.XPath("//a[contains(normalize-space(.),'Seleccionar proyecto')]")));
            Assert.IsTrue(IsElementPresent(By.XPath("//button[contains(normalize-space(.),'Cerrar sesión')]")));
        }

        [TestMethod]
        public void The10MenuEntraEnPerfilYCompruebaDatosTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//a[contains(normalize-space(.),'Ver perfil')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("EMail"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("usuario@test.com", driver.FindElement(By.Id("EMail")).GetAttribute("value"));
            Assert.AreEqual("Usuario", driver.FindElement(By.Id("Nombre")).GetAttribute("value"));
            Assert.AreEqual("de prueba", driver.FindElement(By.Id("Apellidos")).GetAttribute("value"));
        }

        [TestMethod]
        public void The11PerfilModificaNombreYLoRestauraTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//a[contains(normalize-space(.),'Ver perfil')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("Nombre")).Clear();
            driver.FindElement(By.Id("Nombre")).SendKeys("Usuario Katalon");
            driver.FindElement(By.Id("Apellidos")).Clear();
            driver.FindElement(By.Id("Apellidos")).SendKeys("de prueba modificada");
            driver.FindElement(By.XPath("//button[contains(normalize-space(.),'Guardar cambios')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("Usuario Katalon", driver.FindElement(By.Id("Nombre")).GetAttribute("value"));
            Assert.AreEqual("de prueba modificada", driver.FindElement(By.Id("Apellidos")).GetAttribute("value"));
            driver.FindElement(By.Id("Nombre")).Clear();
            driver.FindElement(By.Id("Nombre")).SendKeys("Usuario");
            driver.FindElement(By.Id("Apellidos")).Clear();
            driver.FindElement(By.Id("Apellidos")).SendKeys("de prueba");
            driver.FindElement(By.XPath("//button[contains(normalize-space(.),'Guardar cambios')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.AreEqual("Usuario", driver.FindElement(By.Id("Nombre")).GetAttribute("value"));
            Assert.AreEqual("de prueba", driver.FindElement(By.Id("Apellidos")).GetAttribute("value"));
        }

        [TestMethod]
        public void The12MenuEntraEnProyectosTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//a[contains(normalize-space(.),'Seleccionar proyecto')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("Nombre")));
            Assert.IsTrue(IsElementPresent(By.Id("Siglas")));
            Assert.IsTrue(IsElementPresent(By.Id("Descripcion")));
            Assert.IsTrue(IsElementPresent(By.XPath("//button[contains(normalize-space(.),'Añadir proyecto')]")));
        }

        [TestMethod]
        public void The13ProyectosRellenaFormularioTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//a[contains(normalize-space(.),'Seleccionar proyecto')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("Nombre")).Clear();
            driver.FindElement(By.Id("Nombre")).SendKeys("Proyecto Katalon");
            driver.FindElement(By.Id("Siglas")).Clear();
            driver.FindElement(By.Id("Siglas")).SendKeys("PK");
            driver.FindElement(By.Id("Descripcion")).Clear();
            driver.FindElement(By.Id("Descripcion")).SendKeys("Proyecto creado durante las pruebas de interfaz.");
            Assert.AreEqual("Proyecto Katalon", driver.FindElement(By.Id("Nombre")).GetAttribute("value"));
            Assert.AreEqual("PK", driver.FindElement(By.Id("Siglas")).GetAttribute("value"));
            Assert.AreEqual("Proyecto creado durante las pruebas de interfaz.", driver.FindElement(By.Id("Descripcion")).GetAttribute("value"));
        }

        [TestMethod]
        public void The14ProyectosCreaProyectoTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//a[contains(normalize-space(.),'Seleccionar proyecto')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Nombre"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("Nombre")).Clear();
            driver.FindElement(By.Id("Nombre")).SendKeys("Proyecto Katalon");
            driver.FindElement(By.Id("Siglas")).Clear();
            driver.FindElement(By.Id("Siglas")).SendKeys("PK");
            driver.FindElement(By.Id("Descripcion")).Clear();
            driver.FindElement(By.Id("Descripcion")).SendKeys("Proyecto de prueba creado desde Katalon Recorder.");
            driver.FindElement(By.XPath("//button[contains(normalize-space(.),'Añadir proyecto')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.XPath("//*[contains(normalize-space(.),'Proyecto Katalon')]"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.XPath("//*[contains(normalize-space(.),'Proyecto Katalon')]")));
        }

        [TestMethod]
        public void The15PrincipalSinSesionRedireccionaLoginTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.Navigate().GoToUrl("https://localhost:7056/Principal");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
        }

        [TestMethod]
        public void The16PerfilSinSesionRedireccionaLoginTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.Navigate().GoToUrl("https://localhost:7056/Usuarios/Perfil");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
        }

        [TestMethod]
        public void The17ProyectosSinSesionRedireccionaLoginTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.Navigate().GoToUrl("https://localhost:7056/Proyectos/Seleccionar");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
        }

        [TestMethod]
        public void The18MenuCerrarSesionProtegePrincipalTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtContraseña"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.CssSelector(".dropdown-toggle"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.FindElement(By.CssSelector(".dropdown-toggle")).Click();
            driver.FindElement(By.XPath("//button[contains(normalize-space(.),'Cerrar sesión')]")).Click();
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            driver.Navigate().GoToUrl("https://localhost:7056/Principal");
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("txtUsuario"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
            Assert.IsTrue(IsElementPresent(By.Id("txtUsuario")));
        }


        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
