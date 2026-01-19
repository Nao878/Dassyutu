using UnityEngine;

// ゲーム初期化を制御するクラス
// このスクリプトはシーン内の最初に実行される
[DefaultExecutionOrder(-100)]
public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        // GameManagerを生成
        if (GameManager.Instance == null)
        {
            GameObject gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
        }

        // TopicManagerを生成
        if (TopicManager.Instance == null)
        {
            GameObject topicManagerObj = new GameObject("TopicManager");
            topicManagerObj.AddComponent<TopicManager>();
        }

        // UIManagerを生成
        if (UIManager.Instance == null)
        {
            GameObject uiManagerObj = new GameObject("UIManager");
            uiManagerObj.AddComponent<UIManager>();
        }

        // UIGeneratorを生成（UI要素を動的に作成）
        if (UIGenerator.Instance == null)
        {
            GameObject uiGeneratorObj = new GameObject("UIGenerator");
            uiGeneratorObj.AddComponent<UIGenerator>();
        }
    }
}
