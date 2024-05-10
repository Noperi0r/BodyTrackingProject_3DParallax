using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExponentialFilter : Filter
{
    Vector3 currVec;
    int windowSize;
    int currSkippedWindow;
    float ratio;
    float currRatio;
    public ExponentialFilter() 
    {
        currVec = Vector3.zero;
        windowSize = 4;
        currSkippedWindow = 0;
        ratio = 0.6f;
        currRatio = 1.0f;
    }
    public ExponentialFilter(int n = 4, float r = 0.6f)
    {
        currVec = Vector3.zero;
        windowSize = n;
        currSkippedWindow = 0;
        ratio = r;
        currRatio =1.0f;
    }

    public override void FilterIn(Vector3 input)
    {
        if(input.magnitude == 0)
        {
            currSkippedWindow++;
            currRatio *= ratio;
            if (currSkippedWindow > windowSize)
                currRatio = 0.0f;
            return;
        }
        currSkippedWindow = 0;
        currRatio *= ratio;
        currVec = (1 - currRatio) * currVec + currRatio * input;
        currRatio = 1.0f;
    }

    public override Vector3 FilterOut()
    {
        return currVec;
    }
}
