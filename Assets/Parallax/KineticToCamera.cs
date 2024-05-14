using Apt.Unity.Projection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticToCamera : MonoBehaviour
{
    [Header("Projection plane")]
    public ProjectionPlane ProjectionScreen;
    [Header("Kinetic Main")]
    [SerializeField] main kinectMain;
    [Header("Input Filter")]
    [SerializeField] InputFilter inputFilter;

    Vector3 eyeTrackedTransform; // Sensor-tracked eye transform

    float rScreenWidth, rScreenHeight; // Real screen size in meter

    const float const_mmToMeter = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        GetScreenSize_InMeter();
    }

    void GetScreenSize_InMeter()
    {
        rScreenWidth = ScreenSizeGetter.GetScreenWidth() * const_mmToMeter;
        rScreenHeight = ScreenSizeGetter.GetScreenHeight() * const_mmToMeter;

        //Debug.Log(rScreenWidth + " / " + rScreenHeight);
    }

    void GetRealEyePos_InScreenCoord()
    {
        float windowWidth = ProjectionScreen.Size[0];
        eyeTrackedTransform = eyeTrackedTransform + new Vector3(0, rScreenHeight / 2, 0);
        eyeTrackedTransform = eyeTrackedTransform * windowWidth / rScreenWidth;
    }

    // Update is called once per frame
    void Update()
    {
        eyeTrackedTransform = kinectMain.m_tracker.GetComponent<BodyTransformSender>().InputPositionEye(kinectMain.m_lastFrameData);
        if (eyeTrackedTransform != Vector3.zero)
        {
            eyeTrackedTransform *= -1;
            eyeTrackedTransform = inputFilter.GetVector(eyeTrackedTransform);
            GetRealEyePos_InScreenCoord();
            transform.localPosition = eyeTrackedTransform;
        }
    }
}
