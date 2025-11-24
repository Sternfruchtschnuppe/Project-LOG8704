using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Bottle : ParticleCollisionListener
{
    public List<ChemicalSubstance> chemicalSubstances = new List<ChemicalSubstance>();
    public AnimationCurve outflowThresholdCurve;
    public float fillAmount = 1f;
    public Color color;
    public float drainSpeed = 1f;
    
    public ParticleSystem liquidParticles;
    public float totalParticles = 500;

    private ParticleSystem.EmissionModule _emissionModule;
    private LiquidAnimator _liquidAnimator;
    public Collider trigger;
    
    private float _lastFillEmission = 1f;

    private bool exploding = false;
    
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
        if (fillAmount > threshold)
        {
            _emissionModule.enabled = true;
            
            var deltaFill = _lastFillEmission;
            fillAmount = Mathf.Lerp(fillAmount, threshold, Time.deltaTime * drainSpeed * 0.5f);
            fillAmount = Mathf.MoveTowards(fillAmount, threshold, Time.deltaTime * drainSpeed * 0.5f);
            deltaFill -= fillAmount;
            var particles = (int)(deltaFill * totalParticles);
            if (particles > 0)
            {
                liquidParticles.Emit(particles);
                _lastFillEmission = fillAmount;
            }
        }
        else
        {
            _emissionModule.enabled = false;        
        }

        if (_liquidAnimator)
        {
            _liquidAnimator.gameObject.SetActive(fillAmount >= 0);
        }

        if (fillAmount > _lastFillEmission) _lastFillEmission = fillAmount;
    }
    
    public override void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances)
    {
        this.chemicalSubstances.AddRange(chemicalSubstances);
        this.chemicalSubstances = this.chemicalSubstances.Distinct().ToList();
        fillAmount = Mathf.Clamp01(fillAmount + 1f / totalParticles);
        
        var main = liquidParticles.main;

        var targetColor = GetTargetColor();

        color = Color.Lerp(color, targetColor, Time.deltaTime * 5.0f);
        
        var grad = main.startColor; 
        grad.color = color;
        main.startColor = grad;


        TryReact();
    }

    public Color GetTargetColor()
    {
        var colorVec = Vector3.zero;
        foreach (var chemicalSubstance in this.chemicalSubstances)
        {
            colorVec += new Vector3(chemicalSubstance.color.r, chemicalSubstance.color.g, chemicalSubstance.color.b);
        }
        colorVec /= chemicalSubstances.Count;
        return new Color(colorVec.x,  colorVec.y, colorVec.z);
    }

    private void TryReact()
    {
        if (chemicalSubstances.Any(c => c.substance == ChemicalSubstance.Substance.Water) &&
            chemicalSubstances.Any(c => c.substance == ChemicalSubstance.Substance.Sodium))
        {
            if (!exploding)
            {
                StartCoroutine(Explode());
            }
        }
    }

    private IEnumerator Explode()
    {
        exploding = true;
        yield return new WaitForSeconds(3f);
        var particles = Instantiate(ChemicalReactionManager.Instance.explosionParticles, transform.position, Quaternion.identity);
        Destroy(particles, 10f);
        fillAmount = 0;
        _lastFillEmission = 0;
        chemicalSubstances.Clear();
        exploding = false;
    }
}