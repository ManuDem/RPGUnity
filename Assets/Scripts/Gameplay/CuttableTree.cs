using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, Interactable
{

    [Header("Strings")]
    [TextArea][SerializeField] string thisTreeLooksLikeItCanBeCut;
    [TextArea][SerializeField] string cut;
    [TextArea][SerializeField] string should;
    [TextArea][SerializeField] string useCut;
    [TextArea][SerializeField] string yes;
    [TextArea][SerializeField] string no;
    [TextArea][SerializeField] string usedCut;

    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.Instance.ShowDialogText($"{thisTreeLooksLikeItCanBeCut}");

        var pokemonWithCut = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == $"{cut}"));

        if (pokemonWithCut != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText($"{thisTreeLooksLikeItCanBeCut} {pokemonWithCut.Base.Name} {useCut}", 
                choices: new List<string>() { $"{yes}", $"{no}" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                // Yes
                yield return DialogManager.Instance.ShowDialogText($"{pokemonWithCut.Base.Name} {usedCut}");
                gameObject.SetActive(false);
            }
        }
    }
}
