using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfterBattle;
    [SerializeField] Dialog dialogYourPokemonAreDefeated;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    [SerializeField] AudioClip trainerAppearsClip;
    [SerializeField] AudioClip trainerBattleMusic;

    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToCompleteWhenDefeat;

    // State
    bool battleLost = false;

    Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    private void Update()
    {
            character.HandleUpdate();
    }

    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!battleLost)
        {

            var playerParty = initiator.GetComponent<PokemonParty>();

            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon == null)
            {
                Debug.Log("Defeated");
                yield return DialogManager.Instance.ShowDialogSprite(dialogYourPokemonAreDefeated, sprite, name);
            }
            else
            {
                AudioManager.i.PlayMusic(trainerAppearsClip);

                yield return DialogManager.Instance.ShowDialogSprite(dialog, sprite, name);

                GameController.Instance.StartTrainerBattle(this);
            }

        }
        else
        {
            yield return DialogManager.Instance.ShowDialogSprite(dialogAfterBattle, sprite, name);
        }
        
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {

        var playerParty = player.GetComponent<PokemonParty>();

        var nextPokemon = playerParty.GetHealthyPokemon();
        if (nextPokemon == null)
        {
            Debug.Log("Defeated");

        }
        else
        {
            AudioManager.i.PlayMusic(trainerAppearsClip);

            // Show Exclamation
            exclamation.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            exclamation.SetActive(false);

            // Walk towards the player
            var diff = player.transform.position - transform.position;
            var moveVec = diff - diff.normalized;
            moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

            yield return character.Move(moveVec);

            // Show dialog
            yield return DialogManager.Instance.ShowDialogSprite(dialog, sprite, name);
            GameController.Instance.StartTrainerBattle(this);
        }
    }

    public IEnumerator BattleLost(Transform initiator)
    {

        battleLost = true;
        fov.gameObject.SetActive(false);

        if (questToCompleteWhenDefeat != null)
        {
            var quest = new Quest(questToCompleteWhenDefeat, sprite, name);
            yield return quest.CompleteQuest(initiator);
            questToCompleteWhenDefeat = null;

            Debug.Log($"{quest.Base.Name} completed");
        }
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        battleLost = (bool)state;

        if (battleLost)
            fov.gameObject.SetActive(false);
    }

    public string Name {
        get => name;
    }

    public Sprite Sprite {
        get => sprite;
    }
    public AudioClip TrainerBattleMusic { get => trainerBattleMusic; set => trainerBattleMusic = value; }
}
