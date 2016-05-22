using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Coroutines;


public class GameMachine : MonoBehaviour
{
    [SerializeField]
    NumberFloater NumberPrefab;

    [SerializeField]
    float GameTime;

    public static GameMachine Instance { get; private set; }
    public int Points { get; set; }
    public int Damage { get; set; }

    float timer = 0f;
    bool isRunning = false;

    Coroutines.Coroutine _Main;

    void Awake()
    {
        if (SceneManager.sceneCount == 1)
            SceneManager.LoadScene("Testbed_vrtest", LoadSceneMode.Additive);

        //if (Camera.main == null)
        //    Camera.main = FindObjectOfType<CharacterController>().transform.GetChild(0).GetComponent<Camera>();

        var p = NumberPrefab.gameObject;
        NumberFloater.SetPrefab(NumberPrefab);
        Instance = this;

    }

    // Use this for initialization
    void Start()
    {
        //_Main = nekw Coroutines.Coroutine(Main());
    }

    // Update is called once per frame
    void Update()
    {
        //_Main.Update();
        if (!isRunning)
            return;

        if (timer > GameTime)
        {
            isRunning = false;
            CreditsScroller.Instance.EndGame();
            OpenTween("GameMachine", 50);
            LeanTween.value(gameObject, f =>
            {
                Time.timeScale = f;
            },
            1f, 0f, 2f).onComplete = () => CloseTween("GameMachine", 50);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        isRunning = true;
    }

    public void Reload()
    {
        Time.timeScale = 1.0f;
        Multiplier.Highest = 0;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }


    //IEnumerable<Instruction> Main()
    //{
    //    yield return null;
    //}



    public class TweenTracker
    {
        public string ClassName;
        public int LineNo;
        public int OpenCount;
        public int CloseCount;
    }

    private List<TweenTracker> _tweenTrackers = new List<TweenTracker>();

    [Conditional("DEBUG")]
    public void OpenTween(string className, int lineNo)
    {
        var tweenTracker = _tweenTrackers.SingleOrDefault(tt => tt.ClassName == className && tt.LineNo == lineNo);
        if (tweenTracker == null)
        {
            tweenTracker = new TweenTracker() { ClassName = className, LineNo = lineNo };
            _tweenTrackers.Add(tweenTracker);
        }

        tweenTracker.OpenCount++;
    }

    [Conditional("DEBUG")]
    public void CloseTween(string className, int lineNo)
    {
        var tweenTracker = _tweenTrackers.SingleOrDefault(tt => tt.ClassName == className && tt.LineNo == lineNo);
        if (tweenTracker == null)
        {
            tweenTracker = new TweenTracker() { ClassName = className, LineNo = lineNo };
            _tweenTrackers.Add(tweenTracker);
        }

        tweenTracker.CloseCount++;
    }
}

