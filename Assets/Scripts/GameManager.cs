using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridGenerator.GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
