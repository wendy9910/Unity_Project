﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class meshmodel : MonoBehaviour
{
    public List<Vector3> MousePointPos = new List<Vector3>();
    private Vector3 MousePos, LastPos;
    private Mesh mesh;
    private Vector3[] vertices;

    int down = 0;//滑鼠判定
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            down = 1;
            MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            //MousePointPos.Add(MousePos);
            LastPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

        }
        if (down == 1) {
            

            MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10.0f));
           
            float dist = Vector3.Distance(LastPos, MousePos);
            if (dist > 1.0f) 
            {
                Generate(MousePos, LastPos);
                MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                MousePointPos.Add(MousePos);
                LastPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                Debug.Log(MousePos);
            }


        }
        if (Input.GetMouseButtonUp(0))down = 2;

        if (down == 2) {

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Hair Grid";

            mesh.vertices = MousePointPos.ToArray();

            int[] triangles = new int[(MousePointPos.Count / 2 -1) * 6];
            for (int ti = 0, vi = 0, x = 0; x < MousePointPos.Count / 2 - 1; x++, ti += 6, vi+=2)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = vi + 2;
                triangles[ti + 3] = vi + 2;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = vi + 3;

            }
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

        }
        
    }
     
    void Generate(Vector3 pos1,Vector3 pos2)
    {
        Vector3 Vec0 = pos1-pos2;
        Vector3 Vec1 = new Vector3(Vec0.y,-Vec0.x,0.0f);
        Vector3 AddPos = new Vector3(pos1.x+Vec1.x,pos1.y+Vec1.y,0.0f);
        MousePointPos.Add(AddPos);    
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < MousePointPos.Count; i++) {
            Gizmos.DrawSphere(MousePointPos[i], 0.1f);
        }
        
    }
}