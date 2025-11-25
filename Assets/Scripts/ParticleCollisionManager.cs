using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleCollisionManager : MonoBehaviour
{
    public static ParticleCollisionManager Instance;
    public List<Collider> particleTriggers;
    
    private void Awake()
    {
        Instance = this;
        particleTriggers = new List<Collider>();
    }
}

public abstract class ParticleCollisionListener : MonoBehaviour
{
    public virtual void Start()
    {
        var pcm = ParticleCollisionManager.Instance;
        if (!pcm) return;
        if (GetComponents<Collider>().Any(c => c.isTrigger))
        {
            pcm.particleTriggers.Add(GetComponents<Collider>().First(c => c.isTrigger));
        }
    }
    
    public abstract void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances);
}
