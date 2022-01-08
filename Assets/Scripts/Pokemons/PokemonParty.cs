using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public event Action OnUpdated;

    public List<Pokemon> Pokemons {
        get {
            return pokemons;
        }
        set {
            pokemons = value;
            OnUpdated?.Invoke();
        }
    }

    private void Awake()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    private void Start()
    {
        
    }

    public Pokemon GetHealthyPokemon(List<Pokemon> dontInclude=null)
    {
        var healthyPokemons = pokemons.Where(x => x.HP > 0);
        if (dontInclude != null)
            healthyPokemons = healthyPokemons.Where(p => !dontInclude.Contains(p));

        return healthyPokemons.FirstOrDefault();
    }

    public List<Pokemon> GetHealthyPokemons(int unitCount)
    {
        return pokemons.Where(x => x.HP > 0).Take(unitCount).ToList();
    }

    public void AddPokemon(Pokemon newPokemon)
    {
        if (pokemons.Count < 6)
        {
            pokemons.Add(newPokemon);
            OnUpdated?.Invoke();
        }
        else
        {
            // TODO: Add to the PC once that's implemented
        }
    }

    public IEnumerator CheckForEvolutions()
    {
        foreach (var pokemon in pokemons)
        {
            var evoution = pokemon.CheckForEvolution();
            if (evoution != null)
            {
                yield return EvolutionManager.i.Evolve(pokemon, evoution);
            }
        }

        OnUpdated?.Invoke();
    }

    public static PokemonParty GetPlayerParty()
    {
        return FindObjectOfType<PlayerController>().GetComponent<PokemonParty>();
    }
}
