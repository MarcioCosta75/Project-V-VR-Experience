using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private List<string> dialogueLines;
    [SerializeField] private List<ChatBubble3D.IconType> dialogueIcons;
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
        }

        if (currentLineIndex < dialogueLines.Count)
        {
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                dialogueIcons[currentLineIndex],
                dialogueLines[currentLineIndex]
            );
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
        }

        animator.SetTrigger("Talk");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
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
    }
}
