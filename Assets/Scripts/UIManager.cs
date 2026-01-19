using UnityEngine;
using TMPro;

// UI画面の表示・切り替えを管理するクラス
public class UIManager : MonoBehaviour
{
    // プレイヤー人数選択画面
    public GameObject playerCountPanel;
    // お題表示画面
    public GameObject topicPanel;
    // プレイヤー入力画面
    public GameObject inputPanel;
    // 結果表示画面
    public GameObject resultPanel;

    // お題表示用テキスト（TextMeshPro版）
    public TMP_Text topicText;
    // 結果表示用テキスト（TextMeshPro版）
    public TMP_Text resultText;

    void Start()
    {
        // 最初はプレイヤー人数選択画面を表示
        ShowPlayerCountPanel();
    }

    // プレイヤー人数選択画面を表示
    public void ShowPlayerCountPanel()
    {
        playerCountPanel.SetActive(true);
        topicPanel.SetActive(false);
        inputPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    // お題表示画面を表示
    public void ShowTopicPanel(string topic)
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(true);
        inputPanel.SetActive(false);
        resultPanel.SetActive(false);
        topicText.text = topic;
    }

    // プレイヤー入力画面を表示
    public void ShowInputPanel()
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(false);
        inputPanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    // 結果表示画面を表示
    public void ShowResultPanel(string result)
    {
        playerCountPanel.SetActive(false);
        topicPanel.SetActive(false);
        inputPanel.SetActive(false);
        resultPanel.SetActive(true);
        resultText.text = result;
    }
}
