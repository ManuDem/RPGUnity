using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Menu, MainMenu, PartyScreen, Bag, Cutscene, Paused, Evolution, Shop }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    [Header("Dialog")]
    [SerializeField] Dialog dialog;

    GameState state;
    GameState prevState;
    GameState stateBeforeEvolution;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;

    MainMenuController mainMenuController;

    public static GameController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

        menuController = GetComponent<MenuController>();
        mainMenuController = GetComponent<MainMenuController>();

        PokemonDB.Init();
        MoveDB.Init();
        ConditionsDB.Init();
        ItemDB.Init();
        QuestDB.Init();
    }

    private void Start()
    {
        battleSystem.OnBattleOver += EndBattle;

        partyScreen.Init();

        DialogManager.Instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnDialogFinished += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;

        mainMenuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        mainMenuController.onMenuSelected += OnMainMenuSelected;

        EvolutionManager.i.OnStartEvolution += () =>
        {
            stateBeforeEvolution = state;
            state = GameState.Evolution;
        };
        EvolutionManager.i.OnCompleteEvolution += () =>
        {
            partyScreen.SetPartyData();
            state = stateBeforeEvolution;

            AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
        };

        ShopController.i.OnStart += () => state = GameState.Shop;
        ShopController.i.OnFinish += () => state = GameState.FreeRoam;

        if (SceneManager.GetActiveScene().name == "MainMenu") {
            mainMenuController.OpenMenu();
            state = GameState.MainMenu;
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    public void StartBattle()
    {
        var playerParty = playerController.GetComponent<PokemonParty>();

        var nextPokemon = playerParty.GetHealthyPokemon();
        if (nextPokemon == null)
        {
            Debug.Log("Defeated");
            StartCoroutine(                ShowDialogDefeated());
        }
        else
        {
            state = GameState.Battle;
            battleSystem.gameObject.SetActive(true);
            worldCamera.gameObject.SetActive(false);

            var wildPokemon = CurrentScene.GetComponent<MapArea>().GetRandomWildPokemon();

            var wildPokemonCopy = new Pokemon(wildPokemon.Base, wildPokemon.Level);

            battleSystem.StartBattle(playerParty, wildPokemonCopy);
        }
    }

    TrainerController trainer;
    public void StartTrainerBattle(TrainerController trainer)
    {
        var playerParty = playerController.GetComponent<PokemonParty>();

        var nextPokemon = playerParty.GetHealthyPokemon();
        if (nextPokemon == null)
        {
            Debug.Log("Defeated");
            StartCoroutine(ShowDialogDefeated());
        }
        else
        {
            state = GameState.Battle;
            battleSystem.gameObject.SetActive(true);
            worldCamera.gameObject.SetActive(false);

            this.trainer = trainer;
            var trainerParty = trainer.GetComponent<PokemonParty>();

            battleSystem.StartTrainerBattle(playerParty, trainerParty);
        }
            }

    public IEnumerator ShowDialogDefeated()
    {
        yield return DialogManager.Instance.ShowDialogSprite(dialog, null, null);
    }

    public void OnEnterTrainersView(TrainerController trainer)
    {
        var playerParty = playerController.GetComponent<PokemonParty>();

        var nextPokemon = playerParty.GetHealthyPokemon();
        if (nextPokemon == null)
        {
            Debug.Log("Defeated");
            StartCoroutine(ShowDialogDefeated());
        }
        else
        {
            state = GameState.Cutscene;
            StartCoroutine(trainer.TriggerTrainerBattle(playerController));
        }
       
    }

    void EndBattle(bool won)
    {
        if (trainer != null && won == true)
        {
            StartCoroutine(trainer.BattleLost(transform));
            trainer = null;
        }

        partyScreen.SetPartyData();

        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);

        var playerParty = playerController.GetComponent<PokemonParty>();
        bool hasEvolutions = playerParty.CheckForEvolutions();

        if (hasEvolutions)
            StartCoroutine(playerParty.RunEvolutions());
        else
            AudioManager.i.PlayMusic(CurrentScene.SceneMusic, fade: true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                menuController.OpenMenu();
                state = GameState.Menu;
            }

            /*if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenuController.OpenMenu();
                state = GameState.MainMenu;
            }*/
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.MainMenu)
        {
            mainMenuController.HandleUpdate();
        }
        else if (state == GameState.PartyScreen)
        {
            Action onSelected = () =>
            {
                // TODO: Go to Summary Screen
            };

            Action onBack = () =>
            {
                partyScreen.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            partyScreen.HandleUpdate(onSelected, onBack);
        }
        else if (state == GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == GameState.Shop)
        {
            ShopController.i.HandleUpdate();
        }
    }

    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;
    }

    public void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            // Pokemon
            partyScreen.gameObject.SetActive(true);
            state = GameState.PartyScreen;
        }
        else if (selectedItem == 1)
        {
            // Bag
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Bag;
        }
        else if (selectedItem == 2)
        {
            // Save
            SavingSystem.i.Save("saveSlot1");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3)
        {
            // Load
            SavingSystem.i.Load("saveSlot1");
            state = GameState.FreeRoam;
        }
    }
    public void OnMainMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            // New Game
            var operation = SceneManager.LoadSceneAsync("Gameplay");
            operation.completed += (AsyncOperation op) =>
            {
                state = GameState.FreeRoam;
            };
        }
        else if (selectedItem == 1)
        {
            // Load
            var operation = SceneManager.LoadSceneAsync("Gameplay");

            operation.completed += (AsyncOperation op) =>
            {
                SavingSystem.i.Load("saveSlot1");
                state = GameState.FreeRoam;
            };            
        }
        else if (selectedItem == 2)
        {
            // Exit
            Application.Quit();
        }
    }


    public IEnumerator MoveCamera(Vector2 moveOffset, bool waitForFadeOut=false)
    {
        yield return Fader.i.FadeIn(0.5f);

        worldCamera.transform.position += new Vector3(moveOffset.x, moveOffset.y);

        if (waitForFadeOut)
            yield return Fader.i.FadeOut(0.5f);
        else
            StartCoroutine(Fader.i.FadeOut(0.5f));
    }

    public GameState State => state;
}
