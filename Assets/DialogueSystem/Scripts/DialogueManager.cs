using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Animator animator;

    private Queue<string> sentences;

    public event Action OnStartDialogue;
    public event Action OnEndDialogue;

    public static DialogueManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sentences = new();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        OnStartDialogue?.Invoke();

        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = string.Empty;

        yield return new WaitForSeconds(0.25f);

        foreach (char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator?.SetBool("IsOpen", false);

        OnEndDialogue?.Invoke();
    }
}
