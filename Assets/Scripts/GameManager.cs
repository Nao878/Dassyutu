using UnityEngine;
using System.Collections.Generic;

// ゲーム全体の進行を管理するクラス
public class GameManager : MonoBehaviour
{
    // プレイヤー人数
    public int playerCount = 2;
    // 各プレイヤーの入力値を格納するリスト
    public List<int> playerValues = new List<int>();
    // 現在のお題
    public string topic;
    // お題をランダムで決めるかどうか
    public bool isTopicRandom = true;

    // プレイヤー人数を設定する
    public void SetPlayerCount(int count)
    {
        playerCount = Mathf.Clamp(count, 2, 5);
        playerValues.Clear(); // 入力値リストを初期化
    }

    // お題を決定する
    public void DecideTopic()
    {
        if (isTopicRandom)
        {
            topic = TopicManager.Instance.GetRandomTopic(); // ランダムでお題を取得
        }
        else
        {
            topic = TopicManager.Instance.GetSelectedTopic(); // 手動で選択されたお題を取得
        }
    }

    // プレイヤーの入力値を追加する
    public void AddPlayerValue(int value)
    {
        playerValues.Add(value);
    }

    // 全プレイヤーの入力が完了したか判定する
    public bool IsAllPlayerInput()
    {
        return playerValues.Count >= playerCount;
    }
}
