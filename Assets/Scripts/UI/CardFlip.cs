using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardFlip : MonoBehaviour
{
    [SerializeField]
    internal Sprite Card_Sprite;
    [SerializeField]
    private Transform Card_RT;
    [SerializeField]
    internal Button Card_Button;
    [SerializeField]
    private Image Card_Image;
    internal bool once = false;

    [SerializeField] private GambleController gambleController;

    private void Start()
    {
        if (Card_Button) Card_Button.onClick.RemoveAllListeners();
        if (Card_Button) Card_Button.onClick.AddListener(delegate { FlipMyObject(); });
    }

    internal bool FlipMyObject()
    {
        if (!once)
        {
            if (!gambleController.card_flipped)
            {

                gambleController.card_flipped = true;
                gambleController.allcardsTemp.Remove(this);
                foreach (var item in gambleController.allcardsTemp)
                {
                    item.Card_Button.interactable = false;
                }

            }
            Card_RT.localEulerAngles = new Vector3(0, 180, 0);
            Card_Button.interactable = false;
            once = true;
            Card_RT.DORotate(new Vector3(0, 0, 0), 1, RotateMode.FastBeyond360);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                if (Card_Image) Card_Image.sprite = Card_Sprite;
            });
            return true;
        }
        else return false;
    }
}
