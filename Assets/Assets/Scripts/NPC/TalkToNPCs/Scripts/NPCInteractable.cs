using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private List<string> dialogueLines; // Lista de falas
    [SerializeField] private List<ChatBubble3D.IconType> dialogueIcons; // Lista de �cones para cada fala
    private int currentLineIndex = 0;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private ChatBubble3D activeChatBubble; // Refer�ncia � bolha de chat ativa
    private Transform playerTransform; // Refer�ncia ao transform do jogador

    [SerializeField] private float interactionResetDistance = 5f; // Dist�ncia para resetar di�logo
    private bool isPlayerNearby = false; // Controla se o jogador est� pr�ximo

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

            // Se o jogador sair do alcance de intera��o, resete o di�logo
            if (distanceToPlayer > interactionResetDistance && isPlayerNearby)
            {
                ResetDialogue();
                isPlayerNearby = false; // Jogador j� est� fora do alcance
            }

            // Se o jogador voltar ao alcance
            if (distanceToPlayer <= interactionResetDistance && !isPlayerNearby)
            {
                isPlayerNearby = true; // Jogador voltou ao alcance
            }
        }
    }

    public void Interact(Transform interactorTransform)
    {
        playerTransform = interactorTransform; // Armazena a refer�ncia ao jogador

        // Se houver uma mensagem ativa, destrua antes de criar outra
        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);
        }

        if (currentLineIndex < dialogueLines.Count)
        {
            // Cria a pr�xima linha da narrativa
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                dialogueIcons[currentLineIndex], // �cone correspondente � linha
                dialogueLines[currentLineIndex]
            );
            currentLineIndex++;
        }
        else
        {
            // Final da narrativa
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                ChatBubble3D.IconType.Neutral, // �cone padr�o para o final
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
        // Reseta o �ndice de di�logo e destr�i a bolha ativa, se existir
        currentLineIndex = 0;
        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);
            activeChatBubble = null;
        }
    }
}
