﻿using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    [SerializeField] private GameObject MainHelpCanvas;
    [SerializeField] private TextMeshPro helpText;
    [SerializeField] private TextMeshPro followText;

    void Start()
    {
        if (!MainHelpCanvas) 
            MainHelpCanvas = GameObject.Find("HelpCommands");
        if (!helpText) 
            helpText = new TextMeshPro();
        if (!followText)
            followText = new TextMeshPro();
    }
    public void ChangeTextHelp()
    {
        if (MainHelpCanvas.activeSelf)
            helpText.text = "Say \"Help Off\"";
        else
            helpText.text = "Say \"Help On\"";
    }
    public void ChangeTextFollow()
    {
        if (MainHelpCanvas.activeSelf)
            followText.text = "Say \"Follow Off\"";
        else
            followText.text = "Say \"Follow On\"";
    }
}