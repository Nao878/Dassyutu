using UnityEngine;
using System.Collections.Generic;

// お題の管理とランダム選択を行うクラス
public class TopicManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static TopicManager Instance;
    
    // お題リスト（様々なジャンルを含む）
    private List<string> topics = new List<string> {
        // 日常生活系
        "未読のLINEの件数",
        "今日の歩数",
        "好きな数字",
        "持っている鍵の数",
        "今の気分の点数(1-10)",
        "今日は何度寝しましたか？",
        "一日の平均食事回数",
        "今月の外食回数",
        "昨日の睡眠時間（時間）",
        "持っている靴の数",
        "スマホの通知数",
        "スマホの充電残量(%)",
        "今財布に入っている小銭の枚数",
        
        // 趣味・嗜好系
        "契約しているサブスクの数",
        "納豆の好きさの度合(0-100)",
        "最長で続いた趣味の継続年数",
        "自分にとって縁起の良い数字",
        "個人的に好きな時代の西暦",
        "好きなアーティストの曲数",
        "今年読んだ本の数",
        "今年見た映画の数",
        
        // 人生経験系
        "今年の旅行に行った回数",
        "人生で海外旅行に行った回数",
        "人生でノックアウトした回数",
        "人生でバンジージャンプした回数",
        "人生でスカイダイビングした回数",
        "人生で引っ越しした回数",
        "人生でタンスの角に小指をぶつけた回数",
        "今年、歯医者に行った回数",
        "人生で転職した回数",
        
        // お金系
        "人生で一番高い買い物の値段(万円)",
        "いつも行く食堂で頼む料理の値段の平均",
        "今月使ったコンビニの回数",
        
        // ネット・SNS系
        "尊敬する人のSNSのフォロワー数",
        "自分の名前の画数",
        "登録しているYouTubeチャンネルの数",
        "フォローしているアカウントの数",
        
        // その他ユニーク系
        "人生でテロリストが攻めてきた時の妄想をした回数",
        "人生で一番幸せだったときの西暦",
        "祖国に対する好感度(0-100)",
        "自分の体重(kg)",
        "自分の身長(cm)"
    };
    
    // 選択されたお題
    private string selectedTopic = "";

    // インスタンスをセット（シングルトン化）
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
