using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    Dialogue dialogue;

    GameObject interactGO;

    bool isStartDialogue = false;
    [SerializeField]
    float distance = 2.0f;

    public float Distance => distance;

    private void OnEndDialogue()
    {
        StopInteract(interactGO);
    }

    #region IInteractable

    public void Interact(GameObject other)
    {
        Debug.Log("interact with " + other.name);
        float clacDis = Vector3.Distance(other.transform.position, transform.position);
        if (clacDis > distance)
            return;

        if (isStartDialogue)
            return;

        interactGO = other;

        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;
        isStartDialogue = true;

        DialogueManager.Instance.StartDialogue(dialogue);
    }


    public void StopInteract(GameObject other)
    {
        isStartDialogue = false;
    }

    #endregion IInteractable
}
