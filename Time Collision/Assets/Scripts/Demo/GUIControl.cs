using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIControl : MonoBehaviour
{
    public List<Sprite> PropSprites;
    public GameObject Player;
    public Image Middle;
    public Image Left;
    public Image right;
    public Text MidNum;
    public Text LeftNum;
    public Text RightNum;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
    }

    void UpdateGUI()
    {
        int selectedType = Player.GetComponent<VSPlayerController>().selectedProp;
        int midNum = Player.GetComponent<VSPlayerController>().inventory[selectedType];
        int leftType = LeftType(selectedType);
        int rightType = RightType(selectedType);
        Middle.sprite = PropSprites[selectedType];
        Left.sprite = PropSprites[leftType];
        right.sprite = PropSprites[rightType];
        MidNum.text = ""+midNum;


    }

    int LeftType(int selectedType)
    {
        if (selectedType > 0)
        {
            return selectedType - 1;
        }
        else
        {
            return PropSprites.Count-1;
        }
    }

    int RightType(int selectedType)
    {
        if (selectedType < PropSprites.Count-1)
        {
            return selectedType + 1;
        }
        else
        {
            return 0;
        }
    }
}
