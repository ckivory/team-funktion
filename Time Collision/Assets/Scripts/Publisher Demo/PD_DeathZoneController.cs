using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_DeathZoneController : MonoBehaviour
{
    public List<GameObject> Players;

    public List<float> radii;
    private float radius;

    public List<float> damageValues;
    public float currentDamage;

    public List<float> waveTimes;
    
    private float timer;
    private int waveNum;

    private bool hidden;
    
    private void initializeRadius()
    {
        try
        {
            radius = radii[0];
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message+" Make sure you have at least one radius in the list");
        }
    }

    private void initializeTimer()
    {
        timer = 0f;
        waveNum = 0;
    }

    public void Start()
    {
        initializeRadius();
        initializeTimer();
        currentDamage = damageValues[0];
        hidden = false;
    }

    private void updateRadius()
    {
        float ratio = timer / waveTimes[waveNum];
        // Split the difference
        int outer = waveNum / 2;
        int inner = outer + 1;
        radius = radii[outer] - (radii[outer] - radii[inner]) * ratio;

        transform.localScale = new Vector3(radius, transform.localScale.y, radius);
    }
    
    public void Update()
    {
        if(!hidden)
        {
            if (waveNum > waveTimes.Count - 1)
            {
                // DeathZone should disappear at the end.
                foreach (GameObject player in Players)
                {
                    player.SendMessage("LeaveZone");
                }
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                hidden = true;
                return;
            }

            timer += Time.deltaTime;
            //Debug.Log("Wave: " + waveNum + ". Radius: " + radius + ". Time left in wave: " + (waveTimes[waveNum] - timer));

            // Even waveNumber means we are staying the same size, odd means we are moving to a new size
            if (waveNum % 2 == 1)
            {
                updateRadius();
            }
            if (timer > waveTimes[waveNum])
            {
                waveNum++;
                timer = 0f;
                currentDamage = damageValues[waveNum / 2];
            }
        }
    }
}
