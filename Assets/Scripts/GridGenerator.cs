using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Material[] squareColors;
    [SerializeField] private Vector3 gridStartPoint;
    [SerializeField] private Vector2Int gridSize;

    public void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                // Create grid square
                GameObject square = Instantiate(squarePrefab, gridStartPoint + new Vector3(x, 0, z), Quaternion.identity);

                MeshRenderer meshRenderer = square.GetComponent<MeshRenderer>();

                meshRenderer.material = squareColors[(x * gridSize.y + z) % squareColors.Length];

            }
        }
    }
}
