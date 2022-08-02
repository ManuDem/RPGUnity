using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfableWater : MonoBehaviour, Interactable, IPlayerTriggerable
{
    [Header("Strings")]
    [TextArea][SerializeField] string theWaterIsDeepBlue;
    [TextArea][SerializeField] string surf;
    [TextArea][SerializeField] string should;
    [TextArea][SerializeField] string useSurf;
    [TextArea][SerializeField] string yes;
    [TextArea][SerializeField] string no;
    [TextArea][SerializeField] string usedSurf;

    bool isJumpingToWater = false;

    public bool TriggerRepeatedly => true;

    public IEnumerator Interact(Transform initiator)
    {
        var animator = initiator.GetComponent<CharacterAnimator>();
        if (animator.IsSurfing || isJumpingToWater)
            yield break;

        yield return DialogManager.Instance.ShowDialogText($"{theWaterIsDeepBlue}");

        var pokemonWithSurf = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == $"{surf}"));

        if (pokemonWithSurf != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText($"{should} { pokemonWithSurf.Base.Name} {useSurf}",
                choices: new List<string>() { $"{yes}", $"{no}" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                // Yes
                yield return DialogManager.Instance.ShowDialogText($"{pokemonWithSurf.Base.Name} {usedSurf}");
                
                var dir = new Vector3(animator.MoveX, animator.MoveY);
                var targetPos = initiator.position + dir;

                isJumpingToWater = true;
                yield return initiator.DOJump(targetPos, 0.3f, 1, 0.5f).WaitForCompletion();
                isJumpingToWater = false;

                animator.IsSurfing = true;
            }
        }
    }

    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            GameController.Instance.StartBattle(BattleTrigger.Water);
        }
    }
}
