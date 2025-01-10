using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private bool isPlayerNearby = false; // Verifica se o jogador est� pr�ximo
    private bool isBeingHeld = false;   // Verifica se o objeto est� sendo segurado
    private Transform player;          // Refer�ncia ao jogador

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            player = other.transform; // Armazena a refer�ncia ao jogador
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
        // Verifica se o jogador est� pr�ximo e clicou no mouse
        if (isPlayerNearby && Input.GetMouseButtonDown(0))
        {
            isBeingHeld = true; // Come�a a segurar o objeto
            Debug.Log("Object picked up.");
        }

        if (isBeingHeld)
        {
            if (player != null)
            {
                // Mant�m o objeto na frente do jogador enquanto o bot�o do mouse est� pressionado
                Vector3 holdPosition = player.position + player.forward * 1.5f;
                transform.position = Vector3.Lerp(transform.position, holdPosition, Time.deltaTime * 10f);
                Debug.Log($"Object being held. Current position: {transform.position}");
            }
            else
            {
                Debug.LogWarning("Player reference lost while holding the object.");
            }
        }

        // Solta o objeto ao liberar o bot�o do mouse
        if (Input.GetMouseButtonUp(0) && isBeingHeld)
        {
            isBeingHeld = false; // Solta o objeto
            Debug.Log("Object released.");
        }
    }
}
