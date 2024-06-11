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
            // ������ ���� ���� ����
            randomDirection = Random.onUnitSphere * 3;

            // LineRenderer ����
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, randomDirection);

            // ���� ���� �� �β� ���� (�ɼ�)
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            RaycastHit[] hits = Physics.RaycastAll(Vector3.zero, randomDirection.normalized);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Plane")) // ������Ʈ�� Plane �±׸� ������ �ִ��� Ȯ��
                {
                    // �浹 ������ ���� 2D ��ǥ�� ��ȯ
                    Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);
                    Vector2 local2DPoint = new Vector2(localPoint.x, localPoint.z); // Plane�� 2D ��ǥ (x, z)

                    Debug.Log($"{hit.collider.name}�� �浹�� ����: {local2DPoint}");

                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // ���� ī�޶󿡼� ���콺 ��ġ�� ���� ����
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // ���̰� ������Ʈ�� �浹�ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            // �浹�� ������Ʈ�� Plane �±׸� ������ �ִ��� Ȯ��
            if (hit.collider.gameObject.CompareTag("Plane"))
            {
                markerInstance.transform.position = hit.point;

                // ������ Ŭ���� ������ ���� 2D ��ǥ�� ��ȯ
                Vector3 localPoint = hit.collider.transform.InverseTransformPoint(hit.point);
                userClickPosition = new Vector2(localPoint.x, localPoint.z); // Plane�� 2D ��ǥ (x, z)
            }
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log($"���� Ŭ�� ��ġ: {userClickPosition}");

            // ���ο� ���� ���� ����
            GenerateRandomVector();
        }
    }
}
