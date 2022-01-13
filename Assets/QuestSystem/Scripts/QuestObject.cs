using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    None,
    Accepted,
    Completed,
    Rewarded
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quests/New Quest")]
public class QuestObject : ScriptableObject
{
    public Quest data = new();
    public QuestStatus status;
}
