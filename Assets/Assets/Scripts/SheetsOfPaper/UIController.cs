using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject progressContainer;
    [SerializeField] private GameObject hintContainer;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI hintText;

    private void Start()
    {
        HideUI();
    }

    public void ShowUI()
    {
        progressContainer.SetActive(true);
        hintContainer.SetActive(true);
    }

    public void HideUI()
    {
        progressContainer.SetActive(false);
        hintContainer.SetActive(false);
    }

    public void UpdateProgress(int collected, int total)
    {
        progressText.text = $"Sheets Collected: {collected}/{total}";
    }

    public void ShowHint(string hint)
    {
        hintText.text = hint;
    }
}
