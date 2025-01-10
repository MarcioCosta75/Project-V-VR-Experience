using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    [SerializeField] private Transform[] towers; // Torres no cen�rio
    [SerializeField] private string[] correctTags; // Tags para correspond�ncia de cores
    private int correctPlacements = 0;

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

    public void CheckPlacement(GameObject obj)
    {
        Debug.Log($"Checking placement for object: {obj.name}");

        foreach (Transform tower in towers)
        {
            Debug.Log($"Checking tower: {tower.name} with tag {tower.tag}");

            // Verifica se o objeto est� na torre correta
            if (Vector3.Distance(obj.transform.position, tower.position) < 1f)
            {
                Debug.Log($"Object {obj.name} is close to tower {tower.name}");

                if (obj.CompareTag(tower.tag))
                {
                    Debug.Log($"Object {obj.name} matches the tag of tower {tower.name}");

                    obj.transform.position = tower.position; // Encaixa no topo da torre
                    obj.GetComponent<Collider>().enabled = false; // Desativa intera��es
                    correctPlacements++;

                    Debug.Log($"Correct placements: {correctPlacements}/{towers.Length}");

                    // Verifica condi��o de vit�ria
                    if (correctPlacements == towers.Length)
                    {
                        PuzzleCompleted();
                    }
                    return; // Objeto encaixado, n�o precisa continuar verificando outras torres
                }
                else
                {
                    Debug.Log($"Object {obj.name} does not match the tag of tower {tower.name}");
                }
            }
            else
            {
                Debug.Log($"Object {obj.name} is too far from tower {tower.name}");
            }
        }

        Debug.Log($"Object {obj.name} did not match any towers.");
    }

    private void PuzzleCompleted()
    {
        Debug.Log("Puzzle Completed! All objects are correctly placed.");
        // Adicione l�gica para passar para o pr�ximo evento ou fase
    }
}
