using System.Collections.Generic;

public class ChemicalReaction
{
    public List<string> requiredSubstances;
    public float requiredTemperature;
    public bool requiresStirring;
    public ChemicalSubstance result;


    public bool CanReact(List<ChemicalSubstance> contents, float temp, bool stirred)
    {
        return false;
        // foreach (var r in requiredSubstances)
        //     if (!contents.Exists(c => c.substanceName == r)) return false;
        // if (temp < requiredTemperature) return false;
        // if (requiresStirring && !stirred) return false;
        // return true;
    }
}