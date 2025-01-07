using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SheetInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private SheetOfPaper sheetOfPaper;
    public TMP_Text eButton;
    public TMP_Text interactText;
    public Image background;

    private void Awake()
    {
        sheetOfPaper = GetComponent<SheetOfPaper>();
        eButton.gameObject.SetActive(false);
        interactText.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            eButton.gameObject.SetActive(true);
            interactText.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            eButton.gameObject.SetActive(false);
            interactText.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            CollectSheet();
        }
    }

    private void CollectSheet()
    {
        if (sheetOfPaper != null)
        {
            sheetOfPaper.Collect();
            eButton.gameObject.SetActive(false);
            interactText.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
        }
    }
}