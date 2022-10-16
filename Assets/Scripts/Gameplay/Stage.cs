using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Stage/Stage", order = 1)]
public class Stage : ScriptableObject
{
    MusicFile musicFile;
    public uint numberOfInitialBeats = 0;
    [Scene] public string gameScene;
}
