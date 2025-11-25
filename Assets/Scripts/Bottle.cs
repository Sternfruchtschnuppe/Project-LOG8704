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

    private LiquidAnimator _liquidAnimator;
    public Collider trigger;
    
    private float _lastFillEmission = 1f;

    private bool exploding = false;
    
    public override void Start()
    {
        base.Start();
        _liquidAnimator = GetComponentInChildren<LiquidAnimator>();
        if (GetComponents<Collider>().Any(c => c.isTrigger)) trigger = GetComponents<Collider>().First(c => c.isTrigger);
        if (chemicalSubstances.Count == 0) fillAmount = 0;
        _lastFillEmission =  fillAmount;
    }

    void Update()
    {
        var tilt = Vector3.Dot(transform.up, Vector3.up);
        tilt = Mathf.Clamp01(Mathf.Acos(tilt) / Mathf.PI);
        
        var threshold = outflowThresholdCurve.Evaluate(tilt);
        var emission = liquidParticles.emission;
        emission.enabled = true;
        
        if (fillAmount > threshold)
        {
            
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
            emission.enabled = false;        
        }
        
        if (_liquidAnimator)
        {
            _liquidAnimator.gameObject.SetActive(fillAmount >= 0);
        }
        
        UpdateParticleColor();
        if (fillAmount > _lastFillEmission) _lastFillEmission = fillAmount;
    }
    
    public override void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances)
    {
        this.chemicalSubstances.AddRange(chemicalSubstances);
        this.chemicalSubstances = this.chemicalSubstances.Distinct().ToList();
        fillAmount = Mathf.Clamp01(fillAmount + 1f / totalParticles);
        
        var targetColor = GetTargetColor();

        color = Color.Lerp(color, targetColor, Time.deltaTime * 5.0f);

        // UpdateParticleColor();
        TryReact();
    }

    private void UpdateParticleColor()
    {
        var main = liquidParticles.main;
        var grad = main.startColor; 
        grad.color = color;
        main.startColor = grad;
    }

    public Color GetTargetColor()
    {
        if(chemicalSubstances.Count == 0) return Color.black;
        
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

        if (chemicalSubstances.Any(c => c.substance == ChemicalSubstance.Substance.HydrochloricAcid) &&
            chemicalSubstances.Any(c => c.substance == ChemicalSubstance.Substance.SodiumHydroxide))
        {
            chemicalSubstances = chemicalSubstances.Where(c =>
                c.substance != ChemicalSubstance.Substance.HydrochloricAcid &&
                c.substance != ChemicalSubstance.Substance.SodiumHydroxide).Append(ChemicalReactionManager.Instance.water).Distinct().ToList();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PHStrip") && chemicalSubstances.Count != 0)
        {
            float phValue = 0f;
            foreach (var substance in chemicalSubstances)
            {
                phValue += substance.phValue;
            }
            phValue /= chemicalSubstances.Count;
            
            other.gameObject.GetComponent<PHStrip>().SetTargetColor(PHToColor(phValue));
        }
    }
    
    private Color PHToColor(float ph)
    {
        ph = Mathf.Clamp(ph, 0f, 14f);
        var color = new Color(1f, 1f, 1f, 1f);
        if (ph < 7f)
        {
            color =  Color.Lerp(Color.red, Color.green, ph / 7f);
        }
        else
        {
            color = Color.Lerp(Color.green, Color.blue, (ph - 7f) / 7f);
        }

        return color - 0.1f * Color.white;
    }

}