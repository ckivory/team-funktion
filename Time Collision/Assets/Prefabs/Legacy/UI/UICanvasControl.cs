using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasControl : MonoBehaviour
{
    public List<GameObject> Icons;
    public GameObject Player;
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
        }
    }
}
