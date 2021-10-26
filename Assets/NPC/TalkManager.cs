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
        talkData.Add(3000, new string[] { "평범한 공." });

        //Quest
        talkData.Add(1000 + 10, new string[] { "어서와", "NPC2에게 가봐" });
        talkData.Add(2000 + 11, new string[] { "잘왔네", "수고했어" });
    }

    public string GetTalk(int id,int talkIndex)
    {
        Debug.Log(id + "," + talkIndex);
        if(!talkData.ContainsKey(id))
        {
            //해당 퀘스트 진행중 대시가 없을때 진행순서
            //퀘스트 맨처음 대사 가지고옴
            if (talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 10, talkIndex);
            }
            else
            {
                //퀘스트 맨처음 대사
                return GetTalk(id - id % 100, talkIndex);
            }
        }
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
