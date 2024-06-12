using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{

    [Header("Menu UI")]
    [SerializeField]
    private Button Info_Button;
    [SerializeField] private Button SoundButton;

    [Header("Popus UI")]
    [SerializeField]
    private GameObject MainPopup_Object;

    [Header("info Popup")]
    [SerializeField]
    private GameObject PaytablePopup_Object;
    [SerializeField]
    private Button PaytableExit_Button;
    [SerializeField]
    private Button Next_Button;
    [SerializeField]
    private Button Previous_Button;
    private int paginationCounter = 1;
    [SerializeField] private GameObject[] PageList;
    [SerializeField] private Button[] paginationButtonGrp;
    [SerializeField] private Button Infoback_button;


    [Header("Settings Popup")]
    [SerializeField] private Button Setting_button;
    [SerializeField] private Button SettingExit_button;
    [SerializeField] private Button Setting_back_button;
    [SerializeField] private GameObject Setting_panel;
    [SerializeField] private Slider Sound_slider;
    [SerializeField] private Slider Music_slider;
    [SerializeField] private Button Notification_button;
    [SerializeField] private GameObject Notification_on;
    [SerializeField] private GameObject Notification_off;
    private bool isSOundOn = true;
    private bool isNotifyOff = false;

    [Header("add coin popup")]
    [SerializeField] private Button AddCoin_button;
    [SerializeField] private Button AddCoinExit_button;
    [SerializeField] private Button AddCoin_back_button;
    [SerializeField] private GameObject AddCoin_panel;

    [Header("gamble game")]
    [SerializeField] private Button Gamble_button;
    [SerializeField] private Button GambleExit_button;
    [SerializeField] private GameObject Gamble_game;

    [Header("Audio")]
    [SerializeField] private AudioController audioController;

    [Header("Free Spin")]
    [SerializeField] private GameObject freeSpin;
    [SerializeField] private TMP_Text freeSpin_text;
    private int FreeSpins;

    [SerializeField]
    private Button GameExit_Button;

    [SerializeField]
    private SlotBehaviour slotManager;

    [SerializeField]
    private TMP_Text[] SymbolsText;

    private void Start()
    {
        if (Info_Button) Info_Button.onClick.RemoveAllListeners();
        if (Info_Button) Info_Button.onClick.AddListener(delegate { OpenPopup(PaytablePopup_Object); });

        if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
        if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });

        if (Next_Button) Next_Button.onClick.RemoveAllListeners();
        if (Next_Button) Next_Button.onClick.AddListener(delegate { TurnPage(true); });

        if (Previous_Button) Previous_Button.onClick.RemoveAllListeners();
        if (Previous_Button) Previous_Button.onClick.AddListener(delegate { TurnPage(false); });

        if (Previous_Button) Previous_Button.interactable = false;

        if (paginationButtonGrp[0]) paginationButtonGrp[0].onClick.RemoveAllListeners();
        if (paginationButtonGrp[0]) paginationButtonGrp[0].onClick.AddListener(delegate { GoToPage(0); });

        if (paginationButtonGrp[1]) paginationButtonGrp[1].onClick.RemoveAllListeners();
        if (paginationButtonGrp[1]) paginationButtonGrp[1].onClick.AddListener(delegate { GoToPage(1); });

        if (paginationButtonGrp[2]) paginationButtonGrp[2].onClick.RemoveAllListeners();
        if (paginationButtonGrp[2]) paginationButtonGrp[2].onClick.AddListener(delegate { GoToPage(2); });

        if (paginationButtonGrp[3]) paginationButtonGrp[3].onClick.RemoveAllListeners();
        if (paginationButtonGrp[3]) paginationButtonGrp[3].onClick.AddListener(delegate { GoToPage(3); });

        if (Infoback_button) Infoback_button.onClick.RemoveAllListeners();
        if (Infoback_button) Infoback_button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });


        if (Setting_button) Setting_button.onClick.RemoveAllListeners();
        if (Setting_button) Setting_button.onClick.AddListener(delegate { OpenPopup(Setting_panel); });

        if (Sound_slider) Sound_slider.onValueChanged.RemoveAllListeners();
        if (Sound_slider) Sound_slider.onValueChanged.AddListener(delegate { ChangeSound(); });

        if (Music_slider) Music_slider.onValueChanged.RemoveAllListeners();
        if (Music_slider) Music_slider.onValueChanged.AddListener(delegate { ChangeMusic(); });

        if (Notification_button) Notification_button.onClick.RemoveAllListeners();
        if (Notification_button) Notification_button.onClick.AddListener(ToggleNotification);

        if (SettingExit_button) SettingExit_button.onClick.RemoveAllListeners();
        if (SettingExit_button) SettingExit_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

        if (Setting_back_button) Setting_back_button.onClick.RemoveAllListeners();
        if (Setting_back_button) Setting_back_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

        ToggleNotification();

        if (AddCoin_button) AddCoin_button.onClick.RemoveAllListeners();
        if (AddCoin_button) AddCoin_button.onClick.AddListener(delegate { OpenPopup(AddCoin_panel); });

        if (AddCoinExit_button) AddCoinExit_button.onClick.RemoveAllListeners();
        if (AddCoinExit_button) AddCoinExit_button.onClick.AddListener(delegate { ClosePopup(AddCoin_panel); });

        if (AddCoin_back_button) AddCoin_back_button.onClick.RemoveAllListeners();
        if (AddCoin_back_button) AddCoin_back_button.onClick.AddListener(delegate { ClosePopup(AddCoin_panel); });


        if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
        if (GameExit_Button) GameExit_Button.onClick.AddListener(CallOnExitFunction);

    }

    private void CallOnExitFunction()
    {
        slotManager.CallCloseSocket();
        Application.ExternalCall("window.parent.postMessage", "onExit", "*");
    }

    internal void InitialiseUIData(string SupportUrl, string AbtImgUrl, string TermsUrl, string PrivacyUrl, Paylines symbolsText)
    {
        PopulateSymbolsPayout(symbolsText);
    }

    private void PopulateSymbolsPayout(Paylines paylines)
    {
            print("payline :"+paylines.symbols.Count);
        for (int i = 0; i < paylines.symbols.Count; i++)
        {
            print(i);
            if (i < SymbolsText.Length)
            {
                string text = null;
                if (paylines.symbols[i].multiplier._5x != 0)
                {
                    text += "<color=yellow>5x</color> - " + paylines.symbols[i].multiplier._5x;
                }
                if (paylines.symbols[i].multiplier._4x != 0)
                {
                    text += "\n<color=yellow>4x</color> - " + paylines.symbols[i].multiplier._4x;
                }
                if (paylines.symbols[i].multiplier._3x != 0)
                {
                    text += "\n<color=yellow>3x</color> - " + paylines.symbols[i].multiplier._3x;
                }
                if (paylines.symbols[i].multiplier._2x != 0)
                {
                    text += "\n<color=yellow>2x</color> - " + paylines.symbols[i].multiplier._2x;
                }
                if (SymbolsText[i]) SymbolsText[i].text = text;
            }
        }
    }


    internal void ShowFreeSpinData(int spins)
    {
        FreeSpins = spins;
        if (freeSpin) freeSpin.SetActive(true);
        if (freeSpin_text) freeSpin_text.text += "\n" + spins.ToString();
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        DOVirtual.DelayedCall(1f, () =>
        {
            if (freeSpin) freeSpin.SetActive(false);
            if (MainPopup_Object) MainPopup_Object.SetActive(false);
            if (freeSpin_text) freeSpin_text.text += "0";

        });
    }

    private void OpenPopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
    }

    private void ClosePopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();

        if (Popup) Popup.SetActive(false);
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
    }

    private void TurnPage(bool type)
    {
        if (audioController) audioController.PlayButtonAudio();

        if (type)
            paginationCounter++;
        else
            paginationCounter--;


        GoToPage(paginationCounter - 1);


    }

    private void GoToPage(int index)
    {

        paginationCounter = index + 1;

        paginationCounter = Mathf.Clamp(paginationCounter, 1, 4);

        if (Next_Button) Next_Button.interactable = !(paginationCounter >= 4);

        if (Previous_Button) Previous_Button.interactable = !(paginationCounter <= 1);

        for (int i = 0; i < PageList.Length; i++)
        {
            PageList[i].SetActive(false);
        }

        for (int i = 0; i < paginationButtonGrp.Length; i++)
        {
            paginationButtonGrp[i].interactable = true;
            paginationButtonGrp[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        PageList[paginationCounter - 1].SetActive(true);
        paginationButtonGrp[paginationCounter - 1].interactable = false;
        paginationButtonGrp[paginationCounter - 1].transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ChangeSound()
    {
        audioController.ChangeVolume("wl", Sound_slider.value);
        audioController.ChangeVolume("button", Sound_slider.value);

    }

    private void ChangeMusic()
    {
        audioController.ChangeVolume("bg", Music_slider.value);

    }

    private void ToggleNotification()
    {
        isNotifyOff = !isNotifyOff;

        Notification_on.SetActive(isNotifyOff);
        Notification_off.SetActive(!isNotifyOff);


    }
}
