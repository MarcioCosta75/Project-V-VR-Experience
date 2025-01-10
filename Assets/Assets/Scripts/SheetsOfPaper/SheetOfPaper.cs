using UnityEngine;

public class SheetOfPaper : MonoBehaviour
{
    [SerializeField] private string hintForNextSheet; 
    [SerializeField] private SheetOfPaper nextSheet; 
    [SerializeField] private bool isFinalSheet = false;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Collect()
    {
        PlayCollectSound();

        PortfolioManager.Instance.CollectSheet(this);

        gameObject.SetActive(false);

        if (nextSheet != null)
        {
            nextSheet.gameObject.SetActive(true);
        }
    }

    private void PlayCollectSound()
    {
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    public string GetHint()
    {
        return hintForNextSheet;
    }

    public bool IsFinalSheet()
    {
        return isFinalSheet;
    }
}
