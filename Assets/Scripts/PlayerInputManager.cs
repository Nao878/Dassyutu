using UnityEngine;
using TMPro;

// プレイヤーの値入力を管理するクラス
public class PlayerInputManager : MonoBehaviour
{
    // 入力フィールド（TextMeshPro版）
    public TMP_InputField valueInput;
    // 現在のプレイヤー番号表示用テキスト（TextMeshPro版）
    public TMP_Text playerNumberText;
    // 現在入力中のプレイヤー番号
    private int currentPlayer = 1;

    // ゲーム進行管理用
    public GameManager gameManager;
    // UI画面管理用
    public UIManager uiManager;

    // 入力開始時の初期化
    public void StartInput()
    {
        currentPlayer = 1;
        ShowCurrentPlayer();
    }

    // 入力値の送信処理
    public void OnSubmitValue()
    {
        int value;
        // 入力値が整数か判定
        if (int.TryParse(valueInput.text, out value))
        {
            gameManager.AddPlayerValue(value); // 入力値を追加
            currentPlayer++;
            valueInput.text = ""; // 入力欄をクリア
            // 全員入力済みなら結果画面へ
            if (gameManager.IsAllPlayerInput())
            {
                string result = "全員の入力値: ";
                foreach (var v in gameManager.playerValues)
                {
                    result += v.ToString() + " ";
                }
                uiManager.ShowResultPanel(result + "\nお題を推理してください！");
            }
            else
            {
                ShowCurrentPlayer(); // 次のプレイヤーへ
            }
        }
    }

    // 現在のプレイヤー番号を表示
    private void ShowCurrentPlayer()
    {
        playerNumberText.text = $"プレイヤー{currentPlayer}の入力";
    }
}
