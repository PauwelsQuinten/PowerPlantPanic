using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PowerTurbin : MonoBehaviour, IMiniGame
{
    [SerializeField]
    private GameObject turbineScreen;
    [SerializeField]
    private GameEvent turbinCleared;
    [SerializeField]
    public List<GameObject> garbagePositions;

    private GameObject[] _garbageList;
    private GarbageCollection _garbadgeCollection;

    private void Start()
    {
        _garbadgeCollection = turbineScreen.GetComponentInChildren<GarbageCollection>();

        //Subscribe here on the garbage collector event
        _garbadgeCollection.RemoveAllGarbage.AddListener(OnAllGarbadgeRemoved);
    }

    public void OnAllGarbadgeRemoved()
    {
        StartCoroutine(DelayedUIClose());
    }

    public void completed()
    {
        turbinCleared.Raise(this, new MiniGameFinishedEventArgs{FinishedMiniGame = MiniGame.FanBlock});
        turbineScreen.SetActive(false);
    }

    public void failed()
    {
        Debug.Log("There is no failing, only losing.");
    }

    public void StartMiniGame(Component sender, object obj)
    {
        turbineScreen.SetActive(true);

        //Make garbage visible and put it on the right location
        SetGarabageLocation();
    }

    IEnumerator DelayedUIClose()
    {
        yield return new WaitForSeconds(5f);

        completed();
    }

    private void SetGarabageLocation()
    {
        //Check if there are enough positions 
        if (garbagePositions.Count != _garbageList.Length)
        {
            Debug.Log("Not enough positions for all garbage to be positioned!");
            return;
        }

        _garbageList = GameObject.FindGameObjectsWithTag("Garbage");

        int garbageIndex = 0;
        foreach (var garbage in _garbageList)
        {
            garbage.transform.position = garbagePositions[garbageIndex].transform.position;
            garbage.SetActive(true);
        }
    }
}
