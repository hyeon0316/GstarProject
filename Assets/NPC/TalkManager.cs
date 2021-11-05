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
        talkData.Add(4000, new string[] { "어이.", "왼쪽으로는 안가는게 좋을거야" });
        talkData.Add(5000, new string[] { "너는 누구지?.", "스킬은 배우고 오는게 좋을거야"});
        talkData.Add(6000, new string[] { "흠냐.. 흠냐..zZ", "졸고 있는거 같다 조용히 가자" });
        talkData.Add(7000, new string[] { "빨리 재료를 다시 모아야 할텐데", });
        talkData.Add(8000, new string[] { "텔레포트가 어디가잘못된거지..?", "스승님한테 혼나겠다.." });
        talkData.Add(9000, new string[] { "던전은 위험합니다", "돌아가세요" });
        //Quest

        talkData.Add(8000 + 10 + 0, new string[] { "어...? 너가 거기서 왜나와", "우리가 타야하는 텔레포트였다고!", "?????", "후.. 일단 스승님한테 가봐" });
        talkData.Add(7000 + 10 + 1, new string[] { "일단.. 자네도 원래 있던곳으로 돌아가야 하지 않나",
            "우리는 지금 바뻐서 재료를 다시 모을수가 없네","자네가 대신 해줘야 할거같은데",
            "마력과 아이템을 조금 나눠주겠네.", "지금은 기본공격 밖에 할수없을거네.",
            "재료를 모아와주면 다른 스킬들을 주겠네.", "광산에가서 동굴거미의 심장을 5개 가져와 주시게" });
        talkData.Add(7000 + 10 + 2, new string[] { "아직 다 못구했나? " });
        talkData.Add(7000 + 10 + 3, new string[] { "고생했네", "금방 스킬을 만들어 주겠네", "다시한번 말을 걸어주시게" });
        talkData.Add(7000 + 10 + 4, new string[] { "스킬과 경험치 조금 이네" });



        talkData.Add(7000 + 20 + 0, new string[] { "소피아양 미안하네", "텔레포트를 구현하느라 마력이 조금 부족해서 스킬을 전부 주지 못했네",
                    "광산2에가서 본그리브 잡다보면 반지가 나올걸세","반지좀 구해다 주게나 그럼 나머지 스킬들을 주겠네" });
        talkData.Add(7000 + 20 + 1, new string[] { "다구했나??" });
        talkData.Add(7000 + 20 + 2, new string[] { "아직 반지는 못구했나보군" });
        talkData.Add(7000 + 20 + 3, new string[] { "고생했네", "금방 스킬을 만들어 주겠네", "다시한번 말을 걸어주시게" });
        talkData.Add(7000 + 20 + 4, new string[] { "스킬을 다만들었네 ! 포션도 조금 주겠네 조심하게" });

        talkData.Add(7000 + 30 + 0, new string[] { "소피아양 이제 내가 줄수있는건 다줬네", "던전 입구에 있는 기사단장 누리스한테 가보게","그녀가 도움을줄거야" });

        talkData.Add(9000 + 30 + 1, new string[] { "당신이 소피아양인가요?", "던전은 매우위험합니다 \n 사정은 들었습니다.", "어쩔수 없네요 숙련자의 장비를 드리겠습니다.", "무운을 빕니다", "리치의 정보는 던전안의 유령이 알고 있을것 입니다." }) ;
        
        
        talkData.Add(4000 + 40 + 0, new string[] { "어서와! 어서와!","나? 나는 예전에 리치에게 \n도전했다가 패배해서 이렇게 변했지 머람~",
            "너가 촌장님이 말한 친구지!","리치를 잡아야한다고?\n 그럼 내부탁도 들어줘 그럼 방법을 알려줄게!","던전 1번방에가서 고블린을잡고 증표 30개를 모아줘!"});
        talkData.Add(4000 + 40 + 1, new string[] { "아직 증표를 다 못구한거같은데?" });
        talkData.Add(4000 + 40 + 2, new string[] { "이야 벌써구해왔어? ","보상으로 포션이랑 경험치 조금 줄게!","다시 말걸어줘!"});
        talkData.Add(4000 + 40 + 3, new string[] { "여기 보상이야!","리치한테가는법이 궁금하다고헀지?",
            "리치방에 갈려면 보물방에 있는 \"마력 제어 목걸이\"가 있어야하는데","보물방 열쇠 지금 두개로 나눠져있어","던전 2,3번방에서 몬스터를 잡다보면 하나씩나올거야!","보물방열쇠 두개를 나한테 가져오면 내가 보물방을 열어줄게!" });

        talkData.Add(4000 + 50 + 0, new string[] { "아직 열쇠조각을 다 못구한거같은데?" });
        talkData.Add(4000 + 50 + 1, new string[] { "열쇠조각 다구했구나! 다시말을 걸어줘 금방 보물방을 열어줄게" });
        talkData.Add(4000 + 50 + 2, new string[] { "보물방은 던전1에서 앞으로 쭉가면 나올거야 ","보물방을 잘찾아보면 \"마력제어목걸이\"와 대마법사의 방어구와 무기가있으니까 구해서가는게 좋을거야","\"마력제어목걸이\"를 가져오면 보스방을 열어줄게" });



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
