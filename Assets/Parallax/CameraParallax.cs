using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CameraParallax : MonoBehaviour
{
    Transform eyeTrackedTransform; // Sensor-tracked eye transform
    Transform rEyeTransformSC; 

    //DisplayInfo mainDisplayInfo;
    float rScreenWidth, rScreenHeight; // Real screen size in meter

    const float const_InchToMeter = 0.0254f;
    const float const_mmToMeter = 0.001f;

    void Start()
    {
        //mainDisplayInfo = Screen.mainWindowDisplayInfo;
     
        // Debug
        GetScreenSize_InMeter();
    }

    void Update()
    {
    }

    void GetScreenSize_InMeter()
    {
        //rScreenWidth = (mainDisplayInfo.width / Screen.dpi) * const_InchToMeter; 
        //rScreenHeight = (mainDisplayInfo.height / Screen.dpi) * const_InchToMeter;

        rScreenWidth = ScreenSizeGetter.GetScreenWidth() * const_mmToMeter;
        rScreenHeight = ScreenSizeGetter.GetScreenHeight() * const_mmToMeter;

        Debug.Log(rScreenWidth + " / " + rScreenHeight); 
    }

    void GetRealEyePos_InScreenCoord()
    {
        Vector3 rEyePosWC = new Vector3();
        rEyePosWC = eyeTrackedTransform.position + new Vector3(rScreenHeight/2, rScreenHeight/2, rScreenHeight/2);

        rEyeTransformSC.position = rEyePosWC / rScreenWidth;
    }

    // ---------

    // Compute VEsc method

    // Compute Tvector method

    // Compute Translation DF method
}
