using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public Animator npcAnimator;
      
    void Start()
    {
        npcAnimator = GetComponent<Animator>();
        DialogueManager.instance.conversationStarted += OnConversationStarted;
        DialogueManager.instance.conversationEnded += OnConversationEnded;
    }

    void OnConversationStarted(Transform actor)
    {
        if (actor == this.transform)
        {
            npcAnimator.SetBool("isTalk", true);
        }
    }

    void OnConversationEnded(Transform actor)
    {
        if (actor == this.transform)
        {
            npcAnimator.SetBool("isTalk", false);
        }
    }

    private void OnDisable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationStarted -= OnConversationStarted;
            DialogueManager.instance.conversationEnded -= OnConversationEnded;
        }
    }
}
