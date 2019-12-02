using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PD_UICanvasControl : MonoBehaviour
{
    //public List<GameObject> Icons;
    public GameObject Player;
    public GameObject Arrow;
    public List<Image> Images;
    public List<Text> Texts;
    public List<Sprite> Sprites;

    public Image statBackground;
    public Text playerMass;
    public Text selectedCount;
    public Image mineImage;
    public Text mineAmount;

    List<Vector3> ImageLoc;
    List<int> Inventory;
    int selectedType;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i<Images.Count;i++)
        {
            try
            {
                ImageLoc.Add(Images[i].transform.position);
            }
            catch(System.Exception)
            {
                Debug.Log("Error in PD UI Canvas Control");
            }
        }

        {
            //for(int i = 0; i < Icons.Count; i++)
            //{
            //    Icons[i].GetComponent<PD_UIIconControl>().Player = Player;
            //}
        }

        Color temp = statBackground.color;
        temp.a = 0.5f;
        statBackground.color = temp;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Arrow.active)
        {
            Destroy(gameObject);
        }
        Inventory = Player.GetComponent<PDPlayerController>().inventory;
        int total;
        total=UpdateDisplay();
        if (total > 0)
        {
            updateImage();
        }

        mineAmount.text = ""+Inventory[6];
        if (Inventory[6]<1)
        {
            ToggleVisible(mineImage, mineAmount, false);
        } else ToggleVisible(mineImage, mineAmount, true);
        playerMass.text = "" + Player.GetComponent<PDPlayerController>().playerMass*100;
        selectedCount.text = "" + Player.GetComponent<PDPlayerController>().selectedCount;
    }

    void updateImage()
    {
        selectedType = Player.GetComponent<PDPlayerController>().selectedProp;
        int l1 = LeftFind(selectedType);
        int l2 = LeftFind(l1);
        int r1 = RightFind(selectedType);
        int r2 = RightFind(r1);

        //if (l1 < 0)
        //{
        //    l1 = Sprites.Count + l1;
        //}
        //if (l2 < 0)
        //{
        //    l2 = Sprites.Count + l2;
        //}
        //if (r1 > Sprites.Count - 1)
        //{
        //    r1 = r1 - Sprites.Count;
        //}
        //if (r2 > Sprites.Count - 1)
        //{
        //    r2 = r2 - Sprites.Count;
        //}
        

        Images[2].sprite = Sprites[selectedType];
        Images[1].sprite = Sprites[l1];
        Images[0].sprite = Sprites[l2];
        Images[3].sprite = Sprites[r1];
        Images[4].sprite = Sprites[r2];
        
        
        Texts[2].text = "" + Inventory[selectedType];
        Texts[1].text = "" + Inventory[l1];
        Texts[0].text = "" + Inventory[l2];
        Texts[3].text = "" + Inventory[r1];
        Texts[4].text = "" + Inventory[r2];
    }
    
    int LeftFind(int start)
    {
        int end = start ;
        do
        {
            end -= 1;
            if (end < 0) end += Sprites.Count;
        } while ((end == 6) || (Inventory[end] == 0));
            return end;
    }

    int RightFind(int start)
    {
        int end = start ;
        do
        {
            end += 1;
            if (end > Sprites.Count - 1) end -= Sprites.Count;
        } while ((end == 6) || (Inventory[end] == 0)) ;
            return end;
    }
    int UpdateDisplay()
    {
        int total = 0;
        for (int index=0; index<Inventory.Count;index++)
        {
            if ((Inventory[index] > 0) && (index != 6))
            {
                total += 1;
            }
        }
        if (total==0)
        {
            ToggleVisible(Images[0], Texts[0], false);
            ToggleVisible(Images[1], Texts[1], false);
            ToggleVisible(Images[2], Texts[2], false);
            ToggleVisible(Images[3], Texts[3], false);
            ToggleVisible(Images[4], Texts[4], false);
        }
        if (total==1)
        {
            ToggleVisible(Images[0], Texts[0], false);
            ToggleVisible(Images[1], Texts[1], false);
            ToggleVisible(Images[2], Texts[2], true);
            ToggleVisible(Images[3], Texts[3], false);
            ToggleVisible(Images[4], Texts[4], false);
        }
        if ((total>1)&&(total<=3))
        {
            ToggleVisible(Images[0], Texts[0], false);
            ToggleVisible(Images[1], Texts[1], true);
            ToggleVisible(Images[2], Texts[2], true);
            ToggleVisible(Images[3], Texts[3], true);
            ToggleVisible(Images[4], Texts[4], false);
        }
        if ((total > 3))
        {
            ToggleVisible(Images[0], Texts[0], true);
            ToggleVisible(Images[1], Texts[1], true);
            ToggleVisible(Images[2], Texts[2], true);
            ToggleVisible(Images[3], Texts[3], true);
            ToggleVisible(Images[4], Texts[4], true);
        }
        return total;
    }

    void ToggleVisible(Image image, Text text, bool visible)
    {
        Color temp1 = image.color;
        Color temp2 = text.color;
        if (visible)
        {
            temp1.a = 1;
            temp2.a = 1;
        } else {
            temp1.a = 0;
            temp2.a = 0;
        }
        image.color = temp1;
        text.color = temp2;
    }
}
