using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField] GameObject evolutionUI;
    [SerializeField] Image pokemonImage;

    [Header("Dialog")]
    [TextArea] [SerializeField] string isEvolving;
    [TextArea] [SerializeField] string evolvedInto;

    [SerializeField] AudioClip evolutionMusic;

    public event Action OnStartEvolution;
    public event Action OnCompleteEvolution;

    public static EvolutionManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator Evolve(Pokemon pokemon, Evolution evolution)
    {
        OnStartEvolution?.Invoke();
        evolutionUI.SetActive(true);

        AudioManager.i.PlayMusic(evolutionMusic);

        pokemonImage.sprite = pokemon.Base.FrontSprite;
        yield return DialogManager.Instance.ShowDialogText($"{pokemon.Base.Name} {isEvolving}");

        var oldPokemon = pokemon.Base;
        pokemon.Evolve(evolution);

        pokemonImage.sprite = pokemon.Base.FrontSprite;
        yield return DialogManager.Instance.ShowDialogText($"{oldPokemon.Name} {evolvedInto} {pokemon.Base.Name}");

        evolutionUI.SetActive(false);
        OnCompleteEvolution?.Invoke();
    }
}
