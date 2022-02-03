using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new pokeball")]
public class PokeballItem : ItemBase
{
    [SerializeField] float catchRateModfier = 1;

    public override bool Use(Pokemon pokemon)
    {
        return true;
    }

    public float CatchRateModifier => catchRateModfier;
}
