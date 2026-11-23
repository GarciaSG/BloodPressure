namespace BloodPressureApp.API.Helpers;

// Clasificación orientativa para adultos (referencia: guías AHA/ACC 2025).
// Es solo una referencia para una lectura individual y NO es un diagnóstico médico.
public static class BloodPressureHelper
{
    public const string Normal = "Normal";
    public const string Elevated = "Elevada";
    public const string HypertensionStage1 = "Hipertensión etapa 1";
    public const string HypertensionStage2 = "Hipertensión etapa 2";

    public static string GetClassification(int systolic, int diastolic)
    {
        // Se revisa de la categoría más grave a la más leve.
        // Así, si sistólica y diastólica caen en categorías distintas,
        // se asigna automáticamente la de mayor severidad.
        if (systolic >= 140 || diastolic >= 90)
        {
            return HypertensionStage2;
        }

        if (systolic >= 130 || diastolic >= 80)
        {
            return HypertensionStage1;
        }

        if (systolic >= 120)
        {
            return Elevated;
        }

        return Normal;
    }
}
