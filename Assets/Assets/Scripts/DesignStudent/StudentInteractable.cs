using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> dialogueLines;
    [SerializeField] private List<ChatBubble3D.IconType> dialogueIcons;
    [SerializeField] private List<AudioClip> dialogueClips;
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private Animator studentAnimator;
    [SerializeField] private AudioClip interactionEndClip;

    private int currentLineIndex = 0;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private ChatBubble3D activeChatBubble;
    private Transform playerTransform;

    [SerializeField] private float interactionResetDistance = 5f;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();

        if (dialogueAudioSource != null)
        {
            dialogueAudioSource.loop = false;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > interactionResetDistance && isPlayerNearby)
            {
                ResetDialogue();
                isPlayerNearby = false;
            }

            if (distanceToPlayer <= interactionResetDistance && !isPlayerNearby)
            {
                isPlayerNearby = true;
            }
        }
    }

    public void Interact(Transform interactorTransform)
    {
        playerTransform = interactorTransform;

        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);

            StopDialogueSound();
        }

        if (currentLineIndex < dialogueLines.Count)
        {
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                dialogueIcons[currentLineIndex],
                dialogueLines[currentLineIndex]
            );

            PlayDialogueSound(currentLineIndex);

            currentLineIndex++;
        }
        else
        {
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                ChatBubble3D.IconType.Neutral,
                "That's all for now!"
            );

            StopDialogueSound();

            EndInteraction();
        }

        animator.SetTrigger("Talk");
    }

    private void PlayDialogueSound(int index)
    {
        if (dialogueAudioSource != null && index < dialogueClips.Count && dialogueClips[index] != null)
        {
            dialogueAudioSource.clip = dialogueClips[index];
            dialogueAudioSource.Play();
        }
    }

    private void StopDialogueSound()
    {
        if (dialogueAudioSource != null)
        {
            dialogueAudioSource.Stop();
        }
    }

    private void EndInteraction()
    {
        if (studentAnimator != null)
        {
            studentAnimator.SetTrigger("InteractionEnd");
        }

        if (dialogueAudioSource != null && interactionEndClip != null)
        {
            dialogueAudioSource.PlayOneShot(interactionEndClip);
        }
    }

    public string GetInteractText()
    {
        return currentLineIndex < dialogueLines.Count ? "Talk" : "Goodbye";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void ResetDialogue()
    {
        currentLineIndex = 0;

        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);
            activeChatBubble = null;
        }

        StopDialogueSound();
    }
}