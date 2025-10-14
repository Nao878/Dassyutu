using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int playerCount = 2;
    public List<int> playerValues = new List<int>();
    public string topic;
    public bool isTopicRandom = true;

    public void SetPlayerCount(int count)
    {
        playerCount = Mathf.Clamp(count, 2, 5);
        playerValues.Clear();
    }

    public void DecideTopic()
    {
        if (isTopicRandom)
        {
            topic = TopicManager.Instance.GetRandomTopic();
        }
        else
        {
            topic = TopicManager.Instance.GetSelectedTopic();
        }
    }

    public void AddPlayerValue(int value)
    {
        playerValues.Add(value);
    }

    public bool IsAllPlayerInput()
    {
        return playerValues.Count >= playerCount;
    }
}
