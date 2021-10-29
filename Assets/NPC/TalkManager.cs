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
        talkData.Add(100, new string[] { "들어가긴 너무 깊어보인다." });
        talkData.Add(200, new string[] { "마른 나무를 구할수 있을것 같다." });
        talkData.Add(300, new string[] { "여기로 내려가는건 좋은생각 같지 않다." });




        talkData.Add(1000, new string[] { "안녕", "이 곳에 처음 왔구나?" });
        talkData.Add(2000, new string[] { "안녕2", "안녕3" });
        talkData.Add(3000, new string[] { "평범한 공." });
        talkData.Add(4000, new string[] { "어이.","왼쪽으로는 안가는게 좋을거야" });
        talkData.Add(5000, new string[] { "너는 누구지?.", "2차 스킬은 배우고 오는게 좋을거야","스킬을 배우고 오자" });
        talkData.Add(6000, new string[] { "흠냐.. 흠냐..zZ","졸고 있는거 같다 조용히 가자"});
        talkData.Add(7000, new string[] { "나는 졸리다", "졸고 있는거 같다 조용히 가자" });

        //Quest
        talkData.Add(1000 + 10, new string[] { "어서와", "NPC2에게 가봐" });
        talkData.Add(2000 + 11, new string[] { "잘왔네", "수고했어" });
        talkData.Add(1000 + 12, new string[] { "잘왔네2", "수고했어3" });
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
