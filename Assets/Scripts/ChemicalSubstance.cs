using UnityEngine;

[CreateAssetMenu(fileName = "NewChemicalSubstance", menuName = "Data/ChemicalSubstance")]
public class ChemicalSubstance : ScriptableObject
{
    public enum Substance
    {
        None,
        Water,
        Copper2Chloride,    // fire -> green flame
        StrontiumChloride,  // fire -> red flame
        SodiumChloride,     // fire -> bright yellow flame
        Sodium,             // water + sodium = boom
    }
    
    public Substance substance;
    public string displayName;

    public Color color;
    
    public float fireFeedFactor; //add or remove power of fire source (water has negative value, gas positive)
    public bool canChangeFlameColor;
    public Color flameColor;

    public float phValue;
}