using UnityEngine;
using System.Collections.Generic;
using Coroutines;

public class GameMachine : MonoBehaviour
{
    public GameMachine Instance { get; private set; }
    public int Points { get; set; }


    Coroutines.Coroutine _Main;

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

