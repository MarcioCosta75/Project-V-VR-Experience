using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortfolioManager : MonoBehaviour
{
    public static PortfolioManager Instance { get; private set; }

    [SerializeField] private List<SheetOfPaper> sheetsOfPaper;
    [SerializeField] private UIController uiController;
    [SerializeField] private float finalMessageDisplayTime = 3f;

    [Header("Tower Game Settings")]
    [SerializeField] private GameObject towerGame;
    [SerializeField] private AudioSource activationSound;

    private int currentSheetIndex = 1;

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

        if (towerGame != null)
        {
            towerGame.SetActive(false);
        }
    }

    public void CollectSheet(SheetOfPaper sheet)
    {
        if (currentSheetIndex == 1)
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
        uiController.ShowHint("You have completed the Design Portfolio! Solve the last challenge, and you will find the Design Student!");

        yield return new WaitForSeconds(finalMessageDisplayTime);

        uiController.HideUI();

        ActivateTowerGame();
    }

    private void ActivateTowerGame()
    {
        if (towerGame != null)
        {
            towerGame.SetActive(true);

            if (activationSound != null)
            {
                activationSound.Play();
            }
        }
    }
}