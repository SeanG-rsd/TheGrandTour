using System;
using UnityEngine;

public class Capital : Building
{
    [SerializeField] private float beeRate;
    private float timeUntilNextBee;

    private void FixedUpdate()
    {
        if (!this.gameStart) return;

        if (timeUntilNextBee < 0 && beeCount < this.maxBeeCapacity)
        {
            this.AddBee();
            timeUntilNextBee = beeRate;
        } else
        {
            timeUntilNextBee -= Time.fixedDeltaTime;
        }
    }
}
