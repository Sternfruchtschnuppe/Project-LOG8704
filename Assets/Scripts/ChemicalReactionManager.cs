using System;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalReactionManager : MonoBehaviour
{
    public static ChemicalReactionManager Instance;
    private List<ChemicalReaction> chemicalReactions = new  List<ChemicalReaction>();
    
    private void Awake()
    {
        Instance = this;
    }

    // public bool TryReact(List<ChemicalSubstance> chemicalSubstanceList, bool fire=false)
    // {
    //     
    // }
    
}