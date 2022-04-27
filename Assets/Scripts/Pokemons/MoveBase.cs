using MrAmorphic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] bool alwaysHits;
    [SerializeField] int pp;
    [SerializeField] int priority;

    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;

    [SerializeField] private int id;
    [SerializeField] private MrAmorphic.PokeApiMove pokeApiMove;

    [SerializeField] AudioClip sound;

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public PokemonType Type { get => type; set => type = value; }
    public int Power { get => power; set => power = value; }
    public int Accuracy { get => accuracy; set => accuracy = value; }
    public bool AlwaysHits { get => alwaysHits; set => alwaysHits = value; }
    public int PP { get => pp; set => pp = value; }
    public int Priority { get => priority; set => priority = value; }
    public MoveCategory Category { get => category; set => category = value; }
    public MoveEffects Effects { get => effects; set => effects = value; }
    public List<SecondaryEffects> Secondaries { get => secondaries; set => secondaries = value; }
    public MoveTarget Target { get => target; set => target = value; }
    public int Id { get => id; set => id = value; }
    public PokeApiMove PokeApiMove { get => pokeApiMove; set => pokeApiMove = value; }

    public AudioClip Sound => sound;
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;

    public List<StatBoost> Boosts { get => boosts; set => boosts = value; }
    public ConditionID Status { get => status; set => status = value; }
    public ConditionID VolatileStatus { get => volatileStatus; set => volatileStatus = value; }
}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;
    public int Chance { get => chance; set => chance = value; }
    public MoveTarget Target { get => target; set => target = value; }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategory
{
    Physical, Special, Status
}

public enum MoveTarget
{
    Foe, Self
}
