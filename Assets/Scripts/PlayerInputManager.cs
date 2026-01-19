using UnityEngine;
using TMPro;

// ヒント提供者の入力を管理するクラス
public class PlayerInputManager : MonoBehaviour
{
    // ヒント入力フィールド
    public TMP_InputField hintInputField;
    // 現在のプレイヤー番号表示用テキスト
    public TMP_Text playerNumberText;

    // 入力開始時の初期化
    public void StartHintInput()
    {
        // 入力フィールドをクリア
        if (hintInputField != null)
        {
            hintInputField.text = "";
        }
        
        // 現在のヒント提供者の名前を表示
        ShowCurrentHintGiver();
    }

    // ヒント値の送信処理（送信ボタンから呼び出す）
    public void OnSubmitHint()
    {
        if (hintInputField == null) return;
        
        string inputValue = hintInputField.text.Trim();
        
        // 入力値が空でないか確認
        if (string.IsNullOrEmpty(inputValue))
        {
            Debug.LogWarning("入力値が空です");
            return;
        }

        // ヒント値を追加
        GameManager.Instance.AddHintValue(inputValue);
        
        // 入力欄をクリア
        hintInputField.text = "";

        // 全員入力済みなら回答者の番へ
        if (GameManager.Instance.IsAllHintsGiven())
        {
            // 回答者への交代画面を表示
            UIManager.Instance.ShowAnswererTurnCoverScreen();
        }
        else
        {
            // 次のヒント提供者への交代画面を表示
            string nextPlayerName = GameManager.Instance.GetCurrentHintGiverName();
            UIManager.Instance.ShowPlayerChangeCoverScreen(nextPlayerName);
        }
    }

    // 現在のヒント提供者を表示
    private void ShowCurrentHintGiver()
    {
        if (playerNumberText != null)
        {
            string playerName = GameManager.Instance.GetCurrentHintGiverName();
            playerNumberText.text = $"{playerName}の入力";
        }
    }

    // 伏せ画面からヒント入力画面への遷移（OKボタンから呼び出す）
    public void OnCoverScreenOKForHintInput()
    {
        StartHintInput();
        UIManager.Instance.ShowHintInputPanel();
    }
}
