using UnityEngine;
using System.Collections.Generic;

// ゲームの状態を表すenum
public enum GameState
{
    PlayerCountSelect,  // プレイヤー人数選択
    CoverScreen,        // 伏せ画面（プレイヤー交代確認）
    TopicDisplay,       // お題表示（ヒント提供者向け）
    HintInput,          // ヒント入力
    AnswerInput,        // 回答者の回答入力
    Result              // 結果表示
}

// ゲーム全体の進行を管理するクラス
public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance;

    // プレイヤー人数
    public int playerCount = 4;
    // 各プレイヤーの名前を格納するリスト
    public List<string> playerNames = new List<string>();
    // 現在のお題
    public string topic;
    // お題をランダムで決めるかどうか
    public bool isTopicRandom = true;

    // === 新規追加フィールド ===
    // 現在のゲーム状態
    public GameState currentState = GameState.PlayerCountSelect;
    // 回答者のプレイヤーインデックス（0始まり）
    public int answererIndex = 0;
    // 現在入力中のヒント提供者インデックス
    private int currentHintGiverIndex = 0;
    // ヒント提供者の入力値リスト
    public List<string> hintValues = new List<string>();
    // 現在のラウンド数
    public int currentRound = 1;

    // シングルトン初期化
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

    // プレイヤー人数を設定する
    public void SetPlayerCount(int count)
    {
        playerCount = Mathf.Clamp(count, 2, 5);
    }

    // プレイヤー名を初期化する（プレイヤー1、プレイヤー2...）
    public void InitializePlayerNames()
    {
        playerNames.Clear();
        for (int i = 1; i <= playerCount; i++)
        {
            playerNames.Add($"プレイヤー{i}");
        }
    }

    // お題を決定する
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

    // === 新規追加メソッド ===

    // 新しいラウンドをセットアップする
    public void SetupNewRound()
    {
        hintValues.Clear();
        currentHintGiverIndex = 0;
        DecideTopic();
        currentState = GameState.CoverScreen;
    }

    // 回答者を設定する
    public void SetAnswerer(int index)
    {
        answererIndex = Mathf.Clamp(index, 0, playerCount - 1);
    }

    // 回答者の名前を取得する
    public string GetAnswererName()
    {
        return GetPlayerName(answererIndex);
    }

    // 現在のヒント提供者のインデックスを取得（回答者をスキップ）
    public int GetCurrentHintGiverIndex()
    {
        int hintGiverCount = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (i == answererIndex) continue; // 回答者はスキップ
            if (hintGiverCount == currentHintGiverIndex)
            {
                return i;
            }
            hintGiverCount++;
        }
        return -1; // 全員入力済み
    }

    // 現在のヒント提供者の名前を取得
    public string GetCurrentHintGiverName()
    {
        int index = GetCurrentHintGiverIndex();
        if (index >= 0)
        {
            return GetPlayerName(index);
        }
        return "";
    }

    // ヒント値を追加する
    public void AddHintValue(string value)
    {
        hintValues.Add(value);
        currentHintGiverIndex++;
    }

    // 全ヒント入力が完了したか判定
    public bool IsAllHintsGiven()
    {
        // ヒント提供者は playerCount - 1 人（回答者を除く）
        return hintValues.Count >= playerCount - 1;
    }

    // 回答の正誤判定（部分一致も許容）
    public bool CheckAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer)) return false;
        
        // 完全一致チェック
        if (topic.Equals(answer, System.StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        
        // 部分一致チェック（お題に回答が含まれている、または回答にお題が含まれている）
        string normalizedTopic = topic.Replace(" ", "").Replace("　", "");
        string normalizedAnswer = answer.Replace(" ", "").Replace("　", "");
        
        return normalizedTopic.Contains(normalizedAnswer) || normalizedAnswer.Contains(normalizedTopic);
    }

    // 次のラウンドへ進む（回答者をローテーション）
    public void NextRound()
    {
        currentRound++;
        // 回答者を次のプレイヤーに変更
        answererIndex = (answererIndex + 1) % playerCount;
        SetupNewRound();
    }

    // ゲームをリセットして最初から始める
    public void ResetGame()
    {
        currentRound = 1;
        answererIndex = 0;
        hintValues.Clear();
        currentHintGiverIndex = 0;
        currentState = GameState.PlayerCountSelect;
    }

    // プレイヤー名を取得する
    public string GetPlayerName(int index)
    {
        if (index >= 0 && index < playerNames.Count)
        {
            return playerNames[index];
        }
        return $"プレイヤー{index + 1}";
    }

    // ヒント一覧を取得する（表示用）
    public string GetHintsDisplayText()
    {
        string result = "";
        int hintIndex = 0;
        for (int i = 0; i < playerCount; i++)
        {
            if (i == answererIndex) continue; // 回答者はスキップ
            if (hintIndex < hintValues.Count)
            {
                result += $"{GetPlayerName(i)}: {hintValues[hintIndex]}\n";
                hintIndex++;
            }
        }
        return result;
    }
}
