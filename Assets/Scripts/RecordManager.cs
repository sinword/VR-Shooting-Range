using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RecordManager : MonoBehaviour
{
    string path;
    void Awake() {
        // path = Path.Combine(Application.persistentDataPath, "Record.json");
        // Debug.LogWarning("RecordManager path: " + path);
        path = "Record.json";
    }

    public List<RecordInfo> GetRecords() {
        List<RecordInfo> recordInfoList = FileHandler.ReadFromJSON<RecordInfo>(path);
        Debug.LogWarning("Number of records loaded: " + recordInfoList.Count);
        return recordInfoList;
    }

    public void SaveRedcord(RecordInfo recordInfo) {
        List<RecordInfo> recordInfoList = GetRecords();
        // Sort the list by score, from high to low
        Debug.LogWarning("Playername: " + recordInfo.playerName + " Score: " + recordInfo.score);
        recordInfoList.Add(recordInfo);
        recordInfoList.Sort((x, y) => y.score.CompareTo(x.score));
        FileHandler.SaveToJSON<RecordInfo>(recordInfoList, path);
        
    }
}
