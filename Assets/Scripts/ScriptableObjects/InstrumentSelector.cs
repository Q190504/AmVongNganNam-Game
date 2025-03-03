using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentSelector : MonoBehaviour
{
    public Button[] instrumentButtons;

    private void Start()
    {
        for (int i = 0; i < instrumentButtons.Length; i++)
        {
            int index = i; // captures the current value of i to prevent all lambdas from referencing the same final value of i.

            instrumentButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = InstrumentManager.Instance.GetInstrumentName(index);
            instrumentButtons[i].onClick.AddListener(() => InstrumentManager.Instance.SelectInstrument(index));
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < instrumentButtons.Length; i++)
        {
            instrumentButtons[i].onClick.RemoveAllListeners();
        }
    }
}
