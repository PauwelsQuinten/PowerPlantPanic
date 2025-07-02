using UnityEditor;
using UnityEngine;

public class PowerTurbin : MonoBehaviour, IMiniGame
{
    [SerializeField]
    private GameObject turbineScreen;
    [SerializeField]
    private GameEvent turbinCleared;
    [SerializeField]
    private GarbadgeCollection garbadgeCollection;
    

    private void Start()
    {
        //Subscribe here on the garbage collector event
        garbadgeCollection.RemoveAllGarbage.AddListener(OnAllGarbadgeRemoved);
    }

    public void OnAllGarbadgeRemoved()
    {
        completed();
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

        GameObject[] pile = GameObject.FindGameObjectsWithTag("Garbage");

        foreach (var trash in pile)
        {
            trash.SetActive(true);
        }
    }
}
