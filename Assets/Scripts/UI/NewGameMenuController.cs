using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] AudioClip newGameMenuAudio;
    [SerializeField] InputField inputField;
    [SerializeField] Text sign;
    [SerializeField] Dialog dialog;
    [SerializeField] List<Text> menuItems;

    public event Action<int> onMenuSelected;
    public event Action onBack;

    int selectedItem = 0;

    public InputField InputField { get => inputField; set => inputField = value; }

    private void Awake()
    {
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection();
        AudioManager.i.PlayMusic(newGameMenuAudio, fade: true);
    }

    public IEnumerator openDialog() {
        yield return DialogManager.Instance.ShowDialog(dialog);
        inputField.gameObject.SetActive(true);
        sign.gameObject.SetActive(true);


    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void HandleUpdate()
    {
        int prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++selectedItem;
            inputField.DeactivateInputField();
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            --selectedItem;
            inputField.DeactivateInputField();
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if (prevSelection != selectedItem)
            UpdateItemSelection();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onMenuSelected?.Invoke(selectedItem);
            //CloseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //onBack?.Invoke();
            //CloseMenu();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            if (i == selectedItem)
                menuItems[i].color = GlobalSettings.i.HighlightedColor;
            else
                menuItems[i].color = Color.black;
        }
    }
}
