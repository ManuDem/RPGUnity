using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [Header("Dialog")]
    [TextArea] [SerializeField] string healed;
    [TextArea] [SerializeField] string notHealed;
    [TextArea] [SerializeField] string yes;
    [TextArea] [SerializeField] string no;

    public IEnumerator Heal(Transform player, Dialog dialog)
    {
        int selectedChoice = 0;

        yield return DialogManager.Instance.ShowDialog(dialog, 
            new List<string>() { $"{yes}", $"{no}" }, 
            (choiceIndex) => selectedChoice = choiceIndex );

        if (selectedChoice == 0)
        {
            // Yes
            yield return Fader.i.FadeIn(0.5f);

            var playerParty = player.GetComponent<PokemonParty>();
            playerParty.Pokemons.ForEach(p => p.Heal());
            playerParty.PartyUpdated();

            yield return Fader.i.FadeOut(0.5f);

            yield return DialogManager.Instance.ShowDialogText(healed);
        }
        else if (selectedChoice == 1)
        {
            // No
            yield return DialogManager.Instance.ShowDialogText(notHealed);
        }

        
    }
}
