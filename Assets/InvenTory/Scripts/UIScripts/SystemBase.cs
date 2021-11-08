﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemBase : MonoBehaviour
{
    public static bool systemActivated = false;
    public static bool gamePaused = false;
    [SerializeField]
    private GameObject go_SystemBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory.inventoryActivated && !Information.informationActivated)//인벤창과 장비창이 다꺼져있을때만
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                systemActivated = !systemActivated;
            }
        }
        if (systemActivated)
            OpenSystem();
        else
            CloseSystem();

        if(gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void OpenSystem()
    {
        go_SystemBase.SetActive(true);
        gamePaused = true;
    }

    public void CloseSystem()
    {
        go_SystemBase.SetActive(false);
        gamePaused = false;
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void GoScene()//전체적인 유아이가 같이 따라감(수정)
    {
        SceneManager.LoadScene("Title");
    }

    public void CloseBtn()
    {
        systemActivated = false;
    }
}
