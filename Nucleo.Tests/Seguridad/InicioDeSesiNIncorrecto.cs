using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestClass]
    public class InicioDeSesiNIncorrecto
    {
        private static IWebDriver driver;
        
        
        [TestMethod]
        public void TheInicioDeSesiNIncorrectoTest()
        {
            driver.Navigate().GoToUrl("https://ubunet.ubu.es/");
            driver.FindElement(By.Id("floatingInput")).Clear();
            driver.FindElement(By.Id("floatingInput")).SendKeys("jeg1010@alu.ubu.es");
            driver.FindElement(By.Id("floatingPassword")).Clear();
            driver.FindElement(By.Id("floatingPassword")).SendKeys("ContraseñaIncorrecta");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            Assert.AreEqual("PROBLEMA AL INICIAR SESIÓN", driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='UBUNet'])[1]/following::th[1]")).Text);
            driver.Close();
            driver.Dispose();
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
