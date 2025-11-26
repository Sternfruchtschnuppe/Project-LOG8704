using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PHStrip : ParticleCollisionListener
{
    private MeshRenderer renderer;
    
    private Color targetColor;
    private bool isTargetColorSet = false;
    
    public override void Start()
    {
        base.Start();
        renderer = GetComponentInChildren<MeshRenderer>();
        renderer.material = new Material(renderer.sharedMaterial);
        targetColor = renderer.material.color;
    }

    public override void OnHitBySubstance(List<ChemicalSubstance> chemicalSubstances)
    {
        SetTargetColor(PhColor.Get(chemicalSubstances.Average(s => s.phValue)));
    }

    public void Update()
    {
        renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime * 5);
    }

    public void SetTargetColor(Color color)
    {
        if(isTargetColorSet) return;
        this.targetColor = color;
        isTargetColorSet = true;
    }
}
