using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialog : MonoBehaviour, Interactable
{

    #region Manu Code
    [Header("Image and name")]
    [SerializeField] Sprite sprite;
    [SerializeField] string nameText;
    #endregion


    [SerializeField] ItemBase item;
    [SerializeField] Dialog dialog;

    public bool Used { get; set; } = false;

    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);
    }

    /*public object CaptureState()
    {
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }*/
}
