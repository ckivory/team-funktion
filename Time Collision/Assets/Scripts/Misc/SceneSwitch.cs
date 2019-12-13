using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public Button btnStartGame;
    public GameObject buttonText;
    public Button btnCredit;
    public Button btnQuit;
    public List<GameObject> main = new List<GameObject>();

    public Button btnReturn;
    public GameObject creditRoll;
    public List<GameObject> credit = new List<GameObject>();
    //public GameObject readyCount;
    //private bool[] playerReady=new bool[4];

    public Vector3 startPos;
    public float scrollSpeed;
    public float endPoint;

    void OnEnable()
    {
        Cursor.visible = true;  // Added by Carson. This keeps the cursor from being invisible on the start screen.
        //btnStartGame = gameObject.GetComponent<Button>();
        if (btnStartGame != null)
        {
            btnStartGame.onClick.AddListener(StartGame);
        }
        if (btnCredit!=null)
        {
            btnCredit.onClick.AddListener(ShowCredit);
        }
        if (btnCredit != null)
        {
            btnQuit.onClick.AddListener(QuitGame);
        }
        if (btnReturn!=null)
        {
            btnReturn.onClick.AddListener(ShowMain);
        }
        ShowMain();
        //for (int i=0;i<playerReady.Length;i++)
        //{
        //    playerReady[i] = false;
        //}

    }
    
    void StartGame()
    {
        buttonText.GetComponent<Text>().fontSize = 60;
        buttonText.GetComponent<Text>().text = "Loading...";

        //readyCount.GetComponent<Text>().text = "Loading...";
        SceneManager.LoadScene("Publisher Demo");
    }

    void ShowCredit()
    {
        foreach (GameObject item in credit)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }
        foreach (GameObject item in main)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        creditRoll.transform.position = startPos;
    }

    void ShowMain()
    {
        foreach (GameObject item in main)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }
        foreach (GameObject item in credit)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }

    void QuitGame()
    {
        Application.Quit();
    }

    // Added by Carson. This way we can start the game in a variety of ways.
    private void Update()
    {
        List<KeyCode> startKeys = new List<KeyCode>() {
            KeyCode.KeypadEnter,
            KeyCode.Return,
            KeyCode.Space
        };

        foreach(KeyCode c in startKeys)
        {
            if(Input.GetKeyDown(c))
            {
                if (btnStartGame.IsActive())
                {
                    StartGame();
                }
            }
        }

        for(int i = 1; i < 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "Start"))
            {
                if (btnStartGame.IsActive())
                {
                    StartGame();
                }
            }
        }


        //if (Input.GetButtonDown("J1Start")) playerReady[0] = true;
        //if (Input.GetButtonDown("J2Start")) playerReady[1] = true;
        //if (Input.GetButtonDown("J3Start")) playerReady[2] = true;
        //if (Input.GetButtonDown("J4Start")) playerReady[3] = true;
        //int readyTotal = 0; ;
        //foreach (bool ready in playerReady)
        //{
        //    if (ready) readyTotal += 1;
        //}
        //readyCount.GetComponent<Text>().text = "Player Ready: " + readyTotal;
        //if (readyTotal >= 4) StartGame();

        if (creditRoll.active)
        {
            creditRoll.transform.position += new Vector3(0, scrollSpeed, 0);
            if (creditRoll.transform.position.y>endPoint)
            {
                creditRoll.transform.position = startPos;
            }
        }
    }
}
