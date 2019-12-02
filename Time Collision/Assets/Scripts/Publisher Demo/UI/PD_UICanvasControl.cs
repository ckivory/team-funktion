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
    public Text playerMass;
    public Text selectedCount;
    List<Vector3> ImageLoc;
    public List<Sprite> BlueSprites;
    public List<Sprite> GreySprites;
    public Text PlayerMass;
    PDPlayerController Controller;
    List<int> Inventory;
    float lastType;
    float alarm;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            try
            {
                ImageLoc.Add(Images[i].transform.position);
            }
            catch (System.Exception)
            {
                Debug.Log("Error in PD UI Canvas Control");
            }
        }

        Controller = Player.GetComponent<PDPlayerController>();
        Inventory = Controller.inventory;
        alarm = 2.0f;
        lastType = 0;


    }

    // Update is called once per frame
    void Update()
    {
        alarm -= Time.deltaTime;
        PlayerMass.text = "PlayerMass: " + Controller.playerMass;
        if (Player == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if(alarm <= 0)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].CrossFadeAlpha(0, 2, false);
            }
        }

        if(lastType != Controller.selectedProp)
        {
            PopUp();
            lastType = Controller.selectedProp;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        Images[2].sprite = BlueSprites[Controller.selectedProp];

        if (Controller.selectedProp - 1 < 0)
        {
            Images[1].sprite = GreySprites[Inventory.Count - 1];
        }
        else
        {
            Images[1].sprite = GreySprites[Controller.selectedProp - 1];
        }

        if (Controller.selectedProp - 2 < 0)
        {
            Images[0].sprite = GreySprites[Inventory.Count - 2];
        }
        else
        {
            Images[0].sprite = GreySprites[Controller.selectedProp - 2];
        }

        if (Controller.selectedProp + 1 > Inventory.Count-1)
        {
            Images[3].sprite = GreySprites[0];
        }
        else
        {
            Images[3].sprite = GreySprites[Controller.selectedProp + 1];
        }

        if (Controller.selectedProp + 2 > Inventory.Count-1)
        {
            Images[4].sprite = GreySprites[1];
        }
        else
        {
            Images[4].sprite = GreySprites[Controller.selectedProp + 2];
        }


    void updateImage()
    {
        selectedType = Player.GetComponent<PDPlayerController>().selectedProp;
        int l1 = LeftFind(selectedType);
        int l2 = LeftFind(l1);
        int r1 = RightFind(selectedType);
        int r2 = RightFind(r1);

    }

    public void PopUp()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            Images[i].CrossFadeAlpha(1, 0.1f, false);
        }
        alarm = 2.0f;
    }

}
