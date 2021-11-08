using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NpcManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static NpcManager inst = null;
    public bool wizardNpc;
    public bool ghostNpc;
    public bool knightNpc;
    public bool witchNpc;
    public bool townRoom;
    public bool dunTea;
    public bool roomBoss;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        witchNpc = false;
        ghostNpc = false;
        knightNpc = false;
        witchNpc = false;
        townRoom = false;
        dunTea = false;
        roomBoss = false;
    }
    // Update is called once per frame
    public void NpcCheck()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject h;
        if (sceneName == "Town")
        {

            h = GameObject.Find("Wizard");
            h.GetComponent<ObjData>().questQ.SetActive(wizardNpc);
            h = GameObject.Find("Witch");
            h.GetComponent<ObjData>().questQ.SetActive(witchNpc);
            h = GameObject.Find("KnightFemale");
            h.GetComponent<ObjData>().questQ.SetActive(knightNpc);
        }
        if (sceneName == "Room")
        {
            h = GameObject.Find("Ghost");
            h.GetComponent<ObjData>().questQ.SetActive(ghostNpc);
        }
    }

    public void PotalCheck()
    {
        GameObject h;
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Town")
        {
            h = GameObject.Find("TownRoom");
            h.transform.GetChild(0).gameObject.SetActive(townRoom);
            h.transform.GetChild(1).gameObject.SetActive(townRoom);
        }
        if (sceneName == "Dungeon")
        {
            h = GameObject.Find("dunTea");
            h.transform.GetChild(0).gameObject.SetActive(dunTea);
            h.transform.GetChild(1).gameObject.SetActive(dunTea);
        }
        
    }
}
