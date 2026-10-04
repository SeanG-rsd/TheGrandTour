using System;
using UnityEngine;

public class Capital : Building
{
    [SerializeField] private float beeRate;
    private float timeUntilNextBeeMade;

    private void FixedUpdate()
    {
        if (!this.gameStart) return;

        if (timeUntilNextBeeMade < 0 && beeCount < this.maxBeeCapacity)
        {
            this.AddBee();
            timeUntilNextBeeMade = beeRate;
        } else
        {
            timeUntilNextBeeMade -= Time.fixedDeltaTime;
        }
    }
}
