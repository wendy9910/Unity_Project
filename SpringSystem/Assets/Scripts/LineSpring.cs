﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpring : MonoBehaviour
{
    private LineRenderer player;
    public List<Vector3> MousePointPos = new List<Vector3>();
    public List<Vector3> SpherePos = new List<Vector3>();
    public List<GameObject> SphereGroup = new List<GameObject>();
    private Vector3 MousePos, LastPos;
    GameObject sphere;
    private bool Down;
    public float mass1 = 1f;
    int count = 0;
    int v = 5;


    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.AddComponent<LineRenderer>();
        player.material = new Material(Shader.Find("Sprites/Default"));
        player.SetColors(Color.green, Color.gray);
        player.SetWidth(0.1f, 0.1f);
    }

    void Update()
    {
        player = GetComponent<LineRenderer>();
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Input.mousePosition;

            float x0 = (mousePos.x - Screen.width / 2) / (Screen.width / 2);
            float y0 = (mousePos.y - Screen.height / 2) / (Screen.height / 2);

            float y = 10 * Mathf.Tan(Mathf.Deg2Rad * 30) * y0;// 求tan(30);
            float x = 10 * Mathf.Tan(Mathf.Deg2Rad * 30) * x0 * Screen.width / Screen.height; //加上 Screen.width/Screen.height 控制螢幕寬變動

            MousePos = new Vector3(x, y, 0.0f);
            MousePointPos.Add(MousePos);

            player.numCapVertices = 2;//端點圓度
            player.numCornerVertices = 2;//拐彎圓滑度

            LastPos = new Vector3(x, y, 0.0f);

            player.positionCount = MousePointPos.Count;
            player.SetPositions(MousePointPos.ToArray());

            SphereGroup.Add(sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere));
            sphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            sphere.transform.position = MousePos;
            SpherePos.Add(sphere.transform.position);

            Rigidbody RG = sphere.AddComponent<Rigidbody>();
            RG.isKinematic = true;
            RG.mass = mass1;

            Down = true;  
        }
        if (Down == true)
        {
            Vector3 mousePos = Input.mousePosition;

            float x0 = (mousePos.x - Screen.width / 2) / (Screen.width / 2);
            float y0 = (mousePos.y - Screen.height / 2) / (Screen.height / 2);
            float y = 10 * Mathf.Tan(Mathf.Deg2Rad * 30) * y0;// 求tan(30);
            float x = 10 * Mathf.Tan(Mathf.Deg2Rad * 30) * x0 * Screen.width / Screen.height; //加上 Screen.width/Screen.height 控制螢幕寬變動

            MousePos = new Vector3(x, y, 0.0f);
            float dist = Vector3.Distance(LastPos, MousePos);
            if (dist > 1f)
            {//更新座標
                MousePos = new Vector3(x, y, 0.0f);
                MousePointPos.Add(MousePos);
                LastPos = new Vector3(x, y, 0.0f);

                

                SphereGroup.Add(sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere));
                sphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                sphere.transform.position = MousePos;
                SpherePos.Add(sphere.transform.position);
                Rigidbody RG = sphere.AddComponent<Rigidbody>();
                RG.isKinematic = true;
                RG.mass = mass1;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Down = false;
            if (Down == false)
            {
                Spring();
                for (int i = 0; i < SphereGroup.Count; i++)
                {
                    SpherePos[i] = SphereGroup[i].transform.position;
                }
                player.positionCount = SpherePos.Count;
                player.SetPositions(SpherePos.ToArray());

            }
            //MousePointPos.Clear();
        }

        void Spring()
        {
            Rigidbody fristRG = SphereGroup[0].GetComponent<Rigidbody>();
            fristRG.isKinematic = true;
            fristRG.mass = mass1;

            for (int i = 0; i < SphereGroup.Count - 1; i++)
            {

                SpringJoint MainSpring = SphereGroup[i].AddComponent<SpringJoint>();
                MainSpring.spring = 20.0f;
                MainSpring.damper = 5.0f;

                SphereGroup[i].transform.position = MousePointPos[i];
                SphereGroup[i + 1].transform.position = MousePointPos[i + 1];
              
                Rigidbody otherRG = SphereGroup[i + 1].GetComponent<Rigidbody>();

                otherRG.isKinematic = false;
                otherRG.mass = mass1;
                MainSpring.connectedBody = otherRG;

            }
        }
        
        


    }
}