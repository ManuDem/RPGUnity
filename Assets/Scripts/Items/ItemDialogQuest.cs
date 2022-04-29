using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDialogQuest : MonoBehaviour, Interactable, ISavable
{

    #region Manu Code
    [Header("Image and name")]
    [SerializeField] Sprite sprite;
    [SerializeField] string nameText;
    #endregion

    [Header("Dialog")]
    [SerializeField] Dialog dialog;

    [SerializeField] ItemBase item;

    [Header("Quests")]
    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    IDQState state;
    Quest activeQuest;

    public bool Used { get; set; } = false;

    public IEnumerator Interact(Transform initiator)
    {

        if (questToComplete != null)
        {
            var quest = new Quest(questToComplete, sprite, nameText);
            yield return quest.CompleteQuest(initiator);
            questToComplete = null;

            Debug.Log($"{quest.Base.Name} completed");
        }

        if (questToStart != null)
        {
            activeQuest = new Quest(questToStart, sprite, nameText);
            yield return activeQuest.StartQuest();
            questToStart = null;

            if (activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompleteQuest(initiator);
                activeQuest = null;
            }
        }
        else if (activeQuest != null)
        {
            if (activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompleteQuest(initiator);
                activeQuest = null;
            }
            else
            {
                yield return DialogManager.Instance.ShowDialogSprite(activeQuest.Base.InProgressDialogue, sprite, nameText);
            }
        }
        else
        {
            yield return DialogManager.Instance.ShowDialogSprite(dialog, sprite, nameText);
        }

        state = IDQState.Idle;
    }

    public object CaptureState()
    {
        var saveData = new NPCQuestSaveData();
        saveData.activeQuest = activeQuest?.GetSaveData();

        if (questToStart != null)
            saveData.questToStart = (new Quest(questToStart, sprite, nameText)).GetSaveData();

        if (questToComplete != null)
            saveData.questToComplete = (new Quest(questToComplete, sprite, nameText)).GetSaveData();

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as NPCQuestSaveData;
        if (saveData != null)
        {
            activeQuest = (saveData.activeQuest != null) ? new Quest(saveData.activeQuest) : null;

            questToStart = (saveData.questToStart != null) ? new Quest(saveData.questToStart).Base : null;
            questToComplete = (saveData.questToComplete != null) ? new Quest(saveData.questToComplete).Base : null;
        }
    }
}

[System.Serializable]
public class ItemDialogQuestQuestSaveData
{
    public QuestSaveData activeQuest;
    public QuestSaveData questToStart;
    public QuestSaveData questToComplete;
}

public enum IDQState { Idle, Dialog }