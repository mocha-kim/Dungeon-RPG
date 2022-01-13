using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPC : MonoBehaviour, IInteractable
{
    public QuestObject quest;

    public Dialogue readyDialogue;
    public Dialogue acceptedDialogue;
    public Dialogue completedDialogue;

    bool isStartQuestDialogue = false;
    GameObject interactGO = null;

    private float distance = 2.0f;
    public float Distance => distance;

    // Start is called before the first frame update
    void Start()
    {
        QuestManager.Instance.OnCompletedQuest += OnCompletedQuest;
    }

    private void OnEndDialogue()
    {
        StopInteract(interactGO);
    }

    private void OnCompletedQuest(QuestObject quest)
    {
        if (quest.data.id == this.quest.data.id && quest.status == QuestStatus.Completed)
        {
            // effect
        }
    }

    #region IInteractable

    public void Interact(GameObject other)
    {
        Debug.Log("interact with " + other.name);
        float clacDis = Vector3.Distance(other.transform.position, transform.position);
        if (clacDis > distance)
            return;

        if (isStartQuestDialogue)
            return;

        interactGO = other;

        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartQuestDialogue = true;

        if (quest.status == QuestStatus.None)
        {
            DialogueManager.Instance.StartDialogue(readyDialogue);
            quest.status = QuestStatus.Accepted;
        }
        else if (quest.status == QuestStatus.Accepted)
        {
            DialogueManager.Instance.StartDialogue(acceptedDialogue);
        }
        else if (quest.status == QuestStatus.Completed)
        {
            DialogueManager.Instance.StartDialogue(completedDialogue);
            // reward
            quest.status = QuestStatus.Rewarded;
        }
    }


    public void StopInteract(GameObject other)
    {
        isStartQuestDialogue = false;
    }

    #endregion IInteractable
}
