using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_playerStatues : MonoBehaviour
{
    public int initialSize;
    int size;
    public delegate float WeightFunction();

    // Start is called before the first frame update
    void Start()
    {
        size = initialSize;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void updateSize(WeightFunction func)
    {

    }
}
