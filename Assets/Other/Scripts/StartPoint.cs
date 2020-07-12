using UnityEngine;

public class StartPoint : MonoBehaviour
{
    GameObject _char;
    bool _spawnedChar = false;

    void OnEnable()
    {
        Player.onStartStopped += MaybeSpawnCharacter;
        MaybeSpawnCharacter();
    }

    void OnDisable() => Player.onStartStopped -= MaybeSpawnCharacter;

    void MaybeSpawnCharacter()
    {
        if (!Player.playing || _spawnedChar) return;
        _spawnedChar = true;

        _char = Instantiate(Resources.Load<GameObject>("Character"), transform);
    }
}