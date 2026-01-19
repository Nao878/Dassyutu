using UnityEngine;
using TMPro;

// プレイヤー人数選択を管理するクラス
public class PlayerCountSelector : MonoBehaviour
{
    // プレイヤー人数入力フィールド（TextMeshPro版）
    public TMP_InputField playerCountInput;
    // プレイヤー人数表示用テキスト（TextMeshPro版）
    public TMP_Text playerCountDisplayText;
    
    // ゲーム進行管理用
    public GameManager gameManager;
    // UI画面管理用
    public UIManager uiManager;

    void Start()
    {
        // 初期表示
        UpdatePlayerCountDisplay();
    }

    // プレイヤー人数を増やす
    public void OnIncreasePlayerCount()
    {
        int currentCount = gameManager.playerCount;
        if (currentCount < 5)
        {
            gameManager.SetPlayerCount(currentCount + 1);
            UpdatePlayerCountDisplay();
        }
    }

    // プレイヤー人数を減らす
    public void OnDecreasePlayerCount()
    {
        int currentCount = gameManager.playerCount;
        if (currentCount > 2)
        {
            gameManager.SetPlayerCount(currentCount - 1);
            UpdatePlayerCountDisplay();
        }
    }

    // 入力フィールドからプレイヤー人数を設定
    public void OnPlayerCountInputChanged()
    {
        int count;
        if (int.TryParse(playerCountInput.text, out count))
        {
            gameManager.SetPlayerCount(count);
            UpdatePlayerCountDisplay();
        }
    }

    // プレイヤー人数決定ボタン
    public void OnConfirmPlayerCount()
    {
        // プレイヤー名を初期化（プレイヤー1、プレイヤー2...）
        gameManager.InitializePlayerNames();
        
        // お題を決定
        gameManager.DecideTopic();
        
        // お題表示画面へ
        uiManager.ShowTopicPanel(gameManager.topic);
    }

    // プレイヤー人数の表示を更新
    private void UpdatePlayerCountDisplay()
    {
        if (playerCountDisplayText != null)
        {
            playerCountDisplayText.text = $"プレイヤー人数: {gameManager.playerCount}";
        }
        if (playerCountInput != null)
        {
            playerCountInput.text = gameManager.playerCount.ToString();
        }
    }
}
