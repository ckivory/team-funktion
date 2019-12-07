using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PD_UICanvasControl : MonoBehaviour
{
    //public List<GameObject> Icons;
    public GameObject Player;
    public List<GameObject> OtherPlayers;
    public GameObject Arrow;
    public List<Image> Images;
    public List<Text> Texts;
    public List<Sprite> Sprites;

    public Image statBackground;
    public Text playerMass;
    public Text selectedCount;
    public Image mineImage;
    public Text mineAmount;
    public Text CM;
    public Text CPS;
    public Text WinLose;
    public Image RedOverlay;
    public Image ControlScheme;
    //public Text Hint;

    List<Vector3> ImageLoc;
    List<int> Inventory;
    PDPlayerController Controller;
    int selectedType;
    float UIalarm;
    int preType;
    float winTimer;
    float hitTimer;
    bool winning;       // Added by Carson to fix bug with ties.
    public float controlAlpha;

    // Start is called before the first frame update
    void Start()
    {
        winTimer = 5f;
        hitTimer = 0f;
        winning = false;
        WinLose.enabled = false;
        preType = 0;
        UIalarm = 2.0f;
        Controller = Player.GetComponent<PDPlayerController>();
        RedOverlay.enabled = false;

        for (int i = 0; i < Images.Count; i++)
        {
            try
            {
                ImageLoc.Add(Images[i].transform.position);
            }
            catch (System.Exception e)
            {
                //Debug.Log("Error in PD UI Canvas Control");
            }
        }

        {
            //for(int i = 0; i < Icons.Count; i++)
            //{
            //    Icons[i].GetComponent<PD_UIIconControl>().Player = Player;
            //}
        }

        //initial alpha setup
        Color temp;
        temp = statBackground.color;
        temp.a = 0.5f;
        statBackground.color = temp;
        foreach (Text amount in Texts)
        {
            temp = amount.color;
            temp.a = 0f;
            amount.color = temp;
        }
        foreach (Image icon in Images)
        {
            temp = icon.color;
            temp.a = 0f;
            icon.color = temp;
        }
    }

    // Modified by Carson to fix ties and handle post-win player death
    void Update()
    {
        int aliveCount = 0;
        foreach (GameObject player in OtherPlayers)
        {
            if (player.GetComponent<PDPlayerController>().alive)
            {
                aliveCount += 1;
            }
        }

        if (winning)
        {
            Win();
        }
        else
        {
            if (!Player.GetComponent<PDPlayerController>().alive)
            {
                if(aliveCount > 0)
                {
                    lose();
                }
            }
            else
            {
                if (aliveCount == 0)
                {
                    winning = true;
                    Win();
                    StartCoroutine(WinSequence());
                }
            }
            Inventory = Player.GetComponent<PDPlayerController>().inventory;
            int total;
            total = UpdateDisplay();
            if (total > 0)
            {
                updateImage();
            }

            mineAmount.text = "" + Inventory[6];
            if (Inventory[6] < 1)
            {
                ToggleVisible(mineImage, mineAmount, false);
            }
            else ToggleVisible(mineImage, mineAmount, true);
            playerMass.text = "" + Player.GetComponent<PDPlayerController>().playerMass * 100;
            selectedCount.text = "" + Player.GetComponent<PDPlayerController>().selectedCount;
            UIalarm -= Time.deltaTime;
            Fade();
        }

        if (Controller.hit)
        {
            RedOverlay.enabled = true;
            RedOverlay.CrossFadeAlpha(0f, 1f, false);
            hitTimer += Time.deltaTime;
        }

        if (hitTimer >= 1f)
        {
            RedOverlay.CrossFadeAlpha(0f, 0f, false);
            RedOverlay.enabled = false;
            Controller.hit = false;
            hitTimer = 0f;
        }
        

        if (Controller.ShowControl)
        {
            ControlScheme.enabled = true;
            ControlScheme.CrossFadeAlpha(controlAlpha, 0f, false);

            //Hint.enabled = true;
            //Hint.CrossFadeAlpha(1f, 0f, false);
        }
        else
        {
            ControlScheme.enabled = false;

            //Hint.enabled = false;
        }
    }

    void updateImage()
    {
        selectedType = Player.GetComponent<PDPlayerController>().selectedProp;
        int l1 = LeftFind(selectedType);
        int l2 = LeftFind(l1);
        int r1 = RightFind(selectedType);
        int r2 = RightFind(r1);

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
        int end = start;
        do
        {
            end -= 1;
            if (end < 0) end += Sprites.Count;
        } while ((end == 6) || (Inventory[end] == 0));
        return end;
    }

    int RightFind(int start)
    {
        int end = start;
        do
        {
            end += 1;
            if (end > Sprites.Count - 1) end -= Sprites.Count;
        } while ((end == 6) || (Inventory[end] == 0));
        return end;
    }

    int UpdateDisplay()
    {
        int total = 0;
        for (int index = 0; index < Inventory.Count; index++)
        {
            if ((Inventory[index] > 0) && (index != 6))
            {
                total += 1;
            }
        }
        if (total == 0)
        {
            ToggleVisible(Images[0], Texts[0], false);
            ToggleVisible(Images[1], Texts[1], false);
            ToggleVisible(Images[2], Texts[2], false);
            ToggleVisible(Images[3], Texts[3], false);
            ToggleVisible(Images[4], Texts[4], false);
        }
        if (total == 1)
        {
            ToggleVisible(Images[0], Texts[0], false);
            ToggleVisible(Images[1], Texts[1], false);
            ToggleVisible(Images[2], Texts[2], true);
            ToggleVisible(Images[3], Texts[3], false);
            ToggleVisible(Images[4], Texts[4], false);
        }
        if ((total > 1) && (total <= 3))
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
            temp1.a = 0.8f;
            temp2.a = 0.8f;
            image.CrossFadeAlpha(1, 0.5f, false);
            //image.CrossFadeAlpha(1, 0.5f, false);
            //text.CrossFadeAlpha(1, 0.5f, false);
        }
        else
        {
            temp1.a = 0f;
            temp2.a = 0f;
            //image.CrossFadeAlpha(0, 0.5f, false);
            //text.CrossFadeAlpha(0, 0.5f, false);
        }
        image.color = temp1;
        text.color = temp2;
    }

    void Fade()
    {
        if (UIalarm <= 0)
        {
            Images[0].CrossFadeAlpha(0, 0.5f, false);
            Images[1].CrossFadeAlpha(0, 0.5f, false);
            Images[3].CrossFadeAlpha(0, 0.5f, false);
            Images[4].CrossFadeAlpha(0, 0.5f, false);

            Texts[0].CrossFadeAlpha(0, 0.5f, false);
            Texts[1].CrossFadeAlpha(0, 0.5f, false);
            Texts[3].CrossFadeAlpha(0, 0.5f, false);
            Texts[4].CrossFadeAlpha(0, 0.5f, false);

            //mineImage.CrossFadeAlpha(0.5f, 0.5f, false);
            //mineAmount.CrossFadeAlpha(0.5f, 0.5f, false);
            //statBackground.CrossFadeAlpha(0, 0.5f, false);
            //playerMass.CrossFadeAlpha(0, 0.5f, false);
            //selectedCount.CrossFadeAlpha(0, 0.5f, false);
            //CM.CrossFadeAlpha(0, 0.5f, false);
            //CPS.CrossFadeAlpha(0, 0.5f, false);
        }

        if (preType != Player.GetComponent<PDPlayerController>().selectedProp)
        {
            Images[0].CrossFadeAlpha(1, 0.5f, false);
            Images[1].CrossFadeAlpha(1, 0.5f, false);
            Images[3].CrossFadeAlpha(1, 0.5f, false);
            Images[4].CrossFadeAlpha(1, 0.5f, false);

            Texts[0].CrossFadeAlpha(1, 0.5f, false);
            Texts[1].CrossFadeAlpha(1, 0.5f, false);
            Texts[3].CrossFadeAlpha(1, 0.5f, false);
            Texts[4].CrossFadeAlpha(1, 0.5f, false);

            //mineImage.CrossFadeAlpha(1, 0.5f, false);
            //mineAmount.CrossFadeAlpha(1, 0.5f, false);
            //statBackground.CrossFadeAlpha(1, 0.5f, false);
            //playerMass.CrossFadeAlpha(1, 0.5f, false);
            //selectedCount.CrossFadeAlpha(1, 0.5f, false);
            //CM.CrossFadeAlpha(1, 0.5f, false);
            //CPS.CrossFadeAlpha(1, 0.5f, false);

            preType = Player.GetComponent<PDPlayerController>().selectedProp;
            UIalarm = 5f;
        }
    }

    private IEnumerator WinSequence()
    {
        float changeSpeed = 0.25f;
        while (winTimer>0f)
        {
            WinLose.color = Color.red;
            yield return new WaitForSeconds(changeSpeed);
            WinLose.color = Color.green;
            yield return new WaitForSeconds(changeSpeed);
            WinLose.color = Color.blue;
            yield return new WaitForSeconds(changeSpeed);
        }
    }

    void Win()
    {
        WinLose.text = "YOU WIN!";
        WinLose.enabled = true;
        statBackground.enabled = false;
        playerMass.enabled = false;
        selectedCount.enabled = false;
        mineImage.enabled = false;
        mineAmount.enabled = false;
        CM.enabled = false;
        CPS.enabled = false;

        foreach (Image icon in Images)
        {
            icon.enabled = false;
        }
        foreach (Text amount in Texts)
        {
            amount.enabled = false;
        }

        winTimer -= Time.deltaTime;

        if (winTimer <= 0f)
        {
            SceneManager.LoadScene("Title Screen");
        }
    }

    void lose()
    {
        WinLose.text = "GAME OVER";
        WinLose.enabled = true;
        statBackground.enabled = false;
        playerMass.enabled = false;
        selectedCount.enabled = false;
        mineImage.enabled = false;
        mineAmount.enabled = false;
        CM.enabled = false;
        CPS.enabled = false;
        foreach (Image icon in Images)
        {
            icon.enabled = false;
        }
        foreach (Text amount in Texts)
        {
            amount.enabled = false;
        }
    }

    void tie()
    {
        WinLose.text = "Tie Game";
        WinLose.enabled = true;
        statBackground.enabled = false;
        playerMass.enabled = false;
        selectedCount.enabled = false;
        mineImage.enabled = false;
        mineAmount.enabled = false;
        CM.enabled = false;
        CPS.enabled = false;

        winTimer -= Time.deltaTime;

        if (winTimer <= 0)
        {
            SceneManager.LoadScene("Title Screen");
        }
        foreach (Image icon in Images)
        {
            icon.enabled = false;
        }
        foreach (Text amount in Texts)
        {
            amount.enabled = false;
        }
    }
}
