﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void INPUT_Play()
    {
        SceneManager.LoadScene("scene");
    }

    public void INPUT_QUIT()
    {
        Application.Quit();
    }
}
