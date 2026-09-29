using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] options;
    [SerializeField] private float[] amounts;

    public void SetupOptions(Action<float> select)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].TryGetComponent(out Button button))
            {
                float amount = amounts[i];
                button.onClick.AddListener(() => select(amount));
            }
        }
    }
}
