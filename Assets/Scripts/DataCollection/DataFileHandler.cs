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
    public Vector3 trueVector { get; set; }
    public Vector3 inputVector { get; set; }
    public float time {  get; set; }
    public float longitude { get; set; }
    public float latitude { get; set; }
    public float containedAngle { get; set; }
    

    public TestDataValue(int dim, Vector3 _trueVector, Vector3 _inputVector,float _time, float _longitude, float _latitude, float _containedAngle): this()
    {
        if (dim == 0)
            tag = "3D";
        else if (dim == 1)
            tag = "2D";
        trueVector = _trueVector;
        inputVector = _inputVector;
        time = _time;
        longitude = _longitude;
        latitude = _latitude;
        containedAngle = _containedAngle;
    }
}

public class DataFileHandler : MonoBehaviour
{
    string filePath = Application.streamingAssetsPath + "/DataCollection/"; // Assets/StreamingAssets/DataCollection
    string baseFileName = "TestData_";
    string fileExtension = ".txt";
    string dataFile;
    List<TestDataValue> testDataResults = new List<TestDataValue>();

    void Start()
    {
        Directory.CreateDirectory(filePath);
        dataFile = GetUniqueFilePath();


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
    string GetUniqueFilePath()
    {
        int fileIndex = 0;
        string fileName = baseFileName + fileIndex + fileExtension;
        string filePathWithIndex = Path.Combine(filePath, fileName);

        while (File.Exists(filePathWithIndex))
        {
            fileName = baseFileName + fileIndex + fileExtension;
            filePathWithIndex = Path.Combine(filePath, fileName);
            fileIndex++;
        }

        return filePathWithIndex;
    }

    public void WriteDataToFile()
    {
        if(!File.Exists(dataFile))
        {
            File.WriteAllText(dataFile, "3D PARALLAX TEST DATA\n");
            File.WriteAllText(dataFile, $"{testDataResults[0].tag}\n");
            print("File created");
        }

        // 태그(2d/3d), 방향값 벡터(x,y,z), 실제 위치(x,y,z), 추정위치(x,y,z) | 기록 시간
        foreach (TestDataValue data in testDataResults)
        {
            string result = $"{data.trueVector}/{data.inputVector}/{data.time}/{data.longitude}/{data.latitude}/{data.containedAngle}";
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
