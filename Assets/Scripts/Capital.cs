using System;
using UnityEngine;

public class Capital : Building
{
    [SerializeField] private float beeRate;
    private float timeUntilNextBee;

    private void FixedUpdate()
    {
        if (!this.gameStart) return;

        if (timeUntilNextBee < 0)
        {
            this.AddBee();
            timeUntilNextBee = beeRate;
        } else
        {
            timeUntilNextBee -= Time.fixedDeltaTime;
        }
    }
}
