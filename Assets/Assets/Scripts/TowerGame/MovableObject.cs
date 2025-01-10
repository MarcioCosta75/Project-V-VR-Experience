using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private string correctTowerTag;
    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {

        if (isPlaced) return;

        if (other.CompareTag(correctTowerTag))
        {
            transform.position = other.transform.position + Vector3.up * other.bounds.size.y;
            transform.rotation = Quaternion.identity;
            isPlaced = true;

            ColorMatchGameManager.Instance.ObjectPlaced();
        }
    }
}
