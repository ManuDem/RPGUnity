using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [Header("Image and name")]
    [SerializeField] Sprite sprite;
    [SerializeField] string nameText;


    [Header("Dialog")]
    [TextArea] [SerializeField] string howMayIServeYou;
    [TextArea] [SerializeField] string buy;
    [TextArea] [SerializeField] string sell;
    [TextArea] [SerializeField] string quit;
    [TextArea] [SerializeField] string howManyWouldYouLikeToSell;
    [TextArea] [SerializeField] string iCanGive;
    [TextArea] [SerializeField] string youCantSellThat;
    [TextArea] [SerializeField] string forThat;
    [TextArea] [SerializeField] string wouldYouLikeToSell;
    [TextArea] [SerializeField] string turnedOver;
    [TextArea] [SerializeField] string andReceived;
    [TextArea] [SerializeField] string howManyWouldYouLikeToBuy;
    [TextArea] [SerializeField] string thatWillBe;
    [TextArea] [SerializeField] string thankYouForShoppingUs;
    [TextArea] [SerializeField] string notEnoghtMoneyForThat;
    [TextArea] [SerializeField] string yes;
    [TextArea] [SerializeField] string no;

    [SerializeField] List<ItemBase> availableItems;

    public IEnumerator Trade()
    {
        yield return ShopController.i.StartTrading(this);
    }

    public Sprite Sprite { get => sprite; set => sprite = value; }
    public string NameText { get => nameText; set => nameText = value; }
    public List<ItemBase> AvailableItems { get => availableItems; set => availableItems = value; }
    public string HowMayIServeYou { get => howMayIServeYou; set => howMayIServeYou = value; }
    public string Buy { get => buy; set => buy = value; }
    public string Sell { get => sell; set => sell = value; }
    public string Quit { get => quit; set => quit = value; }
    public string HowManyWouldYouLikeToSell { get => howManyWouldYouLikeToSell; set => howManyWouldYouLikeToSell = value; }
    public string ICanGive { get => iCanGive; set => iCanGive = value; }
    public string YouCantSellThat { get => youCantSellThat; set => youCantSellThat = value; }
    public string ForThat { get => forThat; set => forThat = value; }
    public string WouldYouLikeToSell { get => wouldYouLikeToSell; set => wouldYouLikeToSell = value; }
    public string TurnedOver { get => turnedOver; set => turnedOver = value; }
    public string AndReceived { get => andReceived; set => andReceived = value; }
    public string HowManyWouldYouLikeToBuy { get => howManyWouldYouLikeToBuy; set => howManyWouldYouLikeToBuy = value; }
    public string ThatWillBe { get => thatWillBe; set => thatWillBe = value; }
    public string ThankYouForShoppingUs { get => thankYouForShoppingUs; set => thankYouForShoppingUs = value; }
    public string NotEnoghtMoneyForThat { get => notEnoghtMoneyForThat; set => notEnoghtMoneyForThat = value; }
    public string Yes { get => yes; set => yes = value; }
    public string No { get => no; set => no = value; }

}
