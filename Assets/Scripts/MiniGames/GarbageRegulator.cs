using UnityEngine;

public class GarbageRegulator : MonoBehaviour, IMiniGame
{
    [SerializeField]
    private GameObject _itemHolder;
    [SerializeField]
    private GameObject _barrelPrefab;
    [SerializeField]
    private GameObject _barrelSpawnLocation;
    [SerializeField]
    private GameEvent _minigameFinished;

    [Header("Audio Variables")]
    [SerializeField]
    private AudioClip _grabSound;
    [SerializeField]
    private AudioClip _placeSound;
    [SerializeField]
    private SoundManager _soundManager;

    private GameObject _heldItem;
    private GameObject _spawnedBarrel;

    private void Start()
    {
        _soundManager.LoadSoundWithOutPath("grab", _grabSound);
        _soundManager.LoadSoundWithOutPath("place", _placeSound);
    }

    public void completed()
    {
        _minigameFinished.Raise(this, new MiniGameFinishedEventArgs { FinishedMiniGame = MiniGame.WasteManagement});
    }

    public void failed()
    {

    }

    public void StartMiniGame(Component sender, object obj)
    {
        SpawnBarrel();
    }
    private void SpawnBarrel()
    {
        if (_spawnedBarrel != null) return;
        GameObject go = Instantiate(_barrelPrefab, _barrelSpawnLocation.transform.position, _barrelPrefab.transform.rotation);
        _spawnedBarrel = go;
    }

    public void OpenWasteControl(Component sender, object obj)
    {
        StartMiniGame(sender, obj);
    }

    public void PickUpBarrel(Component sender, object obj)
    {
        if (_heldItem != null) return;

        _soundManager.PlaySound("grab");

        _heldItem = _spawnedBarrel;
        _heldItem.transform.parent = _itemHolder.transform;
        _heldItem.transform.localPosition = Vector3.zero;
    }

    public void PlaceBarrel(Component sender, object obj)
    {
        if (sender.transform.parent.gameObject.transform.parent.gameObject != gameObject) return;
        if (_heldItem == null) return;

        _soundManager.PlaySound("place");

        Destroy(_heldItem);
        _spawnedBarrel = null;
        completed();
    }
}
