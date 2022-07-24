using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState { Menu, Buying, Selling, Busy }

public class ShopController : MonoBehaviour
{
    [SerializeField] Vector2 shopCameraOffset;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ShopUI shopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    [Header("Dialog")]
    [TextArea][SerializeField] string howMayIServeYou;
    [TextArea][SerializeField] string buy;
    [TextArea][SerializeField] string sell;
    [TextArea][SerializeField] string quit;
    [TextArea][SerializeField] string howManyWouldYouLikeToSell;
    [TextArea][SerializeField] string iCanGive;
    [TextArea][SerializeField] string youCantSellThat;
    [TextArea][SerializeField] string forThat;
    [TextArea][SerializeField] string wouldYouLikeToSell;
    [TextArea][SerializeField] string turnedOver;
    [TextArea][SerializeField] string andReceived;
    [TextArea][SerializeField] string howManyWouldYouLikeToBuy;
    [TextArea][SerializeField] string thatWillBe;
    [TextArea][SerializeField] string thankYouForShoppingUs;
    [TextArea][SerializeField] string notEnoghtMoneyForThat;
    [TextArea][SerializeField] string yes;
    [TextArea][SerializeField] string no;


    public event Action OnStart;
    public event Action OnFinish;

    ShopState state;

    Merchant merchant;

    public static ShopController i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public IEnumerator StartTrading(Merchant merchant)
    {
        this.merchant = merchant;

        OnStart?.Invoke();
        yield return StartMenuState();
    }

    IEnumerator StartMenuState()
    {
        state = ShopState.Menu;

        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialogText($"{howMayIServeYou}",
            waitForInput: false,
            choices: new List<string>() { $"{buy}", $"{sell}", $"{quit}" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Buy
            yield return GameController.Instance.MoveCamera(shopCameraOffset);
            walletUI.Show();
            shopUI.Show(merchant.AvailableItems, (item) => StartCoroutine(BuyItem(item)),
                () => StartCoroutine(OnBackFromBuying()));

            state = ShopState.Buying;
        }
        else if (selectedChoice == 1)
        {
            // Sell
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            // Quit
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
        else if (state == ShopState.Buying)
        {
            shopUI.HandleUpdate();
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }

    IEnumerator SellItem(ItemBase item)
    {
        state = ShopState.Busy;

        if (!item.IsSellable)
        {
            yield return DialogManager.Instance.ShowDialogText($"{youCantSellThat}");
            state = ShopState.Selling;
            yield break;
        }

        walletUI.Show();

        float sellingPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventory.GetItemCount(item);
        if (itemCount > 1)
        {
            yield return DialogManager.Instance.ShowDialogText($"{howManyWouldYouLikeToSell}",
                waitForInput: false, autoClose: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellingPrice,
                (selectedCount) => countToSell = selectedCount);

            DialogManager.Instance.CloseDialog();
        }

        sellingPrice = sellingPrice * countToSell;

        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialogText($"{iCanGive} {Math.Round(sellingPrice / 2)}$ {forThat}! {wouldYouLikeToSell}",
            waitForInput: false,
            choices: new List<string>() { $"{yes}", $"{no}" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Yes
            inventory.RemoveItem(item, countToSell);
            Wallet.i.AddMoney((float)Math.Round(sellingPrice / 2));
            yield return DialogManager.Instance.ShowDialogText($"{turnedOver} {item.Name} {andReceived} {Math.Round(sellingPrice / 2)}$ !");
        }

        walletUI.Close();

        state = ShopState.Selling;
    }

    IEnumerator BuyItem(ItemBase item)
    {
        state = ShopState.Busy;

        yield return DialogManager.Instance.ShowDialogText($"{howManyWouldYouLikeToBuy}?",
            waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(100, item.Price,
            (selectedCount) => countToBuy = selectedCount);

        DialogManager.Instance.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogManager.Instance.ShowDialogText($"{thatWillBe} {totalPrice}$.",
                waitForInput: false,
                choices: new List<string>() { $"{yes}", $"{no}" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Selected Yes
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogManager.Instance.ShowDialogText($"{thankYouForShoppingUs}");
            }
        }
        else
        {
            yield return DialogManager.Instance.ShowDialogText($"{notEnoghtMoneyForThat}");
        }

        state = ShopState.Buying;
    }

    IEnumerator OnBackFromBuying()
    {
        yield return GameController.Instance.MoveCamera(-shopCameraOffset);
        shopUI.Close();
        walletUI.Close();
        StartCoroutine(StartMenuState());
    }
}
