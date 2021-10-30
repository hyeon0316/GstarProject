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
        talkData.Add(7000, new string[] { "빨리 재료를 다시 모아야 할텐데", "졸고 있는거 같다 조용히 가자" });
        talkData.Add(8000, new string[] { "텔레포트가 어디가잘못된거지..?", "스승님한테 혼나겠다.." });

        //Quest
        talkData.Add(8000 + 10, new string[] { "어...? 너가 거기서 왜나와", "우리가 타야하는 텔레포트였다고!","?????","후.. 일단 스승님한테 가봐" });
        talkData.Add(7000 + 11, new string[] { "일단.. 자네도 원래 있던곳으로 돌아가야 하지 않나",
            "우리는 지금 바뻐서 재료를 다시 모을수가 없어","자네가 대신 해줘야 할거같은데",
            "마력을 조금 나눠주지. 지금은 기본공격 밖에 할수없을거야.",
            "재료를 모아와주면 다른 스킬을 만들어줄게.", "광산에가서 거미의 전리품을 10개 모아줘" });
        talkData.Add(7000 + 13, new string[] { "아직 다 못구한건가? ", "서둘러줘" });
        talkData.Add(7000 + 14, new string[] { "고생했네", "스킬을 몇개더 주겠네" });
        talkData.Add(7000 + 15, new string[] { "잘왔네2", "수고했어3" });
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
