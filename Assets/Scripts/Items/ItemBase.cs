using MrAmorphic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] float price;
    [SerializeField] bool isSellable;
    [SerializeField] private int id;
    [SerializeField] private MrAmorphic.PokeApiItem pokeApiItem;


    public bool canUseInBattle;
    public bool canUseOutsideBattle;

    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }

    public virtual bool IsReusable => false;



    public virtual string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public float Price { get => price; set => price = value; }
    public bool IsSellable { get => isSellable; set => isSellable = value; }
    public int Id { get => id; set => id = value; }
    public PokeApiItem PokeApiItem { get => pokeApiItem; set => pokeApiItem = value; }

    public bool CanUseInBattle { get => canUseInBattle; set => canUseInBattle = value; }
    public bool CanUseOutsideBattle { get => canUseOutsideBattle; set => canUseOutsideBattle = value; }

}
