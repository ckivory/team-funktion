using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasControl : MonoBehaviour
{
    public List<GameObject> Icons;
    public List<UnityEngine.UI.Image> Images;
    public List<Sprite> GraySprites;
    public List<Sprite> BlueSprites;
    public GameObject Player;
    int selectedType;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Icons.Count; i++)
        {
            Icons[i].GetComponent<UIIconControl>().Player = Player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Player == null)
        {
            Destroy(this.gameObject);
            return;
        }
        updateImage();


    }

    void updateImage()
    {
        selectedType = Player.GetComponent<VSPlayerController>().selectedProp;
        int l1 = selectedType - 1;
        int l2 = selectedType - 2;
        int r1 = selectedType + 1;
        int r2 = selectedType + 2;
        if (l1 < 0)
        {
            l1 = GraySprites.Count + l1;
        }
        if(l2<0)
        {
            l2 = GraySprites.Count + l2;
        }
        if (r1 > GraySprites.Count-1)
        {
            r1 =  r1 - GraySprites.Count;
        }
        if (r2 > GraySprites.Count-1)
        {
            r2 = r2 - GraySprites.Count;
        }

        Images[2].sprite = BlueSprites[selectedType];
        Images[1].sprite = GraySprites[l1];
        Images[0].sprite = GraySprites[l2];
        Images[3].sprite = GraySprites[r1];
        Images[4].sprite = GraySprites[r2];

    }

    void showIcons()
    {
        for(int i = 0; i < Images.Count; i++)
        {
        }
    }
}
