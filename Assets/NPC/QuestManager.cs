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
    
    void Awake()
    {
        if (inst == null) // 싱글톤
        {
            inst = this;
        }
        questList = new Dictionary<int, QuestData>();
        GenerateData();

    }
    void GenerateData()
    {
        questList.Add(10, new QuestData("촌장이랑 대화하기.", new int[] { 8000, 7000, 7000 , 7000 , 7000 }));

        questList.Add(20, new QuestData("반지 찾아주기.", new int[] { 7000, 7000, 7000, 7000, 7000 }));

        questList.Add(30, new QuestData("던전일 돕기.", new int[] { 7000,4000,4000,4000,4000 }));
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

                    Player.inst.questIng = quest[0];
                    Player.inst.questIng.state = QuestState.Progressing;
                }
                if (questActionIndex == 3)
                {
                    questActionIndex = 2;
                }
                if (questActionIndex == 5)
                {
                   foreach(var qu in Player.inst.questIng.rewards)
                    {
                        qu.Reward();

                    }
                    Player.inst.isSkillQ = true;
                    Player.inst.isSkillW = true;
                    Player.inst.coolTimeQ.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.coolTimeW.transform.GetChild(0).gameObject.SetActive(false);
                }
                break;

            case 20:
                if (questActionIndex == 1)
                {

                    Player.inst.questIng = quest[1];
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
                        Player.inst.isSkillE = true;
                        Player.inst.isSkillR = true;
                        Player.inst.coolTimeE.transform.GetChild(0).gameObject.SetActive(false);
                        Player.inst.coolTimeR.transform.GetChild(0).gameObject.SetActive(false);
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
