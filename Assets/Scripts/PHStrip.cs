using System;
using UnityEngine;

public class PHStrip : MonoBehaviour
{
    private MeshRenderer renderer;
    
    private Color targetColor;
    private bool isTargetColorSet = false;
    
    private void Start()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        renderer.material = new Material(renderer.sharedMaterial);
        targetColor = renderer.material.color;
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
