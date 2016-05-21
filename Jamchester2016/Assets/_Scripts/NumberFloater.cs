using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Coroutines;

public class NumberFloater : MonoBehaviour
{
    void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 1, 0.4f);
        LeanTween.delayedCall(0.4f, () => Pool());
    }

    public void SetText(int points)
    {
        var child = transform.GetChild(0).GetComponent<TextMesh>();
        child.text = points.ToString();
        Start();
    }


    private static List<NumberFloater> _pool = new List<NumberFloater>();
    private static NumberFloater _prefab;

    private void Pool()
    {
        _pool.Add(this);
        gameObject.SetActive(false);
    }

    public static NumberFloater Get()
    {
        if (_pool.Count > 0)
        {
            var item = _pool[0];
            _pool.RemoveAt(0);
            item.gameObject.SetActive(true);
            return item;
        }
        else
            return GameObject.Instantiate(_prefab);
    }

    public static void SetPrefab(NumberFloater prefab)
    {
        _prefab = prefab;
    }
}

