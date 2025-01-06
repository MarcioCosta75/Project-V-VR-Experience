using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private List<string> dialogueLines; // Lista de falas
    [SerializeField] private List<ChatBubble3D.IconType> dialogueIcons; // Lista de ícones para cada fala
    private int currentLineIndex = 0;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private ChatBubble3D activeChatBubble; // Referência à bolha de chat ativa
    private Transform playerTransform; // Referência ao transform do jogador

    [SerializeField] private float interactionResetDistance = 5f; // Distância para resetar diálogo
    private bool isPlayerNearby = false; // Controla se o jogador está próximo

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

            // Se o jogador sair do alcance de interação, resete o diálogo
            if (distanceToPlayer > interactionResetDistance && isPlayerNearby)
            {
                ResetDialogue();
                isPlayerNearby = false; // Jogador já está fora do alcance
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
        playerTransform = interactorTransform; // Armazena a referência ao jogador

        // Se houver uma mensagem ativa, destrua antes de criar outra
        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);
        }

        if (currentLineIndex < dialogueLines.Count)
        {
            // Cria a próxima linha da narrativa
            activeChatBubble = ChatBubble3D.Create(
                transform,
                new Vector3(-.3f, 1.7f, 0f),
                dialogueIcons[currentLineIndex], // Ícone correspondente à linha
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
                ChatBubble3D.IconType.Neutral, // Ícone padrão para o final
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
        // Reseta o índice de diálogo e destrói a bolha ativa, se existir
        currentLineIndex = 0;
        if (activeChatBubble != null)
        {
            Destroy(activeChatBubble.gameObject);
            activeChatBubble = null;
        }
    }
}
