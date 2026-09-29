using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proyecto_Tesis.Services;

namespace Proyecto_Tesis.Tests;

[TestClass]
public sealed class RiskRuleTests
{
    [TestMethod]
    public void Calcular_IndicadoresBajos_RetornaFalse()
    {
        var result = RiskRule.Calcular(300, 3, 3, 3, 1, "Apoyo");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Calcular_IndicadoresAltos_RetornaTrue()
    {
        var result = RiskRule.Calcular(800, 16, 16, 16, 6, "Reemplazo");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Calcular_UmbralExactoNoSuperado_NoSumaCondicionMayorQue()
    {
        var result = RiskRule.Calcular(500, 10, 10, 10, 4, "Apoyo");
        Assert.IsFalse(result);
    }
}
