using UnityEngine;
using System.Collections.Generic;
using Coroutines;

public class GameMachine : MonoBehaviour
{
    [SerializeField]
    NumberFloater NumberPrefab;

    public static GameMachine Instance { get; private set; }
    public int Points { get; set; }
    public int Damage { get; set; }


    Coroutines.Coroutine _Main;

    void Awake()
    {
        var p = NumberPrefab.gameObject;
        NumberFloater.SetPrefab(NumberPrefab);
    }

    // Use this for initialization
    void Start()
    {
        Instance = this;
        _Main = new Coroutines.Coroutine(Main());
    }

    // Update is called once per frame
    void Update()
    {
        _Main.Update();
    }


    IEnumerable<Instruction> Main()
    {
        yield return null;
    }
}

