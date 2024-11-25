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

    //Var needed for sound playback.
    private AudioSource soundSource;
    public AudioClip[] sounds;

    
    void Start()
    {
        cubeRend = GetComponent<MeshRenderer>();
        soundSource = GetComponent<AudioSource>();

        //Voice commands for changing color
        keyActs.Add("red", Red);
        keyActs.Add("green", Green);
        keyActs.Add("blue", Blue);
        keyActs.Add("white", White);

        //Voice commands for spinning
        keyActs.Add("spin right", SpinRight);
        keyActs.Add("spin left", SpinLeft);

        //Voice commands for playing sounds.
        keyActs.Add("please say something", Talk);

        //Voice command to show how complex it can get.
        keyActs.Add("pizza is a wonderful food that makes me very happy", FactAcknowledgement);

        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();
    }

    void OnKeywordsRecognized (PhraseRecognizedEventArgs args)
    {
        Debug.Log("Commant: " + args.text);
        keyActs[args.text].Invoke();
    }

        void Red()
    {
        cubeRend.material.SetColor("_Color", Color.red);
    }

    void Green()
    {
        cubeRend.material.SetColor("_Color", Color.green);
    }

    void Blue()
    {
        cubeRend.material.SetColor("_Color", Color.blue);
    }

    void White()
    {
        cubeRend.material.SetColor("_Color", Color.white);
    }

    void SpinRight()
    {
        spinningRight = true;
        StartCoroutine(RotateObject(1f));
    }

    void SpinLeft()
    {
        spinningRight = false;
        StartCoroutine(RotateObject(1f));
    }

    private IEnumerator RotateObject(float duration)
    {
        float StartRot = transform.eulerAngles.x;
        float endRot;

        if (spinningRight)
            endRot = StartRot - 360f;
        else
            endRot = StartRot + 360f;

        float t = 0f;
        float yRot;

        while (t < duration)
        {
            t += Time.deltaTime;
            yRot = Mathf.Lerp(StartRot, endRot, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
            yield return null;
        }
    }

    void Talk()
    {
        soundSource.clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
        soundSource.Play();
    }

    void FactAcknowledgement()
    {
        Debug.Log("How right you are.");
    }
}
