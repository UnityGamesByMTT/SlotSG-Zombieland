using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GambleController : MonoBehaviour
{
    [SerializeField] private GameObject gamble_game;
    [SerializeField] private Button doubleButton;
    [SerializeField] private Button collectButton;
    [SerializeField] private Button gambleButton;
    [SerializeField] private Button gambleCloseButton;
    [SerializeField] private SocketIOManager socketManager;
    [SerializeField] private AudioController audioController;

    [SerializeField] private List<CardFlip> allcards = new List<CardFlip>();
    [SerializeField] internal List<CardFlip> allcardsTemp = new List<CardFlip>();

    [SerializeField] private SlotBehaviour slotController;
    [SerializeField] private Sprite[] cardSpriteList;
    [SerializeField] private Sprite cardCover;
    [SerializeField] internal List<Sprite> tempSpriteList = new List<Sprite>();
    [SerializeField] private Image DealerCard;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image slider;

    [SerializeField] private TMP_Text betText;
    [SerializeField] private TMP_Text bankText;
    [SerializeField] private TMP_Text doubleToText;

    internal bool gambleStart = false;
    internal bool card_flipped;

    private void Start()
    {
        if (doubleButton) doubleButton.onClick.RemoveAllListeners();
        if (doubleButton) doubleButton.onClick.AddListener(StartGamblegame);
        if (gambleButton) gambleButton.onClick.AddListener(OnGambleStart);
        if (collectButton) collectButton.onClick.AddListener(OnGameOver);
        if (gambleCloseButton) gambleCloseButton.onClick.AddListener(OnGameOver);

        toggleGambleButton(false);
    }

    internal void toggleGambleButton(bool toggle)
    {

        gambleButton.interactable = toggle;
    }

    void StartGamblegame()
    {
        if (audioController) audioController.PlayButtonAudio();
        if (gamble_game) gamble_game.SetActive(true);
        PickRandomCard();
        StartCoroutine(LoadingRoutine());
        StartCoroutine(GambleCoroutine());
    }

    void OnGambleStart()
    {
        gambleButton.interactable = true;
        SetText(socketManager.resultData.WinAmout);

        StartGamblegame();
    }
    IEnumerator GambleCoroutine()
    {


        ToggleButtonGrp(false);
        for (int i = 0; i < allcards.Count; i++)
        {

            allcardsTemp.Add(allcards[i]);
        }

        for (int i = 0; i < allcardsTemp.Count; i++)
        {
            allcardsTemp[i].once = false;
            allcardsTemp[i].Card_Button.interactable = true;
            allcardsTemp[i].Card_Button.image.sprite = cardCover;
            card_flipped = false;

        }

        socketManager.OnGamble();

        yield return new WaitUntil(() => socketManager.isResultdone);
        foreach (var item in allcardsTemp)
        {
            item.Card_Button.interactable = true;

        }
        if (socketManager.gambleData.totalWinningAmount > 0)
        {
            for (int i = 0; i < allcardsTemp.Count; i++)
            {
                allcardsTemp[i].Card_Sprite = tempSpriteList[tempSpriteList.Count - 1];


            }
                tempSpriteList.RemoveAt(tempSpriteList.Count - 1);

        }
        else {
            for (int i = 0; i < allcardsTemp.Count; i++)
            {
                allcardsTemp[i].Card_Sprite = tempSpriteList[0];

            }
                tempSpriteList.RemoveAt(0);

        }
        gambleStart = true;

        yield return FlipAllCard();
        collectButton.interactable = true;

        allcardsTemp.Clear();
        SetText(socketManager.gambleData.totalWinningAmount);
        slotController.updateBalance();
        ToggleButtonGrp(true);
        if (socketManager.gambleData.totalWinningAmount == 0)
        {
            ToggleButtonGrp(false);
            OnGameOver();
        }

    }

    IEnumerator FlipAllCard()
    {
        yield return new WaitUntil(() => card_flipped);
        yield return new WaitForSeconds(1.5f);
        List<CardFlip> tempCardList = new List<CardFlip>();

        for (int i = 0; i < allcardsTemp.Count; i++)
        {
            allcardsTemp[i].Card_Sprite = tempSpriteList[i];
            yield return new WaitUntil(() => allcardsTemp[i].FlipMyObject());
        }
    }


    IEnumerator Collectroutine()
    {
        ToggleButtonGrp(false);
        gambleStart = false;
        socketManager.OnCollect();
        yield return new WaitUntil(() => socketManager.isResultdone);
        yield return new WaitForSeconds(2f);
        slotController.updateBalance();
        toggleGambleButton(false);
        ToggleButtonGrp(true);
        SetText(0);
        if (gamble_game) gamble_game.SetActive(false);


    }

    void OnGameOver()
    {
        StartCoroutine(Collectroutine());

    }

    void PickRandomCard()
    {
        int maxlength = cardSpriteList.Length / 5;

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(maxlength * i, maxlength * (i + 1));
            tempSpriteList.Add(cardSpriteList[index]);
        }


    }

    void ToggleButtonGrp(bool toggle)
    {
        if (doubleButton) doubleButton.interactable = toggle;
        if (collectButton) collectButton.interactable = toggle;
        if (gambleCloseButton) gambleCloseButton.interactable = toggle;
    }

    void SetText(double amount)
    {

        betText.text = amount.ToString();
        bankText.text = amount.ToString();
        doubleToText.text = (amount * 2).ToString();

    }

    IEnumerator LoadingRoutine()
    {
        slider.fillAmount = 1;
        loadingScreen.SetActive(true);

        yield return new WaitUntil(() => gambleStart);
        while (slider.fillAmount > 0)
        {
            slider.fillAmount -= Time.deltaTime;
            if (slider.fillAmount <= 0) break;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
    }
}
