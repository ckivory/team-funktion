using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public Button btnStartGame;

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
        SceneManager.LoadScene("Publisher Demo");
    }

    // Added by Carson. This way we can
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
}
