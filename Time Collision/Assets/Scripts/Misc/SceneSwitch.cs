using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    [HideInInspector]
    public Button btnStartGame;
    public GameObject buttonText;

    void OnEnable()
    {
        Cursor.visible = true;  // Added by Carson. This keeps the cursor from being invisible on the start screen.
        btnStartGame = gameObject.GetComponent<Button>();
        if(btnStartGame != null)
        {
            btnStartGame.onClick.AddListener(StartGame);
        }
    }
    
    void StartGame()
    {
        buttonText.GetComponent<Text>().fontSize = 40;
        buttonText.GetComponent<Text>().text = "Loading...";
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
                StartGame();
            }
        }

        for(int i = 1; i < 4; i++)
        {
            if (Input.GetButtonDown("J" + i + "Start"))
            {
                StartGame();
            }
        }
    }
}
