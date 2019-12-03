using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnHit : MonoBehaviour
{
    public float speed1 = 0.25f;
    public float speed2 = 0.25f;
    public float length = 1.0f;
    private Color originalColor;
    private Color switchColor;
    public GameObject core;
    private MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = core.GetComponent<MeshRenderer>();
        originalColor = renderer.material.color;
        switchColor = Color.red;
        switchColor.a = 0.8f;
    }

    public IEnumerator FlashObject()
    {
        float flashing = 0f;
        while (flashing<=length)
        {
            renderer.material.color = switchColor;
            yield return new WaitForSeconds(speed1);
            flashing += speed1;
            renderer.material.color = originalColor;
            yield return new WaitForSeconds(speed2);
            flashing += speed2;
        }
    }
}
