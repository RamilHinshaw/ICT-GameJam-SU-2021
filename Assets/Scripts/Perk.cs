using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[System.Serializable]
public class Perk 
{
    public string name;
    [TextArea] public string description;
    public PerkTypes perkType;
    public bool removeOnSelection = true;
}
