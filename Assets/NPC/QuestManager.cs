using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class QuestManager : MonoBehaviour
{
    public static QuestManager inst = null;
    public int questId;
    public int questActionIndex;
    Dictionary<int, QuestData> questList;
    public Quest[] quest;
    public GameObject[] questObjects;
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

        questList.Add(60, new QuestData("보스방 열쇠얻기", new int[] { 4000, 4000, 4000, 4000 }));

        questList.Add(70, new QuestData("리치를 잡고 집에!", new int[] { 4000, 4000, 7000, 7000 }));
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
        Player.inst.questTitleText.text = questList[questId].questName;
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        Player.inst.questTitleText.text = questList[questId].questName;
        return questList[questId].questName;
    }
    public void InitPanl()
    {
        string questText;
        questText = "";
        foreach (var obj in Player.inst.questIng.collectObjectives)
        {
            obj.UpdateItemCount();
            questText += obj.item.itemName + "\n" + obj.currentAmount + " / " + obj.amount + "\n";
        }
        Player.inst.questProText.text = questText;
    }
    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if (questActionIndex == 1)
                {
                    NpcManager.inst.witchNpc = false;
                    NpcManager.inst.wizardNpc = true;
                    NpcManager.inst.NpcCheck();
                }
                if (questActionIndex == 2)
                {
                    Player.inst.questIng = quest[1];
                    InitPanl();
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
                    Player.inst.questProText.text = "";
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
                    InitPanl();
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
                    Player.inst.questProText.text = "";
                    Player.inst.isSkillE = true;
                    Player.inst.isSkillR = true;
                    Player.inst.coolTimeE.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.coolTimeR.transform.GetChild(0).gameObject.SetActive(false);
                    Player.inst.questIng = null;
                }
                break;
            case 30:
                if (questActionIndex == 1)
                {
                    NpcManager.inst.wizardNpc = false;
                    NpcManager.inst.knightNpc = true;
                    NpcManager.inst.NpcCheck();
                }
                if (questActionIndex == 2)
                {
                    foreach (var qu in quest[3].rewards)
                    {
                        qu.Reward();
                    }
                    NpcManager.inst.knightNpc = false;
                    NpcManager.inst.ghostNpc = true;
                    NpcManager.inst.townRoom = true;
                    NpcManager.inst.PotalCheck();
                }
                break;
            case 40:
                if (questActionIndex == 1)
                {
                    Player.inst.questIng = quest[4];
                    InitPanl();
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
            case 50:
                if (questActionIndex == 1)
                {
                    Player.inst.questIng = quest[5];
                    InitPanl();
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
                    NpcManager.inst.dunTea = true;
                }
                break;
            case 60:
                if (questActionIndex == 1)
                {
                    Player.inst.questIng = quest[6];
                    InitPanl();
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
                    NpcManager.inst.roomBoss = true;
                    Player.inst.questIng = quest[8];
                    InitPanl();
                }
                break;
            case 70:
                if (questActionIndex == 1)
                {
                    foreach (var qu in quest[7].rewards)
                    {
                        qu.Reward();
                    }
                    questActionIndex = 0;
                }
                if (questActionIndex == 4)
                {
                    GameObject player;
                    GameObject uiCanvas;
                    player = GameObject.Find("PlayerM 2");
                    uiCanvas = GameObject.Find("UICanvas 1");
                    Destroy(player);
                    Destroy(uiCanvas);
                    LoadingSceneManager.LoadScene("Outro");

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
