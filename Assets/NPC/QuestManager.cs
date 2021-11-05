using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager inst = null;
    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;
    public Quest[] quest;
    public GameObject[] gameObjects;
    bool firstQuset;
    void Awake()
    {
        if (inst == null) // 싱글톤
        {
            inst = this;
        }
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        firstQuset = true;
    }
    void GenerateData()
    {
        questList.Add(10, new QuestData("촌장이랑 대화하기.", new int[] { 8000, 7000, 7000, 7000, 7000 }));

        questList.Add(20, new QuestData("반지 찾아주기.", new int[] { 7000, 7000, 7000, 7000, 7000 }));

        questList.Add(30, new QuestData("기사 단장한테 말걸기.", new int[] { 7000, 9000 }));

        questList.Add(40, new QuestData("리치정보 얻기", new int[] { 4000, 4000, 4000, 4000 }));

        questList.Add(50, new QuestData("보물방 열쇠얻기", new int[] { 4000, 4000, 4000, 4000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        Debug.Log("qusetActionindex:" + questActionIndex + "\n id:" + id);
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        Debug.Log("qusetActionindex:" + questActionIndex + "\n id:" + id);
        ControlObject();

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();
        Debug.Log("qusetActionindex:" + questActionIndex + "\n id:" + id);

        return questList[questId].questName;
    }
    public string CheckQuest()
    {
        //맨처음 퀘스트 알려주기
        return questList[questId].questName;
    }
    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 2)
                {
                    Player.inst.questIng = quest[1];
                    Player.inst.questIng.state = QuestState.Progressing;
                    foreach (var qu in quest[0].rewards)
                    {
                        qu.Reward();
                    }
                }
                if (questActionIndex == 3)
                {
                    if (firstQuset)
                    {
                        firstQuset = false;
                    }
                    questActionIndex = 2;
                }
                if (questActionIndex == 5)
                {
                    foreach (var qu in Player.inst.questIng.rewards)
                    {
                        qu.Reward();
                    }
                    Player.inst.isSkillQ = true;
                    Player.inst.isSkillW = true;
                    Player.inst.coolTimeQ.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.coolTimeW.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.questIng = null;
                }
                break;

            case 20:
                if (questActionIndex == 1)
                {

                    Player.inst.questIng = quest[2];
                    Player.inst.questIng.state = QuestState.Progressing;
                }
                if (questActionIndex == 3)
                {
                    questActionIndex = 2;
                }
                if (questActionIndex == 5)
                {
                    foreach (var qu in Player.inst.questIng.rewards)
                    {
                        qu.Reward();
                    }

                    Player.inst.isSkillE = true;
                    Player.inst.isSkillR = true;
                    Player.inst.coolTimeE.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.coolTimeR.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.questIng = null;
                }
                break;
            case 30:
                if (questActionIndex == 2)
                {
                    foreach (var qu in quest[3].rewards)
                    {
                        qu.Reward();
                    }
                }
                break;
            case 40:
                if (questActionIndex == 1)
                {
                    Player.inst.questIng = quest[4];
                }
                if (questActionIndex == 2)
                {
                    questActionIndex = 1;
                }
                if (questActionIndex == 4)
                {
                    foreach (var qu in Player.inst.questIng.rewards)
                    {
                        qu.Reward();
                    }
                }
                break;
        }

    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
}
