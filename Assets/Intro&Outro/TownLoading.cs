﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownLoading : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Intro2");
    }
}
