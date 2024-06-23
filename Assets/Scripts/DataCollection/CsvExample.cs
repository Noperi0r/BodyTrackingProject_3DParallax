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
        // 예시 ) Data는 statics으로 선언된 힙에 넣어주세요
        elapsedTime += Time.deltaTime;
        string cur = elapsedTime.ToString();
        CsvFileHandler.timeLog.Add(cur);
    }

    //Ondestroy 함수 말고 Position Selecting 할 때마다 감소하는 정수 자료형 만들어서 0되면 저장되게 해주세요.
    //CsvMaker.GetComponent<CsvFileHandler>().MakeCsv(); 오브젝트 넣고 컴포넌트 불러와서 저장
    

}

