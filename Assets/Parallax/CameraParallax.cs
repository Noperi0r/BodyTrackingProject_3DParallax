using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CameraParallax : MonoBehaviour
{
    Transform eyeTrackedTransform; // Sensor-tracked eye transform
    Transform rEyeTransformSC; 

    DisplayInfo mainDisplayInfo;
    float rScreenWidth, rScreenHeight; // Real screen size in meter
    const float const_InchToMeter = 0.0254f;
    
    void Start()
    {
        mainDisplayInfo = Screen.mainWindowDisplayInfo;

        GetScreenSize_InMeter();
    }

    void Update()
    {
        
    }

    void GetScreenSize_InMeter()
    {
        rScreenWidth = (mainDisplayInfo.width / Screen.dpi) * const_InchToMeter; 
        rScreenHeight = (mainDisplayInfo.height / Screen.dpi) * const_InchToMeter;
        Debug.Log(rScreenWidth + " / " + rScreenHeight); // TODO: Slightly bigger than the actual value
        // 0.6773334 / 0.381
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
