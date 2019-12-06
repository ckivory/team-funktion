using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    [HideInInspector]
    public Button btnStartGame;
    private GameObject buttonText;

    public GameObject readyCount;
    private bool[] playerReady=new bool[4];

    void OnEnable()
    {
        Cursor.visible = true;  // Added by Carson. This keeps the cursor from being invisible on the start screen.
        btnStartGame = gameObject.GetComponent<Button>();
        if(btnStartGame != null)
        {
            btnStartGame.onClick.AddListener(StartGame);
        }


        for (int i=0;i<playerReady.Length;i++)
        {
            playerReady[i] = false;
        }

    }
    
    void StartGame()
    {
        //buttonText.GetComponent<Text>().fontSize = 60;
        //buttonText.GetComponent<Text>().text = "Loading...";

        readyCount.GetComponent<Text>().text = "Loading...";
        SceneManager.LoadScene("Publisher Demo");
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
                //StartGame();
            }
        }

        for(int i = 1; i < 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "Start"))
            {
                //StartGame();
            }
        }


        if (Input.GetButtonDown("J1Start")) playerReady[0] = true;
        if (Input.GetButtonDown("J2Start")) playerReady[1] = true;
        if (Input.GetButtonDown("J3Start")) playerReady[2] = true;
        if (Input.GetButtonDown("J4Start")) playerReady[3] = true;
        int readyTotal = 0; ;
        foreach (bool ready in playerReady)
        {
            if (ready) readyTotal += 1;
        }
        readyCount.GetComponent<Text>().text = "Player Ready: " + readyTotal;
        if (readyTotal >= 4) StartGame();
    }
}
