using UnityEngine;
using TMPro;

// プレイヤー人数選択を管理するクラス
public class PlayerCountSelector : MonoBehaviour
{
    // プレイヤー人数入力フィールド
    public TMP_InputField playerCountInput;
    // プレイヤー人数表示用テキスト
    public TMP_Text playerCountDisplayText;
    
    // ゲームフローコントローラーへの参照
    public GameFlowController gameFlowController;

    void Start()
    {
        // 初期表示
        UpdatePlayerCountDisplay();
    }

    // プレイヤー人数を増やす
    public void OnIncreasePlayerCount()
    {
        int currentCount = GameManager.Instance.playerCount;
        if (currentCount < 5)
        {
            GameManager.Instance.SetPlayerCount(currentCount + 1);
            UpdatePlayerCountDisplay();
        }
    }

    // プレイヤー人数を減らす
    public void OnDecreasePlayerCount()
    {
        int currentCount = GameManager.Instance.playerCount;
        if (currentCount > 2)
        {
            GameManager.Instance.SetPlayerCount(currentCount - 1);
            UpdatePlayerCountDisplay();
        }
    }

    // 入力フィールドからプレイヤー人数を設定
    public void OnPlayerCountInputChanged()
    {
        int count;
        if (int.TryParse(playerCountInput.text, out count))
        {
            GameManager.Instance.SetPlayerCount(count);
            UpdatePlayerCountDisplay();
        }
    }

    // プレイヤー人数決定ボタン
    public void OnConfirmPlayerCount()
    {
        // ゲームフローを開始
        if (gameFlowController != null)
        {
            gameFlowController.StartGame();
        }
    }

    // プレイヤー人数の表示を更新
    private void UpdatePlayerCountDisplay()
    {
        if (playerCountDisplayText != null)
        {
            playerCountDisplayText.text = $"プレイヤー人数: {GameManager.Instance.playerCount}";
        }
        if (playerCountInput != null)
        {
            playerCountInput.text = GameManager.Instance.playerCount.ToString();
        }
    }
}
