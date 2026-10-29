using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SeleniumTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CP11LoginCorrecto
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            driver = new FirefoxDriver();
            baseURL = "https://www.google.com/";
        }
        
        [ClassCleanup]
        public static void CleanupClass()
        {
            try
            {
                //driver.Quit();// quit does not close the window
                driver.Close();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        
        [TestInitialize]
        public void InitializeTest()
        {
            verificationErrors = new StringBuilder();
        }
        
        [TestCleanup]
        public void CleanupTest()
        {
            Assert.AreEqual("", verificationErrors.ToString());
        }
        
        [TestMethod]
        public void TheCP11LoginCorrectoTest()
        {
            driver.Navigate().GoToUrl("https://localhost:7056/Login");
            driver.FindElement(By.Id("txtUsuario")).Click();
            driver.FindElement(By.Id("txtUsuario")).Clear();
            driver.FindElement(By.Id("txtUsuario")).SendKeys("usuario@test.com");
            driver.FindElement(By.Id("btnSiguiente")).Click();
            driver.FindElement(By.Id("txtContraseña")).Click();
            driver.FindElement(By.Id("txtContraseña")).Clear();
            driver.FindElement(By.Id("txtContraseña")).SendKeys("Clave123!");
            driver.FindElement(By.Id("btnAceptar")).Click();
            driver.Navigate().GoToUrl("https://localhost:7056/Principal");
            Assert.AreEqual("Zona de trabajo", driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Cerrar sesión'])[1]/following::h1[1]")).Text);
            Assert.AreEqual("Página principal", driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)=']'])[1]/following::div[1]")).Text);
            for (int second = 0;; second++) {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.XPath("//button[@type='button']"))) break;
                }
                catch (Exception)
                {}
                Thread.Sleep(1000);
            }
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
        
        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
        
        private string CloseAlertAndGetItsText() {
            try {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert) {
                    alert.Accept();
                } else {
                    alert.Dismiss();
                }
                return alertText;
            } finally {
                acceptNextAlert = true;
            }
        }
    }
}
