using UnityEngine;
using TMPro;

// 回答者の回答入力と正誤判定を管理するクラス
public class AnswerManager : MonoBehaviour
{
    // 回答入力フィールド
    public TMP_InputField answerInputField;
    // ヒント一覧表示用テキスト
    public TMP_Text hintsDisplayText;
    // 回答者名表示用テキスト
    public TMP_Text answererNameText;

    // 回答入力開始時の初期化
    public void StartAnswerInput()
    {
        // 入力フィールドをクリア
        if (answerInputField != null)
        {
            answerInputField.text = "";
        }
        
        // ヒント一覧を表示
        if (hintsDisplayText != null)
        {
            hintsDisplayText.text = $"ヒント一覧:\n{GameManager.Instance.GetHintsDisplayText()}";
        }

        // 回答者名を表示
        if (answererNameText != null)
        {
            answererNameText.text = $"{GameManager.Instance.GetAnswererName()}さん、お題を当ててください！";
        }
    }

    // 回答送信処理（送信ボタンから呼び出す）
    public void OnSubmitAnswer()
    {
        if (answerInputField == null) return;
        
        string answer = answerInputField.text.Trim();
        
        // 入力値が空でないか確認
        if (string.IsNullOrEmpty(answer))
        {
            Debug.LogWarning("回答が空です");
            return;
        }

        // 正誤判定
        bool isCorrect = GameManager.Instance.CheckAnswer(answer);
        
        // 結果画面を表示
        UIManager.Instance.ShowResultPanel(isCorrect, GameManager.Instance.topic);
    }

    // 伏せ画面から回答入力画面への遷移（OKボタンから呼び出す）
    public void OnCoverScreenOKForAnswerInput()
    {
        StartAnswerInput();
        UIManager.Instance.ShowAnswerPanel();
    }
}
