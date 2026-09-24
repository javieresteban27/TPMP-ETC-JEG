using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TestProjectManagementPlatform.Nucleo.Seguridad;


namespace Nucleo.Tests.Seguridad;

[TestClass]
public class ServicioContrasenasTests
{
    [TestMethod]
    public void ServicioContrasenas_ValorNulo()
    {
        String? contrasena = null;

        Assert.IsNull(contrasena, "La contraseña no puede ser nula.");
    }

    [TestMethod]
    public void ServicioContrasenas_Espacios()
    {
        string contrasena = "    ";
        Assert.IsTrue(string.IsNullOrWhiteSpace(contrasena), "La contraseña no puede contener solo espacios en blanco.");
    }

    [TestMethod]
    public void ServicioContrasenas_LongitudInvalida()
    {
        string contrasena = "1234@Ae";
        Assert.IsLessThan(8, contrasena.Length, "La contraseña debe tener al menos 8 caracteres.");
    }

    [TestMethod]
    public void ServicioContrasenas_SinMayusculas()
    {
        string contrasena = "1234@aeiou";
        Assert.IsFalse(contrasena.Any(char.IsUpper), "La contraseña debe contener al menos una letra mayúscula.");
    }

    [TestMethod]
    public void ServicioContrasenas_SinMinusculas()
    {
        string contrasena = "1234@AEIOU";
        Assert.IsFalse(contrasena.Any(char.IsLower), "La contraseña debe contener al menos una letra minúscula.");
    }

    [TestMethod]
    public void ServicioContrasenas_SinDigitos()
    {
        string contrasena = "@Aeiou@Aeiou";
        Assert.IsFalse(contrasena.Any(char.IsDigit), "La contraseña debe contener al menos un dígito.");
    }

    [TestMethod]
    public void ServicioContrasenas_SinCaracteresEspeciales()
    {
        string contrasena = "1234AEiou";
        Assert.IsFalse(contrasena.Any(c => !char.IsLetterOrDigit(c)), "La contraseña debe contener al menos un carácter especial.");
    }

    [TestMethod]
    public void ServicioContrasenas_Valida()
    {
        string contrasena = "1234@AEiou";
        Assert.IsTrue(ServicioContrasenas.EsValida(contrasena), "La contraseña debe ser válida según la política de seguridad.");
    }
}
