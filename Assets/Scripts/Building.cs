using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BeeSendInfo
{
    public int BeesLeftToBeSent;
    public GameObject BeeDestination;
    public float TimeUntilNextBee;
};

[RequireComponent(typeof(SpriteRenderer))]

public class Building : MonoBehaviour
{
    private TMP_Text beeCountText;
    [SerializeField] private Vector3 textPosition;
    private int health;
    private int maxHealth;
    public int beeCount { get; private set; }
    protected int maxBeeCapacity = 100;
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private float timeBetweenBees;
    public List<BeeSendInfo> beeSendInfos;

    protected bool gameStart;
    public bool Built { get; private set; }
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

        Built = true;
        GameObject countObject = Instantiate(textPrefab, textPosition + transform.position, Quaternion.identity, worldCanvas);

        beeCountText = countObject.GetComponent<TMP_Text>();

        beeSendInfos = new();
    }

    public void StartGame()
    {
        gameStart = true;
    }

    public void Update()
    {
        if (!Built) return;

        beeCountText.SetText($"{beeCount}");

        if (beeSendInfos.Count > 0)
        {
            for (int i = beeSendInfos.Count - 1; i >= 0; i--)
            {
                BeeSendInfo info = beeSendInfos[i];

                if (info.BeesLeftToBeSent > 0)
                {
                    Debug.Log("here");
                    if (info.TimeUntilNextBee < 0)
                    {
                        if (beeCount > 0)
                        {
                            SendBee(info);

                            info.TimeUntilNextBee = timeBetweenBees;
                        }
                        else
                        {
                            beeSendInfos.RemoveAt(i);
                        }
                    }
                    else
                    {
                        Debug.Log($"time: {info.TimeUntilNextBee}");
                        info.TimeUntilNextBee -= Time.deltaTime;
                    }
                }
                else
                {
                    beeSendInfos.RemoveAt(i);
                }
            }

        }
    }

    #region Bee
    public void MoveBees(float amount, Building destination)
    {
        int bees = (int)(beeCount * amount);

        beeSendInfos.Add(new BeeSendInfo
        {
            BeesLeftToBeSent = bees,
            BeeDestination = destination.gameObject,
            TimeUntilNextBee = timeBetweenBees
        });
    }

    private void SendBee(BeeSendInfo info)
    {
        beeCount--;
        info.BeesLeftToBeSent--;

        GameObject beeObj = Instantiate(beePrefab, transform.position, Quaternion.identity);

        if (beeObj.TryGetComponent(out Bee bee))
        {
            bee.Send(transform, info.BeeDestination.transform);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bee bee))
        {
            if (bee.destination == transform)
            {
                beeCount++;
                Destroy(collision.gameObject);
            }
        }
    }

    #endregion
}
