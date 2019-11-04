using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public Button btnStart;
    void Start()
    {
        btnStart = gameObject.GetComponent<Button>();
        btnStart.onClick.AddListener(Switch);
    }
    void Switch()
    {
        SceneManager.LoadScene("Level1");
    }
}
