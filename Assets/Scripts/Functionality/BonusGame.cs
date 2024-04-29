using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGame : MonoBehaviour
{

    [SerializeField] private Button btn1;
    [SerializeField] private Button btn2;
    [SerializeField] private Button btn3;
    [SerializeField] private Button btn4;
    [SerializeField] private Button btn5;

    [SerializeField] private ImageAnimation[] imagelist;

    [SerializeField] private Sprite[] Symbol1;
    [SerializeField] private Sprite[] Symbol2;
    [SerializeField] private Sprite[] Symbol3;
    [SerializeField] private Sprite[] Symbol4;
    [SerializeField] private Sprite[] Symbol5;

    [SerializeField] private int[] result;

    void Start()
    {
        Initialize();
        if (btn1) btn1.onClick.RemoveAllListeners();
        if (btn1) btn1.onClick.AddListener(delegate { OnSelectGrave(btn1,imagelist[0]); });

        if (btn2) btn2.onClick.RemoveAllListeners();
        if (btn2) btn2.onClick.AddListener(delegate { OnSelectGrave(btn2,imagelist[1]); });

        if (btn3) btn3.onClick.RemoveAllListeners();
        if (btn3) btn3.onClick.AddListener(delegate { OnSelectGrave(btn3,imagelist[2]); });

        if (btn4) btn4.onClick.RemoveAllListeners();
        if (btn4) btn4.onClick.AddListener(delegate { OnSelectGrave(btn4,imagelist[3]); });

        if (btn5) btn5.onClick.RemoveAllListeners();
        if (btn5) btn5.onClick.AddListener(delegate { OnSelectGrave(btn5,imagelist[4]); });


    }

    private void Initialize()
    {
        for (int i = 0; i < result.Length; i++)
        {
            //ImageAnimation img = imagelist[i].GetComponent<ImageAnimation>();
            PopulateAnimationSprites(imagelist[i], result[i]);
        }
    }

    void OnSelectGrave(Button btn,ImageAnimation img) {

        btn.interactable = false;
        img.StartAnimation();

    }

    private void PopulateAnimationSprites(ImageAnimation animScript, int val)
    {
        print(val);
        switch (val)
        {
            case 0:
                for (int i = 0; i < Symbol1.Length; i++)
                {
                    animScript.textureArray.Add(Symbol1[i]);
                }
                break;
            case 1:
                for (int i = 0; i < Symbol2.Length; i++)
                {
                    animScript.textureArray.Add(Symbol2[i]);
                }
                break;
            case 2:
                for (int i = 0; i < Symbol3.Length; i++)
                {
                    animScript.textureArray.Add(Symbol3[i]);
                }
                break;
            case 3:
                for (int i = 0; i < Symbol4.Length; i++)
                {
                    animScript.textureArray.Add(Symbol4[i]);
                }
                break;
            case 4:
                for (int i = 0; i < Symbol5.Length; i++)
                {
                    animScript.textureArray.Add(Symbol5[i]);
                }
                break;
        }
    }

}
