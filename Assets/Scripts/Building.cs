using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    private int health;
    private int maxHealth;
    public int beeCapacity {get; private set;}
    private int maxBeeCapacity = 100;

    protected bool gameStart;

    protected void AddBee() { beeCapacity = Math.Min(maxBeeCapacity, beeCapacity + 1); }
    public void StartGame() { gameStart = true; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
