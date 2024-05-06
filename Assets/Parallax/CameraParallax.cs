using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class CameraParallax : MonoBehaviour
{
    Transform eyeTrackedTransform; // Sensor-tracked eye transform
    public Transform rEyeTransformSC; //test

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
    
    // Get Monitor Information Method
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

    public Camera firstCam;
    public Camera secondCam;

    public GameObject virtualWindow;

    public float windowSize = 2.0f;
    public float windowDistance = 5.0f;

    public float maxHeight = 2000.0f;

    float windowWidth;
    float windowHeight;

    Vector3 vEyeTransformSC;
    Vector3 vEyeTransformWC;

    Vector3 Tvector;

    public bool verbose = false;

    void LateUpdate()
    {
        firstCam.enabled = false;
        firstCam.transform.position = transform.position;
        virtualWindow.transform.position = firstCam.transform.position + firstCam.transform.forward * windowDistance;

        windowWidth = (float)Screen.width / Screen.height * windowSize;
        windowHeight = windowSize;

        // gets the local position of this camera depending on the virtual screen
        Vector3 firstLocalPos = virtualWindow.transform.InverseTransformPoint(firstCam.transform.position);

        vEyeTransformWC = firstLocalPos;
        vEyeTransformSC = vEyeTransformWC / windowWidth;

        Tvector = rEyeTransformSC.position - vEyeTransformSC;
        secondCam.transform.position = firstCam.transform.position + Tvector;

        Vector3 secondLocalPos = virtualWindow.transform.InverseTransformPoint(secondCam.transform.position);

        setAsymmetricFrustum(firstCam, firstLocalPos, firstCam.nearClipPlane);
        setAsymmetricFrustum(secondCam, secondLocalPos, secondCam.nearClipPlane);
    }

    /// <summary>
    /// Sets the asymmetric Frustum for the given virtual Window (at pos 0,0,0 )
    /// and the camera passed
    /// </summary>
    /// <param name="cam">Camera to get the asymmetric frustum for</param>
    /// <param name="pos">Position of the camera. Usually cam.transform.position</param>
    /// <param name="nearDist">Near clipping plane, usually cam.nearClipPlane</param>
    public void setAsymmetricFrustum(Camera cam, Vector3 pos, float nearDist)
    {

        // Focal length = orthogonal distance to image plane
        Vector3 newpos = pos;
        //newpos.Scale (new Vector3 (1, 1, 1));
        float focal = 1;

        newpos = new Vector3(newpos.x, newpos.y, -newpos.z);
        if (verbose)
        {
            Debug.Log(newpos.x + ";" + newpos.y + ";" + newpos.z);
        }

        focal = Mathf.Clamp(newpos.z, 0.001f, maxHeight);

        // Ratio for intercept theorem
        float ratio = focal / nearDist;

        // Compute size for focal
        float imageLeft = (-windowWidth / 2.0f) - newpos.x;
        float imageRight = (windowWidth / 2.0f) - newpos.x;
        float imageTop = (windowHeight / 2.0f) - newpos.y;
        float imageBottom = (-windowHeight / 2.0f) - newpos.y;

        // Intercept theorem
        float nearLeft = imageLeft / ratio;
        float nearRight = imageRight / ratio;
        float nearTop = imageTop / ratio;
        float nearBottom = imageBottom / ratio;

        Matrix4x4 m = PerspectiveOffCenter(nearLeft, nearRight, nearBottom, nearTop, cam.nearClipPlane, cam.farClipPlane);
        cam.projectionMatrix = m;
    }



    /// <summary>
    /// Set an off-center projection, where perspective's vanishing
    /// point is not necessarily in the center of the screen.
    /// left/right/top/bottom define near plane size, i.e.
    /// how offset are corners of camera's near plane.
    /// Tweak the values and you can see camera's frustum change.
    /// </summary>
    /// <returns>The off center.</returns>
    /// <param name="left">Left.</param>
    /// <param name="right">Right.</param>
    /// <param name="bottom">Bottom.</param>
    /// <param name="top">Top.</param>
    /// <param name="near">Near.</param>
    /// <param name="far">Far.</param>
    Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = (2.0f * near) / (right - left);
        float y = (2.0f * near) / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0f * far * near) / (far - near);
        float e = -1.0f;

        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
        m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
        return m;
    }
    /// <summary>
    /// Draws gizmos in the Edit window.
    /// </summary>
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(virtualWindow.transform.position, virtualWindow.transform.position - virtualWindow.transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(virtualWindow.transform.position - virtualWindow.transform.up * 0.5f * windowHeight, virtualWindow.transform.position + virtualWindow.transform.up * 0.5f * windowHeight);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(virtualWindow.transform.position - virtualWindow.transform.right * 0.5f * windowWidth, virtualWindow.transform.position + virtualWindow.transform.right * 0.5f * windowWidth);
        Gizmos.color = Color.white;
        Vector3 leftBottom = virtualWindow.transform.position - virtualWindow.transform.right * 0.5f * windowWidth - virtualWindow.transform.up * 0.5f * windowHeight;
        Vector3 leftTop = virtualWindow.transform.position - virtualWindow.transform.right * 0.5f * windowWidth + virtualWindow.transform.up * 0.5f * windowHeight;
        Vector3 rightBottom = virtualWindow.transform.position + virtualWindow.transform.right * 0.5f * windowWidth - virtualWindow.transform.up * 0.5f * windowHeight;
        Vector3 rightTop = virtualWindow.transform.position + virtualWindow.transform.right * 0.5f * windowWidth + virtualWindow.transform.up * 0.5f * windowHeight;

        Gizmos.DrawLine(leftBottom, leftTop);
        Gizmos.DrawLine(leftTop, rightTop);
        Gizmos.DrawLine(rightTop, rightBottom);
        Gizmos.DrawLine(rightBottom, leftBottom);
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(firstCam.transform.position, leftTop);
        Gizmos.DrawLine(firstCam.transform.position, rightTop);
        Gizmos.DrawLine(firstCam.transform.position, rightBottom);
        Gizmos.DrawLine(firstCam.transform.position, leftBottom);

        Gizmos.DrawLine(secondCam.transform.position, leftTop);
        Gizmos.DrawLine(secondCam.transform.position, rightTop);
        Gizmos.DrawLine(secondCam.transform.position, rightBottom);
        Gizmos.DrawLine(secondCam.transform.position, leftBottom);
    }
}
