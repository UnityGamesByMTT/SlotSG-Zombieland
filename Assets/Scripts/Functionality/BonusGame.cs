using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class BonusGame : MonoBehaviour
{

    [SerializeField] private Button[] btn;
    //[SerializeField] private Button btn2;
    //[SerializeField] private Button btn3;
    //[SerializeField] private Button btn4;
    //[SerializeField] private Button btn5;

    [SerializeField] private ImageAnimation[] imagelist;
    [SerializeField] private TMP_Text[] textList;
    [SerializeField] private Sprite[] GameOver;
    [SerializeField] private Sprite[] Symbol2;
    [SerializeField] private Sprite[] Symbol3;
    [SerializeField] private Sprite[] Symbol4;
    [SerializeField] private Sprite[] Symbol5;
    [SerializeField] private GameObject RayCast_Panel;

    [SerializeField] private List<int> result = new List<int>();
    [SerializeField] private List<Button> tempButtonList = new List<Button>();
    int counter = 0;
    [SerializeField] private GameObject bonusGame;
    [SerializeField] private SlotBehaviour slotBehaviour;
    [SerializeField] private AudioController audioManager;
    List<int> randomIndex = new List<int>();


    void Start()
    {
        if (btn[0]) btn[0].onClick.RemoveAllListeners();
        if (btn[0]) btn[0].onClick.AddListener(delegate { OnSelectGrave(btn[0], imagelist[0], textList[0]); });

        if (btn[1]) btn[1].onClick.RemoveAllListeners();
        if (btn[1]) btn[1].onClick.AddListener(delegate { OnSelectGrave(btn[1], imagelist[1], textList[1]); });

        if (btn[2]) btn[2].onClick.RemoveAllListeners();
        if (btn[2]) btn[2].onClick.AddListener(delegate { OnSelectGrave(btn[2], imagelist[2], textList[2]); });

        if (btn[3]) btn[3].onClick.RemoveAllListeners();
        if (btn[3]) btn[3].onClick.AddListener(delegate { OnSelectGrave(btn[3], imagelist[3], textList[3]); });

        if (btn[4]) btn[4].onClick.RemoveAllListeners();
        if (btn[4]) btn[4].onClick.AddListener(delegate { OnSelectGrave(btn[4], imagelist[4], textList[4]); });
    }

    internal void startgame(List<int> bonusResult)
    {
        if (audioManager) audioManager.SwitchBGSound(true);
        if (RayCast_Panel) RayCast_Panel.SetActive(false);
        result = bonusResult;
        Initialize();

        bonusGame.SetActive(true);
    }

    internal void resetgame()
    {
        if (audioManager) audioManager.SwitchBGSound(false);
        bonusGame.SetActive(false);
        slotBehaviour.CheckPopups = false;
    }

    private void Initialize()
    {
        tempButtonList.Clear();
        randomIndex.Clear();
        counter = 0;
        result.Clear();

        foreach (var item in imagelist)
        {
            item.textureArray.Clear();
        }

        foreach (var item in btn)
        {
            item.interactable = true;
        }


        foreach (var item in textList)
        {
            item.transform.localPosition = Vector2.zero;
        }


        for (int i = 0; i < 4; i++)
        {
            randomIndex.Add(i);
        }

        foreach (var item in btn)
        {
            tempButtonList.Add(item);
        }
    }

    void OnSelectGrave(Button btn, ImageAnimation img, TMP_Text text)
    {
        if (RayCast_Panel) RayCast_Panel.SetActive(true);
        btn.interactable = false;
        tempButtonList.Remove(btn);
        if (counter >= (result.Count - 1))
        {

            foreach (var item in tempButtonList)
            {
                item.interactable = false;
            }
        }
        int index = Random.Range(0, randomIndex.Count);
        if (counter >= (result.Count - 1))
        {
            if (audioManager) audioManager.PlayBonusAudio("lose");
            PopulateAnimationSprites(img, -1);
            text.text = "GAME OVER";
            text.gameObject.SetActive(true);
            text.transform.DOLocalMoveY(140, 1f).onComplete = () =>
            {
                text.gameObject.SetActive(false);
            };

            img.StartAnimation();
            Invoke("resetgame", 2f);
            return;
        }
        if (audioManager) audioManager.PlayBonusAudio("win");

        PopulateAnimationSprites(img, randomIndex[index]);
        text.text = "+" + result[counter].ToString("0.00");
        randomIndex.Remove(index);


        text.gameObject.SetActive(true);
        text.transform.DOLocalMoveY(140, 1f).onComplete = () =>
        {

            text.gameObject.SetActive(false);

        };

        img.StartAnimation();
        counter++;
        if (RayCast_Panel) RayCast_Panel.SetActive(false);
    }

    private void PopulateAnimationSprites(ImageAnimation animScript, int val)
    {
        switch (val)
        {
            case -1:
                for (int i = 0; i < GameOver.Length; i++)
                {
                    animScript.textureArray.Add(GameOver[i]);
                }
                break;
            case 0:
                for (int i = 0; i < Symbol2.Length; i++)
                {
                    animScript.textureArray.Add(Symbol2[i]);
                }
                break;
            case 1:
                for (int i = 0; i < Symbol3.Length; i++)
                {
                    animScript.textureArray.Add(Symbol3[i]);
                }
                break;
            case 2:
                for (int i = 0; i < Symbol4.Length; i++)
                {
                    animScript.textureArray.Add(Symbol4[i]);
                }
                break;
            case 3:
                for (int i = 0; i < Symbol5.Length; i++)
                {
                    animScript.textureArray.Add(Symbol5[i]);
                }
                break;
        }
    }

}
