namespace Proyecto_Tesis.Services;

public static class RiskRule
{
    // Regla heredada del MVP. Los umbrales deben conservar trazabilidad metodológica en la tesis.
    public static bool Calcular(double latencia, int estres, int ansiedad, int depresion, int horasUso, string tipoUso)
    {
        var score = 0;
        if (latencia > 500) score += 1;
        if (latencia > 700) score += 2;
        if (estres > 10) score += 1;
        if (ansiedad > 10) score += 1;
        if (depresion > 10) score += 1;
        if (estres > 15) score += 2;
        if (ansiedad > 15) score += 2;
        if (depresion > 15) score += 2;
        if (horasUso >= 5) score += 1;
        if (tipoUso == "Reemplazo") score += 2;
        return score >= 5;
    }
}
