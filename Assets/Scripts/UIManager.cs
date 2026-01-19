using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UI画面の表示・切り替えを管理するクラス
public class UIManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static UIManager Instance;

    // === パネル ===
    // プレイヤー人数選択画面
    public GameObject playerCountPanel;
    // 伏せ画面（プレイヤー交代確認用）
    public GameObject coverPanel;
    // お題表示画面
    public GameObject topicPanel;
    // ヒント入力画面
    public GameObject hintInputPanel;
    // 回答入力画面
    public GameObject answerPanel;
    // 結果表示画面
    public GameObject resultPanel;

    // === テキスト要素 ===
    // 伏せ画面のメッセージテキスト
    public TMP_Text coverMessageText;
    // お題表示用テキスト
    public TMP_Text topicText;
    // お題確認者表示用テキスト
    public TMP_Text topicViewersText;
    // ヒント入力者表示用テキスト
    public TMP_Text hintInputPlayerText;
    // ヒント一覧表示用テキスト（回答画面用）
    public TMP_Text hintsDisplayText;
    // 結果表示用テキスト
    public TMP_Text resultText;
    // ラウンド表示用テキスト
    public TMP_Text roundText;

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

    void Start()
    {
        // 最初はプレイヤー人数選択画面を表示
        ShowPlayerCountPanel();
    }

    // すべてのパネルを非表示にするヘルパーメソッド
    private void HideAllPanels()
    {
        if (playerCountPanel != null) playerCountPanel.SetActive(false);
        if (coverPanel != null) coverPanel.SetActive(false);
        if (topicPanel != null) topicPanel.SetActive(false);
        if (hintInputPanel != null) hintInputPanel.SetActive(false);
        if (answerPanel != null) answerPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
    }

    // プレイヤー人数選択画面を表示
    public void ShowPlayerCountPanel()
    {
        HideAllPanels();
        if (playerCountPanel != null) playerCountPanel.SetActive(true);
        GameManager.Instance.currentState = GameState.PlayerCountSelect;
    }

    // 伏せ画面を表示（プレイヤー交代時）
    public void ShowCoverScreen(string message)
    {
        HideAllPanels();
        if (coverPanel != null) coverPanel.SetActive(true);
        if (coverMessageText != null) coverMessageText.text = message;
        GameManager.Instance.currentState = GameState.CoverScreen;
    }

    // お題表示画面を表示（ヒント提供者向け）
    public void ShowTopicPanel()
    {
        HideAllPanels();
        if (topicPanel != null) topicPanel.SetActive(true);
        
        // お題を表示
        if (topicText != null)
        {
            topicText.text = $"お題: {GameManager.Instance.topic}";
        }

        // お題を確認するヒント提供者の名前を表示
        if (topicViewersText != null)
        {
            string viewers = "";
            for (int i = 0; i < GameManager.Instance.playerCount; i++)
            {
                if (i == GameManager.Instance.answererIndex) continue;
                viewers += GameManager.Instance.GetPlayerName(i) + "\n";
            }
            topicViewersText.text = $"確認者:\n{viewers}";
        }

        // ラウンド表示
        UpdateRoundDisplay();
        
        GameManager.Instance.currentState = GameState.TopicDisplay;
    }

    // ヒント入力画面を表示
    public void ShowHintInputPanel()
    {
        HideAllPanels();
        if (hintInputPanel != null) hintInputPanel.SetActive(true);
        
        // 現在のヒント提供者の名前を表示
        if (hintInputPlayerText != null)
        {
            string playerName = GameManager.Instance.GetCurrentHintGiverName();
            hintInputPlayerText.text = $"{playerName}の番です\nお題に関する数値を入力してください";
        }

        // ラウンド表示
        UpdateRoundDisplay();
        
        GameManager.Instance.currentState = GameState.HintInput;
    }

    // 回答入力画面を表示（回答者向け）
    public void ShowAnswerPanel()
    {
        HideAllPanels();
        if (answerPanel != null) answerPanel.SetActive(true);
        
        // ヒント一覧を表示
        if (hintsDisplayText != null)
        {
            hintsDisplayText.text = $"ヒント一覧:\n{GameManager.Instance.GetHintsDisplayText()}";
        }

        // ラウンド表示
        UpdateRoundDisplay();
        
        GameManager.Instance.currentState = GameState.AnswerInput;
    }

    // 結果表示画面を表示
    public void ShowResultPanel(bool isCorrect, string correctAnswer)
    {
        HideAllPanels();
        if (resultPanel != null) resultPanel.SetActive(true);
        
        if (resultText != null)
        {
            if (isCorrect)
            {
                resultText.text = $"🎉 正解！🎉\n\nお題: {correctAnswer}\n\n{GameManager.Instance.GetAnswererName()}さん、お見事です！";
            }
            else
            {
                resultText.text = $"❌ 残念！\n\n正解は「{correctAnswer}」でした\n\n次回頑張りましょう！";
            }
        }

        // ラウンド表示
        UpdateRoundDisplay();
        
        GameManager.Instance.currentState = GameState.Result;
    }

    // ラウンド表示を更新
    private void UpdateRoundDisplay()
    {
        if (roundText != null)
        {
            roundText.text = $"ラウンド {GameManager.Instance.currentRound}";
        }
    }

    // 回答者に画面を見せないための伏せ画面を表示
    public void ShowAnswererCoverScreen()
    {
        string message = $"⚠️ {GameManager.Instance.GetAnswererName()}さんは\n画面を見ないでください！\n\n他のプレイヤーがお題を確認します\n\n準備ができたら「OK」を押してください";
        ShowCoverScreen(message);
    }

    // プレイヤー交代用の伏せ画面を表示
    public void ShowPlayerChangeCoverScreen(string nextPlayerName)
    {
        string message = $"📱 デバイスを\n{nextPlayerName}さんに\n渡してください\n\n準備ができたら「OK」を押してください";
        ShowCoverScreen(message);
    }

    // 回答者の番を知らせる伏せ画面を表示
    public void ShowAnswererTurnCoverScreen()
    {
        string message = $"🎯 {GameManager.Instance.GetAnswererName()}さんの番です！\n\nヒントを見てお題を当ててください\n\n準備ができたら「OK」を押してください";
        ShowCoverScreen(message);
    }
}
