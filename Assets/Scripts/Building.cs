using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    private TMP_Text beeCountText;
    [SerializeField] private Vector3 textPosition;
    private int health;
    private int maxHealth;
    public int beeCount {get; private set;}
    protected int maxBeeCapacity = 100;

    protected bool gameStart;
    public bool built {get; private set;}
    protected void AddBee(int amount = 1)
    { 
        beeCount += amount;
    }

    public void Hover()
    {
        Color prev = GetComponent<SpriteRenderer>().color;
        prev.a = 0.4f;

        GetComponent<SpriteRenderer>().color = prev;
    }

    public void Build(Transform worldCanvas, GameObject textPrefab)
    {
        Color prev = GetComponent<SpriteRenderer>().color;
        prev.a = 1;

        GetComponent<SpriteRenderer>().color = prev;

        built = true;
        GameObject countObject = Instantiate(textPrefab, textPosition + transform.position, Quaternion.identity, worldCanvas);

        beeCountText = countObject.GetComponent<TMP_Text>();
    }

    public void StartGame()
    {
        gameStart = true;
    }

    public void Update()
    {
        if (!built) return;

        beeCountText.SetText($"{beeCount}");
    }

    public void MoveBees(float amount, Building destination)
    {
        int bees = (int)(beeCount * amount);

        beeCount -= bees;
        destination.AddBee(bees);
    }
}
