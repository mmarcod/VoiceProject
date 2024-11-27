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
    private bool movingRight;


    //Var needed for sound playback.
    private AudioSource soundSource;
    public AudioClip[] sounds;

    public GameObject cube;

    
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

        //Voice command to Destroy cube
        keyActs.Add("destroy cube", DestroyObject);
        keyActs.Add("create cube", CreateCube);

        //Voice commands for opening link
        keyActs.Add("open link", OpenLink);

        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();

        //Voice commands for moving cube
        keyActs.Add("move", MoveRight);
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

    void MoveRight()
    {
        movingRight = true;
        StartCoroutine(MoveObject(5f, 2f));
    }

    void DestroyObject()
    {
        cube.SetActive(false);
    }

    void CreateCube()
    {
        cubeRend.material.SetColor("_Color", Color.white);

        cube.SetActive(true);
    }
    void OpenLink()
    {
        Application.OpenURL("www.google.com");
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

    private IEnumerator MoveObject(float height, float duration)
    {
        Vector3 originalPosition = gameObject.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * height;

        float elapsedTime = 0;

        while (elapsedTime < duration / 2f)
        {
            gameObject.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / (duration / 2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
        elapsedTime = 0;

        while (elapsedTime < duration / 2f)
        {
            gameObject.transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / (duration / 2f));
            elapsedTime += Time.deltaTime;
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
