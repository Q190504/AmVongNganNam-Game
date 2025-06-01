using System.Text.RegularExpressions;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class InstrumentUIManager : MonoBehaviour
{
    public GameObject instrumentButtonPrefab;
    private List<InstrumentDataSO> instrumentList;
    private List<GameObject> instrumentButtonList;
    public Transform contentPanel;
    [SerializeField] private GameObject unlockConfirmPanel;
    [SerializeField] private GameObject canUnlockButtonPanel;
    [SerializeField] private GameObject unableUnlockButtonPanel;

    [SerializeField] private VoidPublisherSO unlockEvent;

    [Header("Sprites")]
    [SerializeField] private Sprite selectedButtonSprite;
    [SerializeField] private Sprite nonselectedButtonSprite;

    private bool? userConfirmedUnlock;

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

            if (SetupInstrumentButton(inst, instButton))
            {
                hasNewInst = true;
            }            
        }

        if (hasNewInst)
        {
            InstrumentManager.Instance.SaveInstrumentList();
        }
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

        var data = SongManager.Instance.GetGameData();
        int price = SongManager.Instance.GetConfig().instPrice;
        if (data.instrument_token >= price)
        {
            unlockConfirmPanel.GetComponentInChildren<TextMeshProUGUI>().text =
            $"Bạn đang sử dụng {SongManager.Instance.GetConfig().instPrice} Đồng Đàn để mở khóa nhạc cụ {inst.instrumentName}.";
            canUnlockButtonPanel.SetActive(true);
            unableUnlockButtonPanel.SetActive(false);
        }
        else
        {
            unlockConfirmPanel.GetComponentInChildren<TextMeshProUGUI>().text =
            $"Bạn cần thêm {SongManager.Instance.GetConfig().instPrice - data.instrument_token} Đồng Đàn để mở khóa nhạc cụ {inst.instrumentName}.";
            canUnlockButtonPanel.SetActive(false);
            unableUnlockButtonPanel.SetActive(true);
        }
        
        yield return new WaitUntil(() => userConfirmedUnlock != null);

        if (userConfirmedUnlock == true)
        {
            

            if (data.instrument_token >= price)
            {
                data.instrument_token -= price;
                data.unlocked_instruments.Add(inst.instrumentId);
                InstrumentManager.Instance.SaveInstrumentList();
                SetupInstrumentButton(inst, instButton); // Recheck unlock status
                unlockEvent.RaiseEvent();
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

    public void UpdateInstrumentButtons(string instrumentName)
    {
        foreach (GameObject instrumentButton in instrumentButtonList)
        {
            TMP_Text buttonText = instrumentButton.GetComponentInChildren<TMP_Text>();
            if (buttonText.text == instrumentName)
                instrumentButton.GetComponent<Image>().sprite = selectedButtonSprite;
            else
                instrumentButton.GetComponent<Image>().sprite = nonselectedButtonSprite;
        }
    }
}
