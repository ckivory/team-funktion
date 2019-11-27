using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PD_UICanvasControl : MonoBehaviour
{
    //public List<GameObject> Icons;
    public GameObject Player;
    public List<Image> Images;
    List<Vector3> ImageLoc;
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0;i<Images.Count;i++)
        {
            try
            {
                ImageLoc.Add(Images[i].transform.position);
            }
            catch(System.Exception)
            {
                Debug.Log("Error in PD UI Canvas Control");
            }
        }








        {
            //for(int i = 0; i < Icons.Count; i++)
            //{
            //    Icons[i].GetComponent<PD_UIIconControl>().Player = Player;
            //}
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
