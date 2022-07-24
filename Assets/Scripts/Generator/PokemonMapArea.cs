using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonMapArea : MonoBehaviour
{
    [SerializeField] List<PokemonBase> pokemons;

    public List<PokemonBase> Pokemons { get => Pokemons; set => Pokemons = value; }
}
