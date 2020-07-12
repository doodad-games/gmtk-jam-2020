using System;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public static event Action onAStartPointFinished;

    public bool died => _died;
    public bool finished => _finished;

    GameObject _char;
    bool _spawnedChar = false;
    bool _died;
    bool _finished;

    void OnEnable()
    {
        Player.RegisterStartPoint(this);
        Player.onStartStopped += MaybeSpawnCharacter;
        MaybeSpawnCharacter();
    }

    void OnDisable()
    {
        Player.DeregisterStartPoint(this);
        Player.onStartStopped -= MaybeSpawnCharacter;
    }

    public void SetDied()
    {
        _died = true;
        SetFinished();
    }

    public void SetFinished()
    {
        _finished = true;
        onAStartPointFinished?.Invoke();
    }

    void MaybeSpawnCharacter()
    {
        if (!Player.rolling || _spawnedChar) return;
        _spawnedChar = true;

        _char = Instantiate(Resources.Load<GameObject>("Character"), transform);
    }
}