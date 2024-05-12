using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class AverageFilter : Filter
{
    int windowSize;
    Vector3[] vector3s;
    int currWindowIndex;
    Vector3 currVec;

    public AverageFilter()
    {
        windowSize = 4;
        vector3s = new Vector3[windowSize];
        currWindowIndex = 0;
        currVec = new(0,0,0);
        for(int i = 0; i < windowSize; i++)
        {
            vector3s[i] = new(0, 0, 0);
        }
    }
    public AverageFilter(int size)
    {
        windowSize = size;
        vector3s = new Vector3[windowSize];
        currWindowIndex = 0;
        currVec = new(0, 0, 0);
        for (int i = 0; i < windowSize; i++)
        {
            vector3s[i] = new(0, 0, 0);
        }
    }
    public override void FilterIn(Vector3 input)
    {
        //input : float vector3 transform sensor input, if sensor detects no person, input will be (0,0,0)
        vector3s[currWindowIndex] = input;
        currWindowIndex++;
        currWindowIndex %= windowSize;
        currVec = new(0, 0, 0);
        for(int i = 0;i < windowSize; i++)
        {
            currVec += vector3s[i] / windowSize;
        }
    }

    public override Vector3 FilterOut()
    {
        return currVec;
    }
}
