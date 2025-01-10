using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    [SerializeField] private Transform[] towers; // Torres no cenário
    [SerializeField] private string[] correctTags; // Tags para correspondência de cores
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

            // Verifica se o objeto está na torre correta
            if (Vector3.Distance(obj.transform.position, tower.position) < 1f)
            {
                Debug.Log($"Object {obj.name} is close to tower {tower.name}");

                if (obj.CompareTag(tower.tag))
                {
                    Debug.Log($"Object {obj.name} matches the tag of tower {tower.name}");

                    obj.transform.position = tower.position; // Encaixa no topo da torre
                    obj.GetComponent<Collider>().enabled = false; // Desativa interações
                    correctPlacements++;

                    Debug.Log($"Correct placements: {correctPlacements}/{towers.Length}");

                    // Verifica condição de vitória
                    if (correctPlacements == towers.Length)
                    {
                        PuzzleCompleted();
                    }
                    return; // Objeto encaixado, não precisa continuar verificando outras torres
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
        // Adicione lógica para passar para o próximo evento ou fase
    }
}
