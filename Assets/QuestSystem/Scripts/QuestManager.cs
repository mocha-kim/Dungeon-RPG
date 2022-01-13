using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    public static QuestManager Instance => instance;

    public QuestDatabaseObject questDatabase;

    public event Action<QuestObject> OnCompletedQuest;

    private void Awake()
    {
        instance = this;
    }

    public void ProcessQuest(QuestType type, int targetId)
    {
        foreach (QuestObject quest in questDatabase.questObjects)
        {
            if (quest.status == QuestStatus.Accepted && quest.data.targetID == targetId)
            {
                quest.data.completedCount++;
                if (quest.data.completedCount >= quest.data.count)
                {
                    quest.status = QuestStatus.Completed;
                    OnCompletedQuest?.Invoke(quest);
                }
            }
        }
    }
}
