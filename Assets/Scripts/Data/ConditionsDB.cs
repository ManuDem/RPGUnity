using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    static string poison = "Veleno";
    static string hasBeenPoisoned = "è stato avvelenato.";
    static string hurtItselfDueToPoison = "soffre a causa del veleno.";

    static string burn = "Bruciatura";
    static string hasBeenBurned = "è stato scottato.";
    static string hurtItselfDueToBurn = "soffre a causa della scottatura.";

    static string paralyzed = "Paralisi";
    static string hasBeenParalyzed = "è stato paralizzato.";
    static string parylyzedAndCantMove = "è paralizzato e non può attaccare.";

    static string freeze = "Gelo";
    static string hasBeenFrozen = "è stato congelato.";
    static string isNotFrozenAnymore = "non è più congelato.";

    static string sleep = "Sonno";
    static string hasFallenAsleep = "si è stato addormentato.";
    static string wokeUp = "si è svegliato!";
    static string isSleeping = "sta dormendo.";

    static string Confusion = "Confusione";
    static string hasBeenConfused = "è confuso.";
    static string kickedOutOfConfusion = "non è più confuso.";
    static string isConfused = "è confuso.";
    static string itHurtItselfDueToConfusion = "Si colpisce da solo a causa della confusione.";




    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.poison,
            new Condition()
            {
                Name = $"{poison}",
                StartMessage = $"{hasBeenPoisoned}",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {hurtItselfDueToPoison}");
                }
            }
        },
        {
            ConditionID.burn,
            new Condition()
            {
                Name = $"{burn}",
                StartMessage = $"{hasBeenBurned}",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {hurtItselfDueToBurn}");
                }
            }
        },
        {
            ConditionID.paralysis,
            new Condition()
            {
                Name = $"{paralyzed}",
                StartMessage = $"{hasBeenParalyzed}",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if  (Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {parylyzedAndCantMove}");
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            ConditionID.freeze,
            new Condition()
            {
                Name = $"{freeze}",
                StartMessage = $"{hasBeenFrozen}",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if  (Random.Range(1, 5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {isNotFrozenAnymore}");
                        return true;
                    }

                    return false;
                }
            }
        },
        {
            ConditionID.sleep,
            new Condition()
            {
                Name = $"{sleep}",
                StartMessage = $"{hasFallenAsleep}",
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1, 4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {wokeUp}");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {isSleeping}");
                    return false;
                }
            }
        },

        // Volatile Status Conditions
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = $"{Confusion}",
                StartMessage = $"{hasBeenConfused}",
                OnStart = (Pokemon pokemon) =>
                {
                    // Confused for 1 - 4 turns
                    pokemon.VolatileStatusTime = Random.Range(1, 5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {kickedOutOfConfusion}");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    // 50% chance to do a move
                    if (Random.Range(1, 3) == 1)
                        return true;

                    // Hurt by confusion
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} {isConfused}");
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{itHurtItselfDueToConfusion}");
                    return false;
                }
            }
        }
    };

    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if (condition.Id == ConditionID.sleep || condition.Id == ConditionID.freeze)
            return 2f;
        else if (condition.Id == ConditionID.paralysis || condition.Id == ConditionID.poison || condition.Id == ConditionID.burn)
            return 1.5f;

        return 1f;
    }
}

public enum ConditionID
{
    none, poison, burn, sleep, paralysis, freeze,
    confusion, yawn, trap, disable, leech_seed, unknown, nightmare, no_type_immunity, perish_song,
    infatuation, torment, ingrain, embargo, heal_block, silence, tar_shot
}
