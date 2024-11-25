using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    // Voice command vars
    private Dictionary<string, Action> keyActs = new Dictionary<string, Action>();
    private KeywordRecognizer recognizer;

    //Var needed for color manipulation
    private MeshRenderer cubeRend;

    //Var needed for spin manipulation
    private bool spinningRight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
