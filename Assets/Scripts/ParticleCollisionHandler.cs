using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    private Bottle _bottle;
    private ParticleSystem _particleSystem;

    protected virtual void Start()
    {
        _bottle = GetComponentInParent<Bottle>();
        _particleSystem = GetComponent<ParticleSystem>();
        var trigger = _particleSystem.trigger;
        var i = 0;
        foreach (var col in ParticleCollisionManager.Instance.particleTriggers)
        {
            trigger.SetCollider(i++, col);
        }
    }

    void OnParticleTrigger()
    {
        var bottle = _particleSystem.transform.parent.GetComponent<Bottle>();
        if (!bottle) return;

        var entered = new List<ParticleSystem.Particle>();
        _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, entered, out var data);
        
        for (int i = 0; i < entered.Count; i++)
        {
            int colliderCount = data.GetColliderCount(i);
            if (colliderCount == 0)
                continue;

            for (int c = 0; c < colliderCount; c++)
            {
                var col = data.GetCollider(i, c);
                if (col && col.TryGetComponent<ParticleCollisionListener>(out var listener))
                {
                    listener.OnHitBySubstance(bottle.chemicalSubstance);
                }
            }
        }

    }

}