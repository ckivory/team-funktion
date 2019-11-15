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
        btnStartGame = gameObject.GetComponent<Button>();
        btnStartGame.onClick.AddListener(StartGame);
    }
    void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
