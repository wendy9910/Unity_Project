﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairDrawer : MonoBehaviour
{
    int ControllerDown = 0; //按下滑鼠
    int count = 0;//髮片數量
    int HairWidth = 2;//髮片寬度
    public static int HairStyleState = 1;//髮片風格選擇
    float length = 0.5f;//New & Old間距
    public static float WidthLimit = 0.5f;

    public static List<Vector3> PointPos = new List<Vector3>();//儲存座標
    public static List<Vector3> UpdatePointPos = new List<Vector3>();//變形更新點座標
    public List<GameObject> HairModel = new List<GameObject>();//儲存髮片

    Vector3 NewPos, OldPos;

    public MeshGenerate CreateHair;
    public PositionGenerate CreatePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WidthControl();
        if (ControllerDown == 0) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject Model = new GameObject();
                HairModel.Add(Model);
                HairModel[count].name = "FreeHair" + count;

                NewPos = OldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                ControllerDown = 1;
            }
        }
        if (ControllerDown == 1)
        {
            NewPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            float Distance = Vector3.Distance(OldPos,NewPos);
            if (Distance > length) 
            {
                if (HairModel[count].GetComponent<PositionGenerate>() == null) CreatePosition = HairModel[count].AddComponent<PositionGenerate>();
                else CreatePosition = HairModel[count].GetComponent<PositionGenerate>();
                CreatePosition.GeneratePosition(OldPos,NewPos,HairWidth);

                NewPos = OldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            }
            if (PointPos.Count >= (3 + (HairWidth - 1) * 2) * 2) 
            {
                if (HairModel[count].GetComponent<MeshGenerate>() == null) CreateHair = HairModel[count].AddComponent<MeshGenerate>();
                else CreateHair = HairModel[count].GetComponent<MeshGenerate>();
                CreateHair.GenerateMesh(UpdatePointPos,HairWidth);
            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            PointPos.Clear();
            count++;
            ControllerDown = 0;
        }

    }

    void WidthControl()
    {
        if (Input.GetKeyDown("down") && WidthLimit > 0.3f) WidthLimit -= 0.1f; 
        if (Input.GetKeyDown("up") && WidthLimit < 0.7f) WidthLimit += 0.1f;
        if (Input.GetKeyDown("1")) HairStyleState = 1;
        if (Input.GetKeyDown("2")) HairStyleState = 2;



    }
}
