using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

public struct TestDataValue
{
    public enum Dim { d3, d2 };
    public string tag { get; set; }
    public Vector3 direction { get; set; }
    public Vector3 truePos { get; set; }
    public Vector3 predictedPos { get; set; }

    public TestDataValue(int dim, Vector3 _direction, Vector3 _truePos, Vector3 _predictedPos): this()
    {
        if (dim == 0)
            tag = "3D";
        else if (dim == 1)
            tag = "2D";
        direction = _direction;
        truePos = _truePos; 
        predictedPos = _predictedPos;
    }
}

public class DataFileHandler : MonoBehaviour
{
    string filePath = Application.streamingAssetsPath + "/DataCollection/"; // Assets/StreamingAssets/DataCollection
    string fileName = "TestData.txt";
    string dataFile;
    List<TestDataValue> testDataResults = new List<TestDataValue>();

    void Start()
    {
        dataFile = filePath + fileName;

        Directory.CreateDirectory(filePath);

        // DEBUG 
/*        List<TestDataValue> testDataResults = new List<TestDataValue>(); 
       
        TestDataValue testvalue1 = new TestDataValue("2D", Vector3.up, new Vector3(1,2,3), new Vector3(4,5,6));
        TestDataValue testvalue2 = new TestDataValue("3D", Vector3.right, new Vector3(0,0,0), new Vector3(-1,-1,-1));
        TestDataValue testvalue3 = new TestDataValue("3D", Vector3.forward, new Vector3(100, 200, 300), new Vector3(400, 50, 600));

        testDataResults.Add(testvalue1);
        testDataResults.Add(testvalue2);
        testDataResults.Add(testvalue3);

        WriteDataToFile(testDataResults);*/

    }

    public void WriteDataToFile()
    {
        if(!File.Exists(dataFile))
        {
            File.WriteAllText(dataFile, "3D PARALLAX TEST DATA\n");
            print("File created");
        }

        // 태그(2d/3d), 방향값 벡터(x,y,z), 실제 위치(x,y,z), 추정위치(x,y,z) | 기록 시간
        foreach (TestDataValue data in testDataResults) 
        {
            string result = $"Tag: {data.tag} / Direction Vector: {data.direction} / True Position: {data.truePos} / Estimated Position: {data.predictedPos}";
            File.AppendAllText(dataFile, result+"\n");
        }

        print("WriteDataToFile: OK");
    }

    public void dataWrite(TestDataValue t)
    {
        testDataResults.Add(t);
    }
    private void OnDestroy()
    {
        WriteDataToFile();
    }
}
