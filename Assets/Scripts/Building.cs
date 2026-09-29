using System;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private TMP_Text beeCountText;
    private int health;
    private int maxHealth;
    public int beeCount {get; private set;}
    private int maxBeeCapacity = 100;

    protected bool gameStart;

    protected void AddBee(int amount = 1)
    { 
        beeCount = Math.Min(maxBeeCapacity, beeCount + amount); 
    }

    public void Setup()
    {
        
    }

    public void StartGame()
    {
        gameStart = true;
    }

    public void Update()
    {
        beeCountText.text = beeCount.ToString();
    }

    public void MoveBees(float amount, Building destination)
    {
        int bees = (int)(beeCount * amount);

        beeCount -= bees;
        destination.AddBee(bees);
    }
}
