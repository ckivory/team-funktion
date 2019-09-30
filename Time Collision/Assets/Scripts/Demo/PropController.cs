using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public int propNum;

<<<<<<< HEAD
    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("addToInventory", propNum);
        Destroy(gameObject);
=======
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
>>>>>>> 025950f46eea790e966ddd420513158a6e677c9b
    }
}
