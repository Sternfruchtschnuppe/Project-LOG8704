using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Bottle : ParticleCollisionListener
{
    public List<ChemicalSubstance> chemicalSubstances = new List<ChemicalSubstance>();
    public AnimationCurve outflowThresholdCurve;
    public float fill = 1f;
    public float drainSpeed = 1f;
    
    public ParticleSystem liquidParticles;
    public float totalParticles = 500;

    private ParticleSystem.EmissionModule _emissionModule;
    private LiquidAnimator _liquidAnimator;
    public Collider trigger;
    
    private float _lastFillEmission = 1f;
    
    
    
    public override void Start()
    {
        base.Start();
        _liquidAnimator = GetComponentInChildren<LiquidAnimator>();
        _emissionModule = liquidParticles.emission;
        trigger = GetComponents<Collider>().First(c => c.isTrigger);
    }
    
    void Update()
    {
        var tilt = Vector3.Dot(transform.up, Vector3.up);
        tilt = Mathf.Clamp01(Mathf.Acos(tilt) / Mathf.PI);
        
        var threshold = outflowThresholdCurve.Evaluate(tilt);
        if (fill > threshold)
        {
            _emissionModule.enabled = true;
            
            var deltaFill = _lastFillEmission;
            fill = Mathf.Lerp(fill, threshold, Time.deltaTime * drainSpeed * 0.5f);
            fill = Mathf.MoveTowards(fill, threshold, Time.deltaTime * drainSpeed * 0.5f);
            deltaFill -= fill;
            var particles = (int)(deltaFill * totalParticles);
            if (particles > 0)
            {
                liquidParticles.Emit(particles);
                _lastFillEmission = fill;
            }
        }
        else
        {
            _emissionModule.enabled = false;        
        }

        if (_liquidAnimator)
        {
            _liquidAnimator.fillAmount = fill;
            _liquidAnimator.gameObject.SetActive(fill >= 0);
        }

        if (fill > _lastFillEmission) _lastFillEmission = fill;
    }
    
    public override void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances)
    {
        this.chemicalSubstances.AddRange(chemicalSubstances);
        this.chemicalSubstances = this.chemicalSubstances.Distinct().ToList();
        fill = Mathf.Clamp01(fill + 1f / totalParticles);
    }
}