using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<string> dialogueLines; // Dialogue lines
    [SerializeField] private List<ChatBubble3D.IconType> dialogueIcons; // Corresponding icons
    [SerializeField] private List<AudioClip> dialogueClips; // List of AudioClips corresponding to each dialogue line
    [SerializeField] private AudioSource dialogueAudioSource; // AudioSource for playing dialogue sounds

    private int currentLineIndex = 0;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private ChatBubble3D activeChatBubble;
    private Transform playerTransform;

    [SerializeField] private float interactionResetDistance = 5f;
    private bool isPlayerNearby = false;

    [Header("Door Settings")]
    [SerializeField] private GameObject entranceDoors;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource doorSound;
    private bool hasOpenedDoors = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();

        // Ensure the AudioSource is properly configured
        if (dialogueAudioSource != null)
        {
            dialogueAudioSource.loop = false; // Dialogue sounds should not loop
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

            // Stop the current dialogue sound
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

            // Play the corresponding dialogue sound
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

            if (!hasOpenedDoors)
            {
                OpenDoors();
                hasOpenedDoors = true;
            }

            // Stop the sound when the dialogue ends
            StopDialogueSound();
        }

        animator.SetTrigger("Talk");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
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

    private void OpenDoors()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }
        if (doorSound != null)
        {
            doorSound.Play();
        }

        StartCoroutine(DisableDoorsAfterAnimation(3f));
    }

    private IEnumerator DisableDoorsAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (entranceDoors != null)
        {
            entranceDoors.SetActive(false);
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

        // Stop dialogue sound on reset
        StopDialogueSound();
    }
}
