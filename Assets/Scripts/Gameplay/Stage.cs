using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Stage/Stage", order = 1)]
public class Stage : ScriptableObject
{
    public MusicFile musicFile;
    public float timeBeforeGameActuallyStarts = 0.0f;
    [Scene] public List<string> gameScene;
}
