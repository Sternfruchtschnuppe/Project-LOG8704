using System;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalReactionManager : MonoBehaviour
{
    public static ChemicalReactionManager Instance;
    private List<ChemicalReaction> chemicalReactions = new  List<ChemicalReaction>();

    public ParticleSystem explosionParticles;
    
    private void Awake()
    {
        Instance = this;
    }
}