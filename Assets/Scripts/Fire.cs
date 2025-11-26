using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class Fire : ParticleCollisionListener
{
    public Color defaultColor;
    public Color color;
    public float maxHealth;
    public float health;

    public GameObject fireGFX;
    public ParticleSystem[] flameParticles;
    public Light flameLight;

    public float flameColorDuration = 5;
    private float _lastFlameColorTs = float.NegativeInfinity;
    
    public override void Start()
    {
        base.Start();
        SetColor(defaultColor);
    }
    
    private void Update()
    {
        if (_lastFlameColorTs + flameColorDuration < Time.time)
        {
            SetColor(defaultColor);
        }
    }
    
    private void OnValidate()    
    {
        SetColor(color);
        SetHealth(health);
    }

    public void SetColor(Color color)
    {
        this.color = color;
        foreach (var ps in flameParticles)
        {
            var main = ps.main;
            if (main.startLifetime.constant > 100f)
            {
                var col = ps.colorOverLifetime;
                var gradient = col.color;
                gradient.mode = ParticleSystemGradientMode.Color;
                gradient.color = color;
                col.color = gradient;
            }
            else
            {
                main.startColor = color;
            }
        }
        
        flameLight.color = color;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Fire>(out var fire))
        {
            fire.SetHealth(fire.maxHealth);
        }
        
        // Flame Coloring
        if (other.TryGetComponent<SolidMaterial>(out var solidMaterial))
        {
            if (solidMaterial.chemicalSubstance.canChangeFlameColor)
            {
                SetColor(solidMaterial.chemicalSubstance.flameColor);
                _lastFlameColorTs = Time.time;
            }
        }
    }

    public override void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances)
    {
        SetHealth(health + chemicalSubstances.Sum(c => c.fireFeedFactor));
    }

    public void SetHealth(float health)
    {
        this.health = Mathf.Clamp(health, 0f, maxHealth);
        
        fireGFX.SetActive(health > 0);
    }

    
}
