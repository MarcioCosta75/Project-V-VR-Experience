using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private string correctTowerTag;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioSource audioSource;

    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlaced) return;

        if (other.CompareTag(correctTowerTag))
        {
            PlaySound(correctSound);

            transform.position = other.transform.position + Vector3.up * other.bounds.size.y;
            transform.rotation = Quaternion.identity;
            isPlaced = true;

            ColorMatchGameManager.Instance.ObjectPlaced();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
