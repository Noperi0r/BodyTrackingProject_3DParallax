using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvExample : MonoBehaviour
{
    public GameObject CsvMaker;

    [SerializeField]
    private float elapsedTime = 0f;


    void Update()
    {
        // ���� ) Data�� statics���� ����� ���� �־��ּ���
        elapsedTime += Time.deltaTime;
        string cur = elapsedTime.ToString();
        CsvFileHandler.timeLog.Add(cur);
    }

    //Ondestroy �Լ� ���� Position Selecting �� ������ �����ϴ� ���� �ڷ��� ���� 0�Ǹ� ����ǰ� ���ּ���.
    //CsvMaker.GetComponent<CsvFileHandler>().MakeCsv(); ������Ʈ �ְ� ������Ʈ �ҷ��ͼ� ����
    

}

