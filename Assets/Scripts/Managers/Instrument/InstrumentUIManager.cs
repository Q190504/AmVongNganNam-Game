using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class InstrumentUIManager : MonoBehaviour
{
    public GameObject instrumentButtonPrefab;
    private List<InstrumentDataSO> instrumentList;
    private List<GameObject> instrumentButtonList;
    public Transform contentPanel;
    [SerializeField] public GameObject unlockConfirmPanel;

    private bool? userConfirmedUnlock;

    [SerializeField] private TMP_Text currentInstrumentName;


    private void Start()
    {
        instrumentList = InstrumentManager.Instance.GetInstrumentDatas();
        instrumentButtonList = new List<GameObject>();

        GenerateInstrumentButtons();
    }

    private void GenerateInstrumentButtons()
    {
        bool hasNewInst = false;
        for (int i = 0; i < instrumentList.Count; i++)
        {
            int index = i; // capture index for lambda
            var inst = instrumentList[index];
            var instButton = Instantiate(instrumentButtonPrefab, contentPanel);

            instButton.GetComponentInChildren<TextMeshProUGUI>().text = inst.instrumentName;
            instrumentButtonList.Add(instButton);

            hasNewInst = SetupInstrumentButton(inst, instButton);

            if (hasNewInst)
            {
                SaveInstrumentList();
            }
        }
    }

    private void SaveInstrumentList()
    {
        GameDataSO gameData = SongManager.Instance.GetGameData();
        string[] new_unlocked_instrument = gameData.unlocked_instruments.ToArray();
        int inst_token = gameData.instrument_token;

        GameDataStorage.Instance.SaveGameStatus(null, new_unlocked_instrument, null, -1, inst_token);
    }
    private bool SetupInstrumentButton(InstrumentDataSO inst, GameObject instButton)
    {
        var data = SongManager.Instance.GetGameData();

        if (SongManager.Instance.IsInstIdInData(inst.instrumentId))
        {
            UnlockInstrumentUI(inst, instButton);
        }
        else if (inst.isDefault)
        {
            data.unlocked_instruments.Add(inst.instrumentId);
            UnlockInstrumentUI(inst, instButton);
            return true;
        }
        else
        {
            instButton.GetComponent<Button>().onClick.AddListener(() => UnlockInstrument(inst, instButton));
        }
            
        return false;
    }

    private void UnlockInstrumentUI(InstrumentDataSO inst, GameObject instButton)
    {
        instButton.transform.Find("LockPanel").gameObject.SetActive(false);
        instButton.GetComponent<Button>().onClick.RemoveAllListeners();
        instButton.GetComponent<Button>().onClick.AddListener(() => InstrumentManager.Instance.SelectInstrument(inst.instrumentId));
    }

    public void UnlockInstrument(InstrumentDataSO inst, GameObject instButton)
    {
        StartCoroutine(HandleUnlockConfirmation(inst, instButton));
    }

    
    private IEnumerator HandleUnlockConfirmation(InstrumentDataSO inst, GameObject instButton)
    {
        userConfirmedUnlock = null;

        unlockConfirmPanel.SetActive(true);
        unlockConfirmPanel.GetComponentInChildren<TextMeshProUGUI>().text =
            $"You are unlocking {inst.instrumentId} with {SongManager.Instance.GetConfig().instPrice} instrument tokens.";

        yield return new WaitUntil(() => userConfirmedUnlock != null);

        if (userConfirmedUnlock == true)
        {
            var data = SongManager.Instance.GetGameData();
            int price = SongManager.Instance.GetConfig().instPrice;

            if (data.instrument_token >= price)
            {
                data.instrument_token -= price;
                data.unlocked_instruments.Add(inst.instrumentId);
                SaveInstrumentList();
                SetupInstrumentButton(inst, instButton); // Recheck unlock status
            }
            else
            {
                Debug.LogWarning("Not enough tokens!");
            }
        }

        unlockConfirmPanel.SetActive(false);
    }

    public void OnConfirmUnlock() => userConfirmedUnlock = true;
    public void OnCancelUnlock() => userConfirmedUnlock = false;

    public void SetCurrentInstrumentName(string instrumentName)
    {
        currentInstrumentName.text = instrumentName;
    }
}
