using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    DestoryEnemy,
    AcquireItem
}

[Serializable]
public class Quest
{
    public int id;

    public QuestType type;
    public int targetID;

    public int count;
    public int completedCount;

    public int rewardExp;
    public int rewardGold;
    public int[] rewardItemIds;

    public string title;
    public string description;
}
