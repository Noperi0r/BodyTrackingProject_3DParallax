using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    // Component for getting eye transformation 

    // Get Monitor Information Method

    // Compute REsc Method

    // ---------

    public Camera mainCamera;
    public Camera firstCamera;

    float nearClipPlaneHeight;
    float nearClipPlaneWidth;

    Vector3 vEyeTransformSC;
    Vector3 vEyeTransformWC;
    public Transform rEyeTransformSC; //test

    Vector3 Tvector;

    private void Start()
    {
        GetNearPlaneSize();
    }

    void GetNearPlaneSize()
    {
        float degreeOfFieldOfView = (firstCamera.fieldOfView / 2) * Mathf.PI / 180;
        nearClipPlaneHeight = 2 * firstCamera.nearClipPlane * Mathf.Tan(degreeOfFieldOfView);
        nearClipPlaneWidth = nearClipPlaneHeight * firstCamera.aspect;
    }

    void LateUpdate()
    {
        RestCamera();
        GetVEsc();
        GetTvector();
        GetDF();
    }

    void GetVEsc()
    {
        vEyeTransformWC = new Vector3(0.0f, 0.0f, -firstCamera.nearClipPlane);

        vEyeTransformSC = vEyeTransformWC / nearClipPlaneWidth;
    }

    void GetTvector()
    {
        Tvector = rEyeTransformSC.position - vEyeTransformSC;
        Debug.Log(Tvector);
        mainCamera.transform.position += Tvector;
    }

    //private float left = -0.2F;
    //private float right = 0.2F;
    //private float top = 0.2F;
    //private float bottom = -0.2F;

    void GetDF()
    {
        // Projection Matrix를 가져옴
        //Matrix4x4 projectionMatrix = mainCamera.projectionMatrix;

        // near plane을 이동할 벡터만큼 조정
        //Matrix4x4 m = PerspectiveOffCenter(-nearClipPlaneWidth/2, nearClipPlaneWidth/2, -nearClipPlaneHeight/2, nearClipPlaneHeight/2, firstCamera.nearClipPlane, firstCamera.farClipPlane);
        //projectionMatrix.m02 -= Tvector.x;
        //projectionMatrix.m12 -= Tvector.y;
        //projectionMatrix.m22 -= Tvector.z;


        /////////////////// ??

        //left = firstCamera.nearClipPlane * (-(nearClipPlaneWidth / 2) - rEyeTransformSC.position.x) / rEyeTransformSC.position.z;
        //right = firstCamera.nearClipPlane * (nearClipPlaneWidth / 2 - rEyeTransformSC.position.x) / rEyeTransformSC.position.z;
        //bottom = firstCamera.nearClipPlane * (-(nearClipPlaneHeight / 2) - rEyeTransformSC.position.y) / rEyeTransformSC.position.z;
        //top = firstCamera.nearClipPlane * (nearClipPlaneHeight / 2 - rEyeTransformSC.position.y) / rEyeTransformSC.position.z;
        //mainCamera.transform.position = new Vector3(-rEyeTransformSC.position.x, rEyeTransformSC.position.y, rEyeTransformSC.position.z);
        //mainCamera.transform.LookAt(new Vector3(-rEyeTransformSC.position.x, rEyeTransformSC.position.y, 0));

        //Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, firstCamera.nearClipPlane, firstCamera.farClipPlane);
        //mainCamera.projectionMatrix = m;
    }

    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }

    void RestCamera()
    {
        mainCamera.transform.position = firstCamera.transform.position;
        mainCamera.projectionMatrix = firstCamera.projectionMatrix;
    }
}
