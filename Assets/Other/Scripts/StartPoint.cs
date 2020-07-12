using System;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public static event Action onAStartPointFinished;

    public bool finished => _finished;

    GameObject _char;
    bool _spawnedChar = false;
    bool _finished;

    void OnEnable()
    {
        Player.onStartStopped += MaybeSpawnCharacter;
        MaybeSpawnCharacter();
    }

    void OnDisable() => Player.onStartStopped -= MaybeSpawnCharacter;

    public void SetFinished()
    {
        _finished = true;
        onAStartPointFinished?.Invoke();
    }

    void MaybeSpawnCharacter()
    {
        if (!Player.playing || _spawnedChar) return;
        _spawnedChar = true;

        _char = Instantiate(Resources.Load<GameObject>("Character"), transform);
    }
}