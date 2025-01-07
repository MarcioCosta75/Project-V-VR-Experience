using UnityEngine;

public class SheetOfPaper : MonoBehaviour
{
    [SerializeField] private string hintForNextSheet; 
    [SerializeField] private SheetOfPaper nextSheet; 
    [SerializeField] private bool isFinalSheet = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Collect()
    {
        PortfolioManager.Instance.CollectSheet(this);

        gameObject.SetActive(false);

        if (nextSheet != null)
        {
            nextSheet.gameObject.SetActive(true);
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