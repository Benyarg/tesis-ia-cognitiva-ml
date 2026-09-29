using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Tesis.Models.DTO;
using Proyecto_Tesis.ViewModels.Auth;

namespace Proyecto_Tesis.Tests;

[TestClass]
public sealed class DtoValidationTests
{
    [TestMethod]
    public void UsoIA_DiasFueraDeRango_EsInvalido()
    {
        var dto = new UsoIADto { HorasUsoDiario = 2, DiasSemana = 8, TipoUso = "Apoyo" };
        Assert.IsFalse(EsValido(dto));
    }

    [TestMethod]
    public void UsoIA_TipoNoPermitido_EsInvalido()
    {
        var dto = new UsoIADto { HorasUsoDiario = 2, DiasSemana = 4, TipoUso = "Otro" };
        Assert.IsFalse(EsValido(dto));
    }

    [TestMethod]
    public void AdminLogin_CorreoInvalido_EsInvalido()
    {
        var model = new AdminLoginViewModel { Email = "admin", Password = "clave" };
        Assert.IsFalse(EsValido(model));
    }

    private static bool EsValido(object model)
    {
        var context = new ValidationContext(model);
        return Validator.TryValidateObject(model, context, new List<ValidationResult>(), validateAllProperties: true);
    }
}
