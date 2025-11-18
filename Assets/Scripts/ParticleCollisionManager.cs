using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionManager : MonoBehaviour
{
    public static ParticleCollisionManager Instance;
    public List<Collider> particleTriggers = new  List<Collider>();
    
    private void Awake()
    {
        Instance = this;
    }
}

public abstract class ParticleCollisionListener : MonoBehaviour
{
    public virtual void Start()
    {
        ParticleCollisionManager.Instance.particleTriggers.Add(GetComponent<Collider>());
    }
    
    public abstract void OnHitBySubstance(ChemicalSubstance chemicalSubstance);
}
