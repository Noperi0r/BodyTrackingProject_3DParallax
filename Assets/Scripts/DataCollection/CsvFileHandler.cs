using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CsvFileHandler : MonoBehaviour
{

    string filePath = Application.streamingAssetsPath + "/DataCollection/"; // Assets/StreamingAssets/DataCollection
    string baseFileName = "TestData";
    string fileExtension = ".csv";
    string dataFile;

    // 데이터 배열
    public static List<string> timeLog = new List<string>();
    public static List<string> TruePos = new List<string>();
    public static List<string> PredictedPos = new List<string>();

    public void MakeCsv()
    {
        // 배열에 데이터 비어있으면 예외처리되어 저장 안됩니다.
        try
        {
            using (StreamWriter sw = new StreamWriter(GetUniqueFilePath(), false, Encoding.UTF8))
            {
                sw.WriteLine("Consumed time,TruePos,PredictedPos");
                var Timelist = timeLog;
                var TruePosList = TruePos;
                var PredictedPosList = PredictedPos;
                for (int i = 0; i < Timelist.Count; i++)
                {
                    var tmp1 = Timelist[i];
                    var tmp2 = TruePosList[i];
                    var tmp3 = PredictedPosList[i];
                    sw.WriteLine(string.Format("{0},{1},{2}", tmp1, tmp2, tmp3));
                }
            }
            Debug.Log("CSV file saved");

        }
        catch (Exception ex)
        {
            Debug.Log("An error occurred while writing to the CSV file: " + ex.Message);
        }

    }
    string GetUniqueFilePath()
    {
        int fileIndex = 1;
        string fileName = baseFileName + fileExtension;
        string filePathWithIndex = Path.Combine(filePath, fileName);

        while (File.Exists(filePathWithIndex))
        {
            fileName = baseFileName + fileIndex + fileExtension;
            filePathWithIndex = Path.Combine(filePath, fileName);
            fileIndex++;
        }

        return filePathWithIndex;
    }

}