using System.Collections;
using System.Collections.Generic;

public class QuestData 
{
    public string questName;//퀘스트창에 표시 될 퀘스트명
    public int[] npcId;//

    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}
