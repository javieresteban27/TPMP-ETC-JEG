using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nucleo.Seguridad;

namespace NucleoTest.Seguridad;

[TestClass]
public class ServicioContrasenasTest
{
    [TestMethod]
    public void EsValida_Null_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida(null!), "Null debe considerarse inválido.");
    }

    [TestMethod]
    public void EsValida_WhitespaceOnly_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("   "), "Solo espacios en blanco debe ser inválido.");
    }

    [TestMethod]
    public void EsValida_TooShort_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("Aa1@12"), "Contraseña con menos de 8 caracteres debe ser inválida.");
    }

    [TestMethod]
    public void EsValida_MissingUppercase_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("1234@aeiou"), "Falta mayúscula -> inválida.");
    }

    [TestMethod]
    public void EsValida_MissingLowercase_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("1234@AEIOU"), "Falta minúscula -> inválida.");
    }

    [TestMethod]
    public void EsValida_MissingDigit_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("@Aeiou@Aeiou"), "Falta dígito -> inválida.");
    }

    [TestMethod]
    public void EsValida_MissingSpecialChar_ReturnsFalse()
    {
        Assert.IsFalse(ServicioContrasenas.EsValida("1234AEiou"), "Falta carácter especial -> inválida.");
    }

    [TestMethod]
    public void EsValida_Valid_MinLength8_ReturnsTrue()
    {
        Assert.IsTrue(ServicioContrasenas.EsValida("A1a@5678"), "Contraseña que cumple todos los requisitos debe ser válida.");
    }

    [TestMethod]
    public void EsValida_Valid_Example_ReturnsTrue()
    {
        Assert.IsTrue(ServicioContrasenas.EsValida("1234@AEiou"), "Ejemplo válido debe ser aceptado.");
    }
}