using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PD_UIIconControl : MonoBehaviour
{
    public Text Text;
    [HideInInspector]
    public GameObject Player;
    public Sprite BlueImage;
    public Sprite GrayImage;
    public Image self;
    /// <summary>
    /// Must be assigned according to the order in inventory
    /// </summary>
    public int Type;
    List<int> Inventory;
    bool selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateGUI();
    }

    void UpdateGUI()
    {
        if(Player != null)
        {
            Inventory = Player.GetComponent<PDPlayerController>().inventory;
            if (Type == Player.GetComponent<PDPlayerController>().selectedProp)
            {
                self.sprite = BlueImage;
            }
            else
            {
                self.sprite = GrayImage;
            }

            try
            {
                Text.text = "" + Inventory[Type];
            }
            catch(System.Exception e)
            {
                Debug.Log("Type: " + Type);
                Debug.Log("Inventory size: " + Inventory.Count);
                Debug.Log(e.Message);
            }
        }
    }
    
}
