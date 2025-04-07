using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InstrumentUIManager : MonoBehaviour
{
    public GameObject instrumentButtonPrefab;
    private List<InstrumentDataSO> instrumentList;
    private List<GameObject> instrumentButtonList;
    public Transform contentPanel;

    [SerializeField] private TMP_Text currentInstrumentName;


    private void Start()
    {
        instrumentList = InstrumentManager.Instance.instrumentList;
        instrumentButtonList = new List<GameObject>();

        for (int i = 0; i < instrumentList.Count; i++)
        {
            int index = i; // captures the current value of i to prevent all lambdas from referencing the same final value of i.

            GameObject instrumentButton = Instantiate(instrumentButtonPrefab, contentPanel);

            instrumentButton.GetComponentInChildren<TextMeshProUGUI>().text = InstrumentManager.Instance.GetInstrumentName(index);
            instrumentButton.GetComponent<Button>().onClick.AddListener( () => InstrumentManager.Instance.SelectInstrument(index));

            instrumentButtonList.Add(instrumentButton);
        }
    }

    public void SetCurrentInstrumentName(string instrumentName)
    {
        currentInstrumentName.text = instrumentName;
    }
}
