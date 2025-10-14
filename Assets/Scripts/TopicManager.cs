using UnityEngine;
using System.Collections.Generic;

public class TopicManager : MonoBehaviour
{
    public static TopicManager Instance;
    private List<string> topics = new List<string> {
        "–¢“Ç‚ÌLINE‚ÌŒ”",
        "¡“ú‚Ì•à”",
        "D‚«‚È”š",
        "‚Á‚Ä‚¢‚éŒ®‚Ì”",
        "¡‚Ì‹C•ª‚Ì“_”(1-10)"
    };
    private string selectedTopic = "";

    void Awake()
    {
        Instance = this;
    }

    public string GetRandomTopic()
    {
        int idx = Random.Range(0, topics.Count);
        selectedTopic = topics[idx];
        return selectedTopic;
    }

    public string GetSelectedTopic()
    {
        return selectedTopic;
    }

    public void SetSelectedTopic(string topic)
    {
        selectedTopic = topic;
    }

    public List<string> GetTopics()
    {
        return topics;
    }
}
