﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonGiver : MonoBehaviour, ISavable
{
    [SerializeField] Pokemon pokemonToGive;
    [SerializeField] Dialog dialog;

    [Header("Strings")]
    [TextArea] [SerializeField] string received;

    bool used = false;

    public IEnumerator GivePokemon(PlayerController player, Sprite sprite, string nameText)
    {
        yield return DialogManager.Instance.ShowDialog(dialog, sprite, nameText);

        pokemonToGive.Init();
        player.GetComponent<PokemonParty>().AddPokemon(pokemonToGive);

        used = true;

        //AudioManager.i.PlaySfx(AudioId.PokemonObtained, pauseMusic: true);


        string dialogText = $"{player.Name} {received} {pokemonToGive.Base.Name}.";

        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return pokemonToGive != null && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
