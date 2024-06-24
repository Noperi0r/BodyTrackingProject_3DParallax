using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class RandomNormalVector : MonoBehaviour
{
    public enum Dim { d3, d2 };
    public Dim dim;
    public GameObject markerPrefab;
    public GameObject Origin;
    public DataFileHandler dataFileHandler;
    public string is3d;

    private GameObject markerInstance;

    private LineRenderer lineRenderer;
    private Vector3 randomDirection;

    private Vector3 inputVector;
    private bool isStarted = false;
    private float deltaTime;
    private Stopwatch watch = new Stopwatch();

    void Start()
    {
        markerInstance = Instantiate(markerPrefab);
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void GenerateRandomVector()
    {
        while (true)
        {
            // 임의의 방향 벡터 생성
            randomDirection = Random.onUnitSphere * 3;

            // LineRenderer 설정
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, randomDirection);

            // 선의 색상 및 두께 설정 (옵션)
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;

            RaycastHit[] hits = Physics.RaycastAll(Vector3.zero, randomDirection.normalized);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Plane")) // 오브젝트가 Plane 태그를 가지고 있는지 확인
                {
                    watch.Start();
                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 메인 카메라에서 마우스 위치로 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // 레이가 오브젝트와 충돌했는지 확인
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌한 오브젝트가 Plane 태그를 가지고 있는지 확인
            if (hit.collider.gameObject.CompareTag("Plane"))
            {
                //평면상에 마커 배치
                markerInstance.transform.position = hit.point;
            }
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isStarted)
            {
                isStarted = true;
                return;
            }
            //Debug.Log($"Answer : {answerPlane} : {answerPosition}\nUser : {userClickPlane} : {userClickPosition}");
            watch.Stop();
            deltaTime = watch.ElapsedMilliseconds;
            inputVector = markerInstance.transform.position-Origin.transform.position;
            inputVector.Normalize();
            randomDirection.Normalize();
            float realLongitude = vectorToLongitudeDegree(randomDirection);
            float realLatitude = vectorToLatitudeDegree(randomDirection);

            float inputLongitude = vectorToLongitudeDegree(inputVector);
            float inputLatitude = vectorToLatitudeDegree(inputVector);

            float longitude = inputLongitude- realLongitude;
            if (longitude > 180.0f)
            {
                longitude = 360 - longitude;
            }
            if(longitude < -180.0f)
            {
                longitude = 360 + longitude;
            }
            float latitude = inputLatitude -realLatitude;
            float angle = Vector3.Angle(randomDirection, inputVector); //구현필요
            TestDataValue t = new TestDataValue((int)dim, randomDirection, inputVector, deltaTime, longitude, latitude, angle);
            dataFileHandler.dataWrite(t);
            // 새로운 랜덤 벡터 생성
            GenerateRandomVector();
        }
    }
    private float vectorToLongitudeDegree(Vector3 vector)
    {
        return Mathf.Atan2(vector.z,vector.x) * Mathf.Rad2Deg + 180;
    }
    private float vectorToLatitudeDegree(Vector3 vector)
    {
        return Mathf.Atan2(Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z), vector.y) * Mathf.Rad2Deg;
    }
}
