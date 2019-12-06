using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReaperController : MonoBehaviour
{
    public float interval = 0.2f;
    private float timer;

    private void CloseGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("Edit/Play");
        #endif

        Application.Quit();
    }

    void Start()
    {
        timer = 0f;
    }
    
    void Update()
    {
        // Double-tab escape to close game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(timer > 0f)
            {
                CloseGame();
            }
            else
            {
                timer = interval;
            }
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
