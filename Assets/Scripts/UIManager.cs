using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject playerCountPanel;
    public GameObject topicPanel;
    public GameObject inputPanel;
    public GameObject resultPanel;

    public Text topicText;
    public Text resultText;

    public void ShowPlayerCountPanel()
    {
        playerCountPanel.SetActive(true);
        topicPanel.SetActive(false);
        inputPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void ShowTopicPanel(string topic)
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(true);
        inputPanel.SetActive(false);
        resultPanel.SetActive(false);
        topicText.text = topic;
    }

    public void ShowInputPanel()
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(false);
        inputPanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    public void ShowResultPanel(string result)
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(false);
        inputPanel.SetActive(false);
        resultPanel.SetActive(true);
        resultText.text = result;
    }
}
