using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();


    }
    void GenerateData()
    {
        questList.Add(10, new QuestData("마을 사람들과 대화하기.", new int[] { 1000, 2000 }));

        questList.Add(20, new QuestData("동전 찾아주기.", new int[] { 5000, 2000 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
            questActionIndex++;

        ControlObject();

        if (questActionIndex == questList[questId].npcId.Length)
            NextQuest();

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
                break;
            case 20:
                break;
        }

    }
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
}
