using UnityEngine;

public class Container : ParticleCollisionListener
{
    public override void OnHitBySubstance(ChemicalSubstance chemicalSubstance)
    {
        // Debug.Log(chemicalSubstance);
    }
}
