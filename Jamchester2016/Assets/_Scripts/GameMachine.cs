using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Coroutines;
using Assets._Scripts.Util;

public class GameMachine : MonoBehaviour
{

	[SerializeField]
	public GameObject NumberFloaterPrefab;

    [SerializeField]
    float GameTime;

    public static GameMachine Instance
	{
		get
		{
			return Singleton<GameMachine>.Instance;
		}
			
	}
    public int Points { get; set; }
    public int Damage { get; set; }

	float timer = 0f;
    bool isRunning = false;

    Coroutines.Coroutine _Main;
    CameraStartup _cameraStartup;
    GameObject _mousePlayer, _vrPlayer;

    void Awake()
    {
		if (SceneManager.sceneCount == 1)
			SceneManager.LoadScene("Testbed_vrtest", LoadSceneMode.Additive);

	}

    // Use this for initialization
    void Start()
    {
        var camstart = GameObject.Find("CameraStartup");
        _cameraStartup = camstart.GetComponent<CameraStartup>();
        _cameraStartup.SetSceneCamera(Camera.main);
        camstart.SetActive(false);

        _mousePlayer = GameObject.Find("MousePlayer");
        _vrPlayer = GameObject.Find("VRPlayspace");
        _mousePlayer.SetActive(false);
        _vrPlayer.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        if (_Main != null)
            _Main.Update();

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

    public void TransitionToPlayer()
    {
        _Main = new Coroutines.Coroutine(RunTransition());
    }

    IEnumerable<Instruction> RunTransition()
    {   
        var overlayCanvas = CreditsScroller.Instance._OverlayCanvas;
        var image = overlayCanvas.transform.GetChild(0);
        var text = overlayCanvas.transform.GetChild(1);


        // fade out
        overlayCanvas.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
        LeanTween.color(image.transform as RectTransform, Color.black, 1f);

        yield return ControlFlow.Call(Wait(1f));


        // swap
        _mousePlayer.SetActive(true);
        _vrPlayer.SetActive(true);

        CreditsScroller.Instance.DisableCreditOverlay();

        yield return null;
        Camera.main.gameObject.SetActive(false);
        _cameraStartup.gameObject.SetActive(true);


        // fade in
        LeanTween.color(image.transform as RectTransform, new Color(1f, 1f, 1f, 0f), 1f);

        yield return ControlFlow.Call(Wait(1f));


        // end

        text.gameObject.SetActive(true);
        overlayCanvas.gameObject.SetActive(false);
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

