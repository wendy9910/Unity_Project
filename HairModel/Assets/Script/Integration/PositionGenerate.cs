﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGenerate : MonoBehaviour
{
    public List<Vector3> GetUpdatePointPos = HairDrawer.UpdatePointPos;
    public List<Vector3> GetPointPos = HairDrawer.PointPos;
    List<Vector3> TempPoint = new List<Vector3>();
    int width = HairDrawer.HairWidth;
    Vector3 cross1, cross2, crossWave, crossTwist;

    public List<Vector3> directionA = HairDrawer.direction;
    //float w = 0;

 
    public void VectorCross(Vector3 up, Vector3 forward, Vector3 right)
    {
        cross1 = Vector3.Cross(up, forward);//x
        cross1.Normalize();
        cross2 = Vector3.Cross(up, right);//z
        cross2.Normalize();
        crossWave = Vector3.Cross(up, right);
        crossWave.Normalize();
        Debug.Log(cross1);
        Debug.Log(cross2);
        directionA.Add(cross1);
        directionA.Add(cross2);
    }

    public void StraightHairtyle(List<Vector3> GetPointPos, int range,int thickness) 
    {
        float w1;
        float w = range * 0.005f * 0.2f;
        float t = thickness * 0.002f;
        if (GetPointPos.Count <= 6) w1 = (range * 0.005f) / GetPointPos.Count;
        else w1 = (range * 0.005f) / range;
        
        TempPoint.Clear();
        for (int i = 0,n = 0; i < GetPointPos.Count; i++, n+=2)
        {
            
            TempPoint.Add(GetPointPos[i] - directionA[n] * w);
            TempPoint.Add(GetPointPos[i] + directionA[n + 1] * t);
            TempPoint.Add(GetPointPos[i] + directionA[n] * w);
            TempPoint.Add(GetPointPos[i] - directionA[n + 1] * t);

            if (w < range * 0.005f) w += w1;
        }
        GetUpdatePointPos.Clear();
        GetUpdatePointPos.AddRange(TempPoint);
    }

    public void DimandHiarStyle(List<Vector3> GetPointPos, int range ,int thickness)
    {

        float w1 = range * 0.005f / (GetPointPos.Count / 2);
        float w = range * 0.005f * 0.2f;
        float t = thickness * 0.002f;

        TempPoint.Clear();
        for (int i = 0, n = 0; i < GetPointPos.Count; i++ , n+=2)
        {
            if (i == GetPointPos.Count - 1 && GetPointPos.Count > 2)
            {
                for (int j = 0; j < 4; j++) TempPoint.Add(GetPointPos[i]);
            }
            else
            {
                TempPoint.Add(GetPointPos[i] - directionA[n] * w);
                TempPoint.Add(GetPointPos[i] + directionA[n + 1] * t);
                TempPoint.Add(GetPointPos[i] + directionA[n] * w);
                TempPoint.Add(GetPointPos[i] - directionA[n + 1] * t);
            }
            if (w < range * 0.005f && i < GetPointPos.Count / 2) w += w1;
            else if (i > GetPointPos.Count / 2) w -= w1;
        }
        GetUpdatePointPos.Clear();
        GetUpdatePointPos.AddRange(TempPoint);
    }

    public void WaveHairStyle(List<Vector3> GetPointPos, int range, int thickness, float WaveCurve) 
    {
        TempPoint.Clear();
        float w1 = range * 0.005f / (GetPointPos.Count / 2);
        float w = range * 0.005f * 0.2f;
        float t = thickness * 0.002f;
        float waveSize = 0.001f;
        float angle = -Mathf.PI;

        for (int i = 0, n = 0; i < GetPointPos.Count; i++,n+=2)
        {
            float y = -Mathf.Sin(angle);//正負的影響
            if (i == 0)
            {
                Vector3 temp = crossWave * waveSize * y;
                Vector3 Vec = GetPointPos[i] + temp;
                for (int j = 0; j < 4; j++) TempPoint.Add(Vec);
            }
            else
            {
                Vector3 temp = crossWave * waveSize * y;
                Vector3 Vec = GetPointPos[i] + temp;
                TempPoint.Add(Vec - directionA[n] * w);
                TempPoint.Add(Vec + directionA[n + 1] * t);
                TempPoint.Add(Vec + directionA[n] * w);
                TempPoint.Add(Vec - directionA[n + 1] * t);

            }
            //if (w < range * 0.005f) w += w1;
            if (w < range * 0.005f && i < GetPointPos.Count / 2) w += w1;
            else if (i > GetPointPos.Count / 2) w -= w1;
            if (waveSize < 0.03f && i%7==0) waveSize += 0.01f;
            //if (i > GetPointPos.Count - 5) waveSize = 0.01f;
            angle += WaveCurve;//0.9f
        }

        GetUpdatePointPos.Clear();
        GetUpdatePointPos.AddRange(TempPoint);
    }
    public void TwistHairStyle(List<Vector3> GetPointPos, int range, int thickness,float TwistCurve)
    {
        TempPoint.Clear();
        float w1 = range * 0.005f / (GetPointPos.Count / 2);
        float w = range * 0.005f * 0.2f;
        float t1 = thickness * 0.005f / (GetPointPos.Count / 2);
        float t = thickness * 0.005f * 0.2f;
        float d = Mathf.PI;
        float a = 0.01f;//0.01f~0.08f

        for (int i = 0, n = 0; i < GetPointPos.Count; i++ , n += 2)
        {
            float x = a * Mathf.Sin(d);
            float y = a * Mathf.Cos(d);
            float z = i;

            Vector3 Vec;
            if (i == 0) Vec = new Vector3(GetPointPos[i].x, GetPointPos[i].y, GetPointPos[i].z);
            else
            {
                Vector3 temp1 = directionA[n] * x, temp2 = directionA[n+2] * y;
                Vec = GetPointPos[i] + temp1 + temp2;
            }
            TempPoint.Add(Vec - directionA[n] * w);
            TempPoint.Add(Vec + directionA[n + 1] * t);
            TempPoint.Add(Vec + directionA[n] * w);
            TempPoint.Add(Vec - directionA[n + 1] * t);

            d += TwistCurve;//原:0.5f
            //if (a < 2 && i % 10 == 0) a += 0.5f;
            if (a < 0.05f && i % 10 == 0) a += 0.01f;
            if (w < range * 0.005f && i < GetPointPos.Count / 2) w += w1;
            if (t < thickness * 0.005f && i < GetPointPos.Count / 2) t += t1;
            if (i > GetPointPos.Count / 2)
            {
                w -= w1;
                t -= t1;
            }
        }
        GetUpdatePointPos.Clear();
        GetUpdatePointPos.AddRange(TempPoint);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < GetPointPos.Count; i++)
        {
            Gizmos.DrawSphere(GetPointPos[i], 0.005f);
        }
    }
}
