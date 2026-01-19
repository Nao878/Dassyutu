using UnityEngine;

// ゲーム全体のフロー（画面遷移）を制御するクラス
public class GameFlowController : MonoBehaviour
{
    // 各マネージャーへの参照
    public PlayerInputManager playerInputManager;
    public AnswerManager answerManager;

    // 現在の伏せ画面の次のステップを記録
    private enum NextStep
    {
        TopicDisplay,       // お題表示へ
        HintInput,          // ヒント入力へ
        AnswerInput         // 回答入力へ
    }
    private NextStep currentNextStep = NextStep.TopicDisplay;

    // ゲーム開始（プレイヤー人数決定後に呼び出す）
    public void StartGame()
    {
        // プレイヤー名を初期化
        GameManager.Instance.InitializePlayerNames();
        
        // 新しいラウンドをセットアップ
        GameManager.Instance.SetupNewRound();
        
        // 回答者に画面を見ないよう伏せ画面を表示
        currentNextStep = NextStep.TopicDisplay;
        UIManager.Instance.ShowAnswererCoverScreen();
    }

    // 伏せ画面のOKボタンが押されたときの処理
    public void OnCoverScreenOK()
    {
        switch (currentNextStep)
        {
            case NextStep.TopicDisplay:
                // お題表示画面へ
                UIManager.Instance.ShowTopicPanel();
                break;
                
            case NextStep.HintInput:
                // ヒント入力画面へ
                if (playerInputManager != null)
                {
                    playerInputManager.StartHintInput();
                }
                UIManager.Instance.ShowHintInputPanel();
                break;
                
            case NextStep.AnswerInput:
                // 回答入力画面へ
                if (answerManager != null)
                {
                    answerManager.StartAnswerInput();
                }
                UIManager.Instance.ShowAnswerPanel();
                break;
        }
    }

    // お題確認完了ボタンが押されたときの処理
    public void OnTopicConfirmed()
    {
        // 最初のヒント提供者への交代画面を表示
        string firstHintGiverName = GameManager.Instance.GetCurrentHintGiverName();
        currentNextStep = NextStep.HintInput;
        UIManager.Instance.ShowPlayerChangeCoverScreen(firstHintGiverName);
    }

    // ヒント送信後に呼び出す（PlayerInputManagerから呼び出される）
    public void OnHintSubmitted()
    {
        // 全員入力済みなら回答者の番へ
        if (GameManager.Instance.IsAllHintsGiven())
        {
            currentNextStep = NextStep.AnswerInput;
            UIManager.Instance.ShowAnswererTurnCoverScreen();
        }
        else
        {
            // 次のヒント提供者への交代画面を表示
            currentNextStep = NextStep.HintInput;
            string nextPlayerName = GameManager.Instance.GetCurrentHintGiverName();
            UIManager.Instance.ShowPlayerChangeCoverScreen(nextPlayerName);
        }
    }

    // 次のラウンドへ進む（結果画面から呼び出す）
    public void OnNextRound()
    {
        GameManager.Instance.NextRound();
        currentNextStep = NextStep.TopicDisplay;
        UIManager.Instance.ShowAnswererCoverScreen();
    }

    // ゲームをリセットして最初から始める（結果画面から呼び出す）
    public void OnRestartGame()
    {
        GameManager.Instance.ResetGame();
        UIManager.Instance.ShowPlayerCountPanel();
    }

    // タイトルに戻る
    public void OnBackToTitle()
    {
        GameManager.Instance.ResetGame();
        UIManager.Instance.ShowPlayerCountPanel();
    }
}
