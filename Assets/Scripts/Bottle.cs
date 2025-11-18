using System;
using UnityEngine;
using UnityEngine.Playables;

public class Bottle : MonoBehaviour
{
    public ChemicalSubstance chemicalSubstance;
    public AnimationCurve outflowThresholdCurve;
    public float fill = 1f;
    public float drainSpeed = 1f;
    
    public ParticleSystem liquidParticles;
    public float totalParticles = 500;

    private ParticleSystem.EmissionModule _emissionModule;
    private LiquidAnimator _liquidAnimator;

    private float _lastFillEmission = 1f;
    private void Start()
    {
        _liquidAnimator = GetComponentInChildren<LiquidAnimator>();
        _emissionModule = liquidParticles.emission;
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
}