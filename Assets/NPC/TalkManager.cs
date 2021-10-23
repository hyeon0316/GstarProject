using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕", "이 곳에 처음 왔구나?" });
        talkData.Add(2000, new string[] { "안녕2", "안녕3" });
        talkData.Add(100, new string[] { "평범한 공." });
    }

    public string GetTalk(int id,int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
