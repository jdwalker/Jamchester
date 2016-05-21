using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Coroutines;
using Random = UnityEngine.Random;

public class CreditsScroller : MonoBehaviour
{
    [SerializeField]
    Text _StartingText;

    [SerializeField]
    Text _ScrollText1, _ScrollText2, _ScrollText3;


    Coroutines.Coroutine _Main;
    float shakeTimer = 0f;
    float[] timesForShake = new float[] { 1, 2, 1, 3, 2, 4, 1, 1, 3 };
    int shakeCount = 0;

    // Use this for initialization
    void Start()
    {
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
        shakeTimer += Time.deltaTime;
        if (shakeTimer > timesForShake[shakeCount])
        {
            shakeCount++;
            if (shakeCount >= timesForShake.Length)
                shakeCount = 0;
            shakeTimer = 0f;

            var dir = Random.Range(0, 2) == 0;
            var moveTo = (dir ? 1f : -1f) * 0.075f;
            var time = 0.035f;

            LeanTween.moveLocalX(gameObject, moveTo, time);
            //.setEase(LeanTweenType.easeInOutBounce);

            LeanTween.moveLocalX(gameObject, 0, time)
                //.setEase(LeanTweenType.easeInOutBounce)
                .setDelay(time);

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

        yield return ControlFlow.Call(ScrollCredits());
    }

    IEnumerable<Instruction> Wait(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerable<Instruction> FadeIn(float duration)
    {
        //_Text.CrossFadeAlpha(1f, duration, false);
        LeanTween.textColor(_StartingText.rectTransform, Color.black, duration);

        yield return ControlFlow.Call(Wait(duration));
    }

    IEnumerable<Instruction> ScrollCredits()
    {
        var timePerScreen = 10f;

        PopulateCredits(_ScrollText1, true);

        var startY = _StartingText.rectTransform.localPosition.y;
        LeanTween.moveLocalY(_StartingText.gameObject, startY + 100f, timePerScreen);


        var texts = new[] { _ScrollText1, _ScrollText2, _ScrollText3 };
        var nextTextIdx = 1;

        var first = true;

        while (true)
        {
            startY = _ScrollText1.rectTransform.localPosition.y;
            LeanTween.moveLocalY(_ScrollText1.gameObject, startY + 100f, timePerScreen);

            startY = _ScrollText2.rectTransform.localPosition.y;
            LeanTween.moveLocalY(_ScrollText2.gameObject, startY + 100f, timePerScreen);

            startY = _ScrollText3.rectTransform.localPosition.y;
            LeanTween.moveLocalY(_ScrollText3.gameObject, startY + 100f, timePerScreen);

            yield return ControlFlow.Call(Wait(timePerScreen));

            PopulateCredits(texts[nextTextIdx], true);

            if (first)
                first = false;
            else
            {
                var prevTextIdx = (nextTextIdx - 1);
                if (prevTextIdx == -1)
                    prevTextIdx = texts.Length - 1;

                nextTextIdx = (nextTextIdx + 1) % texts.Length;
                texts[prevTextIdx].rectTransform.localPosition += new Vector3(0, -300f, 0);
            }
        }
    }





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
        int linesPerScreen = 20;
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
