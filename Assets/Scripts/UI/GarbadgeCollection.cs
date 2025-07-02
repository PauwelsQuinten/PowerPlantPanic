using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class GarbadgeCollection : MonoBehaviour
{
    [Tooltip("Fill in Garbage as tag")]
    [SerializeField]
    private string garbageTag;

    private float _removedGarbage = 0, _totalGarbage, _timer;
    public UnityEvent RemoveAllGarbage;

    private void Start()
    {
       _totalGarbage = GameObject.FindGameObjectsWithTag(garbageTag).Length;
    }


    private void Update()
    {
        if (_removedGarbage >= _totalGarbage)
        {
            RemoveAllGarbage.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != garbageTag) return;

        _removedGarbage++; // add 1 to the totale count 
    }
}
