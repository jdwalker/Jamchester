﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Coroutines;
using Random = UnityEngine.Random;

public class CreditsScroller : MonoBehaviour
{
    public static CreditsScroller Instance { get; set; }

    [SerializeField]
    Text _StartingText;

    [SerializeField]
    Text _ScrollText1, _ScrollText2, _ScrollText3;

    [SerializeField]
    public Canvas _OverlayCanvas;


    Coroutines.Coroutine _Main;
    float shakeTimer = 0f;
    float[] timesForShake = new float[] { 1, 2, 1, 3, 2, 4, 1, 1, 3 };
    int shakeCount = 0;
    bool isEnding = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1.0f;
    }

    // Use this for initialization
    void Start()
    {
        _OverlayCanvas.gameObject.SetActive(false);
        _Main = new Coroutines.Coroutine(Main());
    }

    // Update is called once per frame
    void Update()
    {
        _Main.Update();

        CreditsShake();
    }

    void CreditsShake()
    {
        shakeTimer += Time.unscaledDeltaTime;
        if (shakeTimer > timesForShake[shakeCount])
        {
            shakeCount++;
            if (shakeCount >= timesForShake.Length)
                shakeCount = 0;
            shakeTimer = 0f;

            var dir = Random.Range(0, 2) == 0;
            var moveTo = (dir ? 1f : -1f) * 0.075f;
            var time = 0.035f;

            GameMachine.Instance.OpenTween("CreditsScroller", 65);
            LeanTween.moveLocalX(gameObject, moveTo, time).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 65);
            //.setEase(LeanTweenType.easeInOutBounce);

            GameMachine.Instance.OpenTween("CreditsScroller", 69);
            LeanTween.moveLocalX(gameObject, 0, time)
                //.setEase(LeanTweenType.easeInOutBounce)
                .setDelay(time).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 69);

            //LeanTween.moveLocalX(gameObject, 0, 0.1f)
            //    //.setEase(LeanTweenType.easeInOutBounce)
            //    .setDelay(0.3f);
        }
    }


    //public void T


    // Root coroutine
    IEnumerable<Instruction> Main()
    {
        _StartingText.text = StaticIntro;
        _StartingText.color = new Color(0, 0, 0, 0);

        yield return ControlFlow.Call(Wait(2));

        yield return ControlFlow.Call(FadeIn(2));

        yield return ControlFlow.Call(Wait(4));

        LeanTween.delayedCall(10f, GameMachine.Instance.TransitionToPlayer);

        yield return ControlFlow.ExecuteWhile(() => !isEnding,
            ScrollCredits());

        yield return ControlFlow.Call(RunEndGameCredits());
    }

    IEnumerable<Instruction> Wait(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerable<Instruction> FadeIn(float duration)
    {
        GameMachine.Instance.OpenTween("CreditsScroller", 115);
        LeanTween.textColor(_StartingText.rectTransform, Color.black, duration).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 115);

        yield return ControlFlow.Call(Wait(duration));
    }

    IEnumerable<Instruction> ScrollCredits()
    {
        var timePerScreen = 10f;

        PopulateCredits(_ScrollText1, true);
        PopulateCredits(_ScrollText2);
        PopulateCredits(_ScrollText3);

        var startY = _StartingText.rectTransform.localPosition.y;
        GameMachine.Instance.OpenTween("CreditsScroller", 130);
        LeanTween.moveLocalY(_StartingText.gameObject, startY + 100f, timePerScreen).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 130);


        var texts = new List<Text>() { _ScrollText1, _ScrollText2, _ScrollText3 };


        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                startY = texts[i].rectTransform.localPosition.y;
                GameMachine.Instance.OpenTween("CreditsScroller", 142);
                LeanTween.moveLocalY(texts[i].gameObject, startY + 100f, timePerScreen).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 142);
            }

            yield return ControlFlow.Call(Wait(timePerScreen));

            GameMachine.Instance.StartGame();
        }

        while (true)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                startY = texts[i].rectTransform.localPosition.y;
                GameMachine.Instance.OpenTween("CreditsScroller", 156);
                LeanTween.moveLocalY(texts[i].gameObject, startY + 100f, timePerScreen).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 156);
            }

            yield return ControlFlow.Call(Wait(timePerScreen));


            var moveToEnd = texts[0];
            texts.RemoveAt(0);
            texts.Add(moveToEnd);

            moveToEnd.rectTransform.localPosition += new Vector3(0, -300f, 0);
            PopulateCredits(moveToEnd);
        }
    }

    IEnumerable<Instruction> RunEndGameCredits()
    {
        _OverlayCanvas.gameObject.SetActive(true);
        _StartingText.rectTransform.localPosition = new Vector3(_StartingText.rectTransform.localPosition.x, -100f, _StartingText.rectTransform.localPosition.z);
        _StartingText.rectTransform.LookAt(-Camera.main.transform.position);
        _StartingText.color = Color.white;

        //_OverlayCanvas.transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(1f, 5f, true);
        LeanTween.color(_OverlayCanvas.transform.GetChild(0).transform as RectTransform, Color.black, 5f).setUseEstimatedTime(true);
        var text = _OverlayCanvas.transform.GetChild(1).GetComponent<Text>();
        //text.gameObject.SetActive(true);
        text.color = new Color(1f, 1f, 1f, 0f);
        //text.CrossFadeAlpha(1f, 5f, true);
        //LeanTween.color(text.rectTransform, new Color(1f, 1f, 1f, 1f), 5f).setUseEstimatedTime(true);
        LeanTween.textAlpha(text.rectTransform, 1f, 5f).setUseEstimatedTime(true);
        text.text = LargeSize + "Your score!\n" + EndSize + GameMachine.Instance.Points + "\n\nProperty Damage: " + GameMachine.Instance.Damage.ToString("C") + "\n\nHighest Multiplier Gained: " + Multiplier.Highest;
        //LeanTween.value(gameObject,
        //    c => {
        //        _OverlayCanvas.transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(1f, 5f, true);
        //    },
        //    new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), 5f);
        GameMachine.Instance.OpenTween("CreditsScroller", 190);
        LeanTween.moveLocalY(_StartingText.gameObject, 0f, 10f).onComplete = () => GameMachine.Instance.CloseTween("CreditsScroller", 190);

        yield return ControlFlow.Call(Wait(10f));

        _OverlayCanvas.transform.GetChild(2).gameObject.SetActive(true);
    }



    #region Public API

    public void EndGame()
    {
        isEnding = true;
    }

    public void DisableCreditOverlay()
    {
        var t = transform.GetChild(0);

        foreach (Transform child in t)
        {
            var text = child.GetComponent<Text>();
            text.material = null;
        }
    }

    #endregion



    private const string BigNameSize = "<size=10>";
    private const string LargeSize = "<size=14>";
    private const string SuperLargeSize = "<size=20>";

    private const string EndSize = "</size>";

    private string StaticIntro
    {
        get
        {
            return
                LargeSize + "\n\n\nA\n" + EndSize
                + SuperLargeSize + "Dark Field\n" + EndSize
                + LargeSize + "Production" + EndSize;
        }
    }

    private void PopulateCredits(Text text, bool startingText = false)
    {
        const int linesPerScreen = 15;

        var sb = new StringBuilder();

        if (startingText)
        {
            sb.AppendLine("Producer");
            sb.AppendLine(BigNameSize + "Blair Knickerbocker" + EndSize);
            sb.AppendLine();

            sb.AppendLine("Directed by");
            sb.AppendLine(BigNameSize + "James Walker" + EndSize);
            sb.AppendLine();

            sb.AppendLine("Sound Direction by");
            sb.AppendLine(BigNameSize + "Katie Tarrant" + EndSize);
            sb.AppendLine();

            sb.AppendLine("Art Direction by");
            sb.AppendLine(BigNameSize + "Rachael Jones" + EndSize);
            sb.AppendLine();
        }
        else
        {
            switch (Random.Range(0, 10))
            {
                case 0:
                case 1:
                case 2:
                    //   Job Title
                    // Name1   Name2


                    sb.AppendLine(BigNameSize + JobTitleGen.GetJobTitlePlural() + EndSize);
                    sb.AppendLine();

                    var names2 = new KeyValuePair<string, string>[linesPerScreen - 2];
                    for (int i = 0; i < names2.Length; i++)
                    {
                        names2[i] = new KeyValuePair<string, string>(NameGen.GetName(), NameGen.GetName());
                    }

                    var maxNameWidth = names2.SelectMany(n => new[] { n.Key, n.Value }).OrderByDescending(n => n.Length).First().Length;

                    var format = string.Format("{{0,-{0}}}   {{1,{0}}}", maxNameWidth);

                    for (int i = 0; i < names2.Length; i++)
                    {
                        sb.AppendLine(string.Format(format, names2[i].Key, names2[i].Value));
                    }

                    break;


                case 3:
                case 4:
                case 5:
                    //   Job Title
                    // Name1   Name2
                    //   Job Title
                    // Name1   Name2

                    const int linesPerBlock = 4;

                    for (int j = 0; j < 2; j++)
                    {
                        sb.AppendLine(BigNameSize + JobTitleGen.GetJobTitlePlural() + EndSize);
                        sb.AppendLine();

                        var names3 = new KeyValuePair<string, string>[linesPerBlock];
                        for (int i = 0; i < linesPerBlock; i++)
                        {
                            names3[i] = new KeyValuePair<string, string>(NameGen.GetName(), NameGen.GetName());
                        }

                        maxNameWidth = names3.SelectMany(n => new[] { n.Key, n.Value }).OrderByDescending(n => n.Length).First().Length;
                        format = string.Format("{{0,-{0}}}   {{1,{0}}}", maxNameWidth);
                        
                        for (int i = 0; i < names3.Length; i++)
                        {
                            sb.AppendLine(string.Format(format, names3[i].Key, names3[i].Value));
                        }

                        sb.AppendLine();
                    }



                    break;


                case 6:
                case 7:
                case 8:

                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            sb.AppendLine(LargeSize + "Special Thanks to" + EndSize);
                            break;

                        case 1:
                            sb.AppendLine(LargeSize + "Extras" + EndSize);
                            break;
                    }
                    sb.AppendLine();
                    
                    names2 = new KeyValuePair<string, string>[linesPerScreen - 2];
                    for (int i = 0; i < names2.Length; i++)
                    {
                        names2[i] = new KeyValuePair<string, string>(JobTitleGen.GetJobTitle(), NameGen.GetName());
                    }

                    maxNameWidth = names2.SelectMany(n => new[] { n.Key, n.Value }).OrderByDescending(n => n.Length).First().Length;
                    format = string.Format("<b>{{0,-{0}}}</b>   {{1,{0}}}", maxNameWidth);

                    for (int i = 0; i < names2.Length; i++)
                    {
                        sb.AppendLine(string.Format(format, names2[i].Key, names2[i].Value));
                    }

                    break;

                case 9:

                    // Production babies!

                    sb.AppendLine(LargeSize + "Production Babies!" + EndSize);
                    sb.AppendLine();
                    
                    names2 = new KeyValuePair<string, string>[linesPerScreen - 2];
                    for (int i = 0; i < names2.Length; i++)
                    {
                        names2[i] = new KeyValuePair<string, string>(NameGen.GetName(), NameGen.GetName());
                    }

                    maxNameWidth = names2.SelectMany(n => new[] { n.Key, n.Value }).OrderByDescending(n => n.Length).First().Length;
                    format = string.Format("{{0,-{0}}}   {{1,{0}}}", maxNameWidth);

                    for (int i = 0; i < names2.Length; i++)
                    {
                        sb.AppendLine(string.Format(format, names2[i].Key, names2[i].Value));
                    }

                    break;

                case 10:


                    break;
            }
        }

        text.text = sb.ToString();
    }


    #region Privates

    private string NewLine(int n = 1)
    {
        if (n == 1)
            return "\n";

        var sb = new StringBuilder();

        for (int i = 0; i < n; i++)
            sb.Append("\n");

        return sb.ToString();
    }

    #endregion
}
