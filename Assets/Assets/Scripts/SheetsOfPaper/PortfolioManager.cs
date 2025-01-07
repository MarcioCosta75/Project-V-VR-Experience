using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioManager : MonoBehaviour
{
    public static PortfolioManager Instance { get; private set; }

    [SerializeField] private List<SheetOfPaper> sheetsOfPaper; // List of all sheets in the portfolio
    [SerializeField] private UIController uiController; // UI controller
    [SerializeField] private float finalMessageDisplayTime = 3f; // Time to display the final message

    private int currentSheetIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (sheetsOfPaper.Count > 0 && sheetsOfPaper[0] != null)
        {
            sheetsOfPaper[0].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No sheets of paper are set in the PortfolioManager or the first sheet is missing!");
        }
    }

    public void CollectSheet(SheetOfPaper sheet)
    {
        if (currentSheetIndex == 0)
        {
            uiController.ShowUI();
        }

        uiController.ShowHint(sheet.GetHint());

        if (sheet.IsFinalSheet())
        {
            StartCoroutine(HandleFinalSheet());
        }
        else
        {
            currentSheetIndex++;
            uiController.UpdateProgress(currentSheetIndex, sheetsOfPaper.Count);
        }
    }

    private IEnumerator HandleFinalSheet()
    {
        uiController.ShowHint("Portfolio complete! Proceed to the next challenge.");

        yield return new WaitForSeconds(finalMessageDisplayTime);

        uiController.HideUI();;
    }
}
