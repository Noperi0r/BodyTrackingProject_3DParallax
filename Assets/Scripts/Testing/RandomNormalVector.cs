using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class RandomNormalVector : MonoBehaviour
{
    public GameObject markerPrefab;
    private GameObject markerInstance;

    private LineRenderer lineRenderer;
    private Vector3 randomDirection;

    Vector2 userClickPosition = Vector2.zero;
    void Start()
    {
        markerInstance = Instantiate(markerPrefab);
        lineRenderer = GetComponent<LineRenderer>();
        GenerateRandomVector();
    }

    void GenerateRandomVector()
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
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            RaycastHit[] hits = Physics.RaycastAll(Vector3.zero, randomDirection.normalized);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Plane")) // 오브젝트가 Plane 태그를 가지고 있는지 확인
                {
                    // 충돌 지점을 로컬 2D 좌표로 변환
                    Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);
                    Vector2 local2DPoint = new Vector2(localPoint.x, localPoint.z); // Plane의 2D 좌표 (x, z)

                    Debug.Log($"{hit.collider.name}과 충돌한 지점: {local2DPoint}");

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
                markerInstance.transform.position = hit.point;

                // 유저가 클릭한 지점을 로컬 2D 좌표로 변환
                Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);
                userClickPosition = new Vector2(localPoint.x, localPoint.z); // Plane의 2D 좌표 (x, z)
            }
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log($"유저 클릭 위치: {userClickPosition}");

            // 새로운 랜덤 벡터 생성
            GenerateRandomVector();
        }
    }
}
