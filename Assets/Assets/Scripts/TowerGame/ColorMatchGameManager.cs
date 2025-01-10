using System.Collections;
using UnityEngine;

public class ColorMatchGameManager : MonoBehaviour
{
    public static ColorMatchGameManager Instance { get; private set; }

    [SerializeField] private int totalObjects = 3;
    private int objectsPlaced = 0;

    [Header("Door Settings")]
    [SerializeField] private GameObject door;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private AudioSource doorSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ObjectPlaced()
    {
        objectsPlaced++;

        if (objectsPlaced >= totalObjects)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open");
        }

        if (doorSound != null)
        {
            doorSound.Play();
        }
        StartCoroutine(DisableDoorAfterAnimation(3f));
    }

    private IEnumerator DisableDoorAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
