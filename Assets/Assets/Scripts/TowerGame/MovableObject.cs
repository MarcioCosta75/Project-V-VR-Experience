using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private bool isPlayerNearby = false; // Verifica se o jogador está próximo
    private bool isBeingHeld = false;   // Verifica se o objeto está sendo segurado
    private Transform player;          // Referência ao jogador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            player = other.transform; // Armazena a referência ao jogador
            Debug.Log("Player entered the trigger zone. Ready to pick up the object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
            Debug.Log("Player left the trigger zone.");
        }
    }

    private void Update()
    {
        // Verifica se o jogador está próximo e clicou no mouse
        if (isPlayerNearby && Input.GetMouseButtonDown(0))
        {
            isBeingHeld = true; // Começa a segurar o objeto
            Debug.Log("Object picked up.");
        }

        if (isBeingHeld)
        {
            if (player != null)
            {
                // Mantém o objeto na frente do jogador enquanto o botão do mouse está pressionado
                Vector3 holdPosition = player.position + player.forward * 1.5f;
                transform.position = Vector3.Lerp(transform.position, holdPosition, Time.deltaTime * 10f);
                Debug.Log($"Object being held. Current position: {transform.position}");
            }
            else
            {
                Debug.LogWarning("Player reference lost while holding the object.");
            }
        }

        // Solta o objeto ao liberar o botão do mouse
        if (Input.GetMouseButtonUp(0) && isBeingHeld)
        {
            isBeingHeld = false; // Solta o objeto
            Debug.Log("Object released.");
        }
    }
}
