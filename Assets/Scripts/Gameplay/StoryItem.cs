using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryItem : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Dialog dialog;
    #region Manu Code
    [SerializeField] Sprite sprite;
    #endregion

    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false;
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, sprite));
    }

    public bool TriggerRepeatedly => false;
}
