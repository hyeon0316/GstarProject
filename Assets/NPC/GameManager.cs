using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public GameObject skillBG;
    public TypeEffect talk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    public void Awake()
    {
        scanObject = null;
        //Debug.Log(questManager.CheckQuest());
    }
    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0)) && scanObject!=null && isAction)
        {
            ObjData objData = scanObject.GetComponent<ObjData>();
            Talk(objData.id, objData.isNpc);
            talkPanel.SetActive(isAction);
        }
    }
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNpc);
        talkPanel.SetActive(isAction);
        skillBG.SetActive(false);
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex;
        string talkData;

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }
        
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Player.inst.npcCam.SetActive(false);
            skillBG.SetActive(true);
            Player.inst.h.SetActive(true);
            Debug.Log("Talkdata == null  id:"+id+","+questManager.CheckQuest(id));
            return;
        }
        if(isNpc)
        {
            talk.SetMsg(talkData);
        }
        else
        {
            talk.SetMsg(talkData);
        }
        talkIndex++;
        isAction = true;
    }
}
