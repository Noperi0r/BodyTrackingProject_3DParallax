using Microsoft.Azure.Kinect.BodyTracking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTransformSender
{
    [SerializeField] main kinectMain;

    void GetJointTrasnform(JointId jointId, Transform jointTransform) // jointId parameter would be mainly JointId.Head in this case.
    {
        TrackerHandler trackerHandler = kinectMain.m_tracker.GetComponent<TrackerHandler>();
        BackgroundData lastFrameData = kinectMain.m_lastFrameData;

        var jointPos = lastFrameData.Bodies[trackerHandler.findClosestTrackedBody(lastFrameData)].JointPositions3D[(int)jointId];
        var jointRot = lastFrameData.Bodies[trackerHandler.findClosestTrackedBody(lastFrameData)].JointRotations[(int)jointId];

        jointTransform.position = new Vector3(jointPos.X, jointPos.Y, jointPos.Z);
        jointTransform.rotation = new Quaternion(jointRot.X, jointRot.Y, jointRot.Z, jointRot.W);
    }
}
