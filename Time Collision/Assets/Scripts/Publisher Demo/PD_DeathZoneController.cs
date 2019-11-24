﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_DeathZoneController : MonoBehaviour
{
    public List<GameObject> Players;

    public List<float> radii;
    private float radius;

    public List<float> damageAmounts;

    public List<float> waveTimes;

    private float timer;
    private int waveNum;

    private void invertMesh()
    {
        MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (filter != null)
        {
            Mesh mesh = filter.mesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }
    }
    
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
        invertMesh();
        initializeRadius();
        initializeTimer();
    }

    private void updateRadius()
    {
        float ratio = timer / waveTimes[waveNum];
        // Split the difference
        int outer = waveNum / 2;
        int inner = outer + 1;
        radius = radii[outer] - (radii[outer] - radii[inner]) * ratio;

        transform.localScale = new Vector3(radius, 250, radius);
    }
    
    public void Update()
    {
        if (waveNum > waveTimes.Count - 1)
        {
            // DeathZone should disappear at the end.
            foreach(GameObject player in Players)
            {
                player.SendMessage("LeaveZone");
            }
            Destroy(gameObject);
            return;
        }
        timer += Time.deltaTime;

        //Debug.Log("Wave: " + waveNum + ". Radius: " + radius + ". Time left in wave: " + (waveTimes[waveNum] - timer));

        // Even waveNumber means we are staying the same size, odd means we are moving to a new size
        if (waveNum % 2 == 1)
        {
            updateRadius();
        }
        if(timer > waveTimes[waveNum])
        {
            waveNum++;
            timer = 0f;
        }
    }
}
