using UnityEngine;
using System.Collections.Generic;

// お題の管理とランダム選択を行うクラス
public class TopicManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static TopicManager Instance;
    // お題リスト
    private List<string> topics = new List<string> {
        "未読のLINEの件数",
        "今日の歩数",
        "好きな数字",
        "持っている鍵の数",
        "今の気分の点数(1-10)",
        "今日は何度寝しましたか？",
        "一日の平均食事回数",
        "今月の外食回数",
        "今年の旅行に言った回数",
        "人生で海外旅行に言った回数",
        "契約しているサブスクの数",
        "納豆の好きさの度合(0-100)",
        "人生でノックアウトした回数",
        "人生でバンジージャンプした回数",
        "人生でスカイダイビングした回数",
        "人生で引っ越しした回数",
        "人生でテロリストが攻めてきた時の妄想をした回数",
        "人生でタンスの角に小指をぶつけた回数",
        "今年、歯医者に行った回数",
        "人生で一番高い買い物の値段",
        "いつも行く食堂で頼む料理の値段の平均",
        "自分にとって縁起の良い数字",
        "最長で続いた趣味の、継続年数",
        "人生で一番幸せだったときの西暦",
        "祖国に対する好感度(0-100)",
        "尊敬する人のSNSのフォロワー数",
        "自分の名前の画数",
        "個人的に好きな時代の西暦"
    };
    // 選択されたお題
    private string selectedTopic = "";

    // インスタンスをセット（シングルトン化）
    void Awake()
    {
        Instance = this;
    }

    // ランダムでお題を取得
    public string GetRandomTopic()
    {
        int idx = Random.Range(0, topics.Count);
        selectedTopic = topics[idx];
        return selectedTopic;
    }

    // 選択されたお題を取得
    public string GetSelectedTopic()
    {
        return selectedTopic;
    }

    // お題を手動でセット
    public void SetSelectedTopic(string topic)
    {
        selectedTopic = topic;
    }

    // お題リストを取得
    public List<string> GetTopics()
    {
        return topics;
    }
}
