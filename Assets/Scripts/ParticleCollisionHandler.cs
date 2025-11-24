using System;
using System.Collections;
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
        StartCoroutine(UpdateParticleTriggers());
    }

    private IEnumerator UpdateParticleTriggers()
    {
        while (true)
        {
            var trigger = _particleSystem.trigger;
            var i = 0;
            foreach (var col in ParticleCollisionManager.Instance.particleTriggers)
            {
                if (col == _bottle.trigger) continue;
                trigger.SetCollider(i++, col);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // void OnParticleTrigger()
    // {
    //     var entered = new List<ParticleSystem.Particle>();
    //     _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, entered, out var data);
    //     
    //     for (int i = 0; i < entered.Count; i++)
    //     {
    //         int colliderCount = data.GetColliderCount(i);
    //         if (colliderCount == 0)
    //             continue;
    //
    //         for (int c = 0; c < colliderCount; c++)
    //         {
    //             var col = data.GetCollider(i, c);
    //             if (col && col.gameObject.TryGetComponent<ParticleCollisionListener>(out var listener))
    //             {
    //                 listener.OnHitBySubstance(_bottle.chemicalSubstances);
    //             }
    //         }
    //     }
    // }
    
    void OnParticleTrigger()
    {
        var entered = new List<ParticleSystem.Particle>();
        int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, entered, out var data);

        for (int i = 0; i < numEnter; i++)
        {
            int colliderCount = data.GetColliderCount(i);
            if (colliderCount == 0)
                continue;

            for (int c = 0; c < colliderCount; c++)
            {
                var col = data.GetCollider(i, c);
                if (col && col.gameObject.TryGetComponent<ParticleCollisionListener>(out var listener))
                {
                    listener.OnHitBySubstance(_bottle.chemicalSubstances);
                }
            }
        }
    }


}