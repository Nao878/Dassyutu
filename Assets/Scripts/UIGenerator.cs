using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ゲーム開始時にUI要素を動的に生成するクラス
public class UIGenerator : MonoBehaviour
{
    // シングルトンインスタンス
    public static UIGenerator Instance;

    // 生成されたUI要素への参照を保持
    private Canvas mainCanvas;
    private GameObject playerCountPanel;
    private GameObject coverPanel;
    private GameObject topicPanel;
    private GameObject hintInputPanel;
    private GameObject answerPanel;
    private GameObject resultPanel;

    // カラーパレット（見やすいデザイン）
    private Color primaryColor = new Color(0.2f, 0.4f, 0.8f, 1f);      // 青
    private Color secondaryColor = new Color(0.1f, 0.6f, 0.4f, 1f);    // 緑
    private Color warningColor = new Color(0.9f, 0.5f, 0.1f, 1f);      // オレンジ
    private Color dangerColor = new Color(0.8f, 0.2f, 0.2f, 1f);       // 赤
    private Color successColor = new Color(0.2f, 0.7f, 0.3f, 1f);      // 明るい緑
    private Color panelColor = new Color(0.15f, 0.15f, 0.2f, 0.95f);   // 暗い背景
    private Color textColor = Color.white;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GenerateAllUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // すべてのUI要素を生成
    private void GenerateAllUI()
    {
        // メインキャンバスを作成
        CreateMainCanvas();

        // 各パネルを生成
        CreatePlayerCountPanel();
        CreateCoverPanel();
        CreateTopicPanel();
        CreateHintInputPanel();
        CreateAnswerPanel();
        CreateResultPanel();

        // UIManagerに参照を設定
        SetupUIManagerReferences();

        // 最初はプレイヤー人数選択画面のみ表示
        ShowOnlyPanel(playerCountPanel);
    }

    // メインキャンバスを作成
    private void CreateMainCanvas()
    {
        GameObject canvasObj = new GameObject("MainCanvas");
        mainCanvas = canvasObj.AddComponent<Canvas>();
        mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        mainCanvas.sortingOrder = 0;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();
    }

    // プレイヤー人数選択パネル
    private void CreatePlayerCountPanel()
    {
        playerCountPanel = CreatePanel("PlayerCountPanel");

        // タイトル
        CreateText(playerCountPanel.transform, "TitleText", "数値お題当てゲーム", 60, FontStyles.Bold, new Vector2(0, 350));

        // サブタイトル
        CreateText(playerCountPanel.transform, "SubtitleText", "みんなで遊ぶパーティーゲーム", 30, FontStyles.Normal, new Vector2(0, 280));

        // 人数表示
        TMP_Text countText = CreateText(playerCountPanel.transform, "PlayerCountText", "プレイヤー人数: 4", 40, FontStyles.Bold, new Vector2(0, 100));

        // 減らすボタン
        Button decreaseBtn = CreateButton(playerCountPanel.transform, "DecreaseButton", "－", new Vector2(-150, 0), new Vector2(100, 100), primaryColor);
        
        // 増やすボタン
        Button increaseBtn = CreateButton(playerCountPanel.transform, "IncreaseButton", "＋", new Vector2(150, 0), new Vector2(100, 100), primaryColor);

        // 開始ボタン
        Button startBtn = CreateButton(playerCountPanel.transform, "StartButton", "ゲーム開始", new Vector2(0, -150), new Vector2(400, 100), successColor);

        // ボタンにイベントを設定
        decreaseBtn.onClick.AddListener(() => {
            int count = GameManager.Instance.playerCount;
            if (count > 2)
            {
                GameManager.Instance.SetPlayerCount(count - 1);
                countText.text = $"プレイヤー人数: {GameManager.Instance.playerCount}";
            }
        });

        increaseBtn.onClick.AddListener(() => {
            int count = GameManager.Instance.playerCount;
            if (count < 5)
            {
                GameManager.Instance.SetPlayerCount(count + 1);
                countText.text = $"プレイヤー人数: {GameManager.Instance.playerCount}";
            }
        });

        startBtn.onClick.AddListener(() => {
            StartGame();
        });
    }

    // 伏せ画面パネル
    private void CreateCoverPanel()
    {
        coverPanel = CreatePanel("CoverPanel");

        // 警告アイコン
        CreateText(coverPanel.transform, "WarningIcon", "⚠️", 100, FontStyles.Normal, new Vector2(0, 200));

        // メッセージ
        TMP_Text messageText = CreateText(coverPanel.transform, "CoverMessage", "次のプレイヤーに\n渡してください", 45, FontStyles.Bold, new Vector2(0, 0));
        messageText.alignment = TextAlignmentOptions.Center;

        // OKボタン
        Button okBtn = CreateButton(coverPanel.transform, "CoverOKButton", "準備OK", new Vector2(0, -250), new Vector2(300, 100), primaryColor);
        okBtn.onClick.AddListener(() => OnCoverOK());

        // UIManagerに参照を渡す
        coverPanel.AddComponent<CoverPanelHelper>().messageText = messageText;
    }

    // お題表示パネル
    private void CreateTopicPanel()
    {
        topicPanel = CreatePanel("TopicPanel");

        // ラウンド表示
        CreateText(topicPanel.transform, "RoundText", "ラウンド 1", 30, FontStyles.Normal, new Vector2(0, 400));

        // 説明
        CreateText(topicPanel.transform, "InstructionText", "📋 お題を確認してください", 35, FontStyles.Bold, new Vector2(0, 250));

        // お題表示
        TMP_Text topicText = CreateText(topicPanel.transform, "TopicText", "お題がここに表示されます", 50, FontStyles.Bold, new Vector2(0, 100));
        topicText.color = new Color(1f, 0.9f, 0.3f); // 黄色系

        // 確認者リスト
        TMP_Text viewersText = CreateText(topicPanel.transform, "ViewersText", "確認者:\nプレイヤー2\nプレイヤー3\nプレイヤー4", 28, FontStyles.Normal, new Vector2(0, -100));

        // 確認完了ボタン
        Button confirmBtn = CreateButton(topicPanel.transform, "ConfirmTopicButton", "確認完了", new Vector2(0, -300), new Vector2(300, 100), successColor);
        confirmBtn.onClick.AddListener(() => OnTopicConfirmed());

        // ヘルパーコンポーネントを追加
        var helper = topicPanel.AddComponent<TopicPanelHelper>();
        helper.topicText = topicText;
        helper.viewersText = viewersText;
    }

    // ヒント入力パネル
    private void CreateHintInputPanel()
    {
        hintInputPanel = CreatePanel("HintInputPanel");

        // プレイヤー名表示
        TMP_Text playerText = CreateText(hintInputPanel.transform, "HintPlayerText", "プレイヤー2の番です", 40, FontStyles.Bold, new Vector2(0, 250));

        // 説明
        CreateText(hintInputPanel.transform, "HintInstructionText", "お題に関する数値を入力してください", 30, FontStyles.Normal, new Vector2(0, 150));

        // 入力フィールド
        TMP_InputField inputField = CreateInputField(hintInputPanel.transform, "HintInputField", "数値を入力", new Vector2(0, 0), new Vector2(400, 80));

        // 送信ボタン
        Button submitBtn = CreateButton(hintInputPanel.transform, "SubmitHintButton", "送信", new Vector2(0, -150), new Vector2(300, 100), primaryColor);
        submitBtn.onClick.AddListener(() => OnHintSubmit(inputField, playerText));

        // ヘルパーコンポーネントを追加
        var helper = hintInputPanel.AddComponent<HintInputPanelHelper>();
        helper.playerText = playerText;
        helper.inputField = inputField;
    }

    // 回答入力パネル
    private void CreateAnswerPanel()
    {
        answerPanel = CreatePanel("AnswerPanel");

        // 回答者名
        TMP_Text answererText = CreateText(answerPanel.transform, "AnswererText", "回答者さんの番です！", 40, FontStyles.Bold, new Vector2(0, 350));

        // ヒント一覧
        TMP_Text hintsText = CreateText(answerPanel.transform, "HintsDisplayText", "ヒント一覧:\nプレイヤー2: 10\nプレイヤー3: 5\nプレイヤー4: 8", 32, FontStyles.Normal, new Vector2(0, 150));
        hintsText.alignment = TextAlignmentOptions.Center;

        // 入力フィールド
        TMP_InputField inputField = CreateInputField(answerPanel.transform, "AnswerInputField", "お題を入力", new Vector2(0, -50), new Vector2(500, 80));

        // 回答ボタン
        Button answerBtn = CreateButton(answerPanel.transform, "SubmitAnswerButton", "回答する", new Vector2(0, -200), new Vector2(300, 100), successColor);
        answerBtn.onClick.AddListener(() => OnAnswerSubmit(inputField));

        // ヘルパーコンポーネントを追加
        var helper = answerPanel.AddComponent<AnswerPanelHelper>();
        helper.answererText = answererText;
        helper.hintsText = hintsText;
        helper.inputField = inputField;
    }

    // 結果表示パネル
    private void CreateResultPanel()
    {
        resultPanel = CreatePanel("ResultPanel");

        // 結果テキスト
        TMP_Text resultText = CreateText(resultPanel.transform, "ResultText", "結果がここに表示されます", 40, FontStyles.Bold, new Vector2(0, 100));
        resultText.alignment = TextAlignmentOptions.Center;

        // 次のラウンドボタン
        Button nextBtn = CreateButton(resultPanel.transform, "NextRoundButton", "次のラウンド", new Vector2(0, -150), new Vector2(350, 100), primaryColor);
        nextBtn.onClick.AddListener(() => OnNextRound());

        // 最初からボタン
        Button restartBtn = CreateButton(resultPanel.transform, "RestartButton", "最初から", new Vector2(0, -280), new Vector2(350, 100), secondaryColor);
        restartBtn.onClick.AddListener(() => OnRestart());

        // ヘルパーコンポーネントを追加
        var helper = resultPanel.AddComponent<ResultPanelHelper>();
        helper.resultText = resultText;
    }

    // === ヘルパーメソッド ===

    // パネルを作成
    private GameObject CreatePanel(string name)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(mainCanvas.transform, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = panel.AddComponent<Image>();
        bg.color = panelColor;

        return panel;
    }

    // テキストを作成
    private TMP_Text CreateText(Transform parent, string name, string text, int fontSize, FontStyles style, Vector2 position)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(900, 150);

        TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = fontSize;
        tmpText.fontStyle = style;
        tmpText.color = textColor;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.enableWordWrapping = true;

        return tmpText;
    }

    // ボタンを作成
    private Button CreateButton(Transform parent, string name, string buttonText, Vector2 position, Vector2 size, Color color)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image btnImage = btnObj.AddComponent<Image>();
        btnImage.color = color;

        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnImage;

        // ボタンテキスト
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TMP_Text tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = buttonText;
        tmpText.fontSize = 36;
        tmpText.fontStyle = FontStyles.Bold;
        tmpText.color = Color.white;
        tmpText.alignment = TextAlignmentOptions.Center;

        return btn;
    }

    // 入力フィールドを作成
    private TMP_InputField CreateInputField(Transform parent, string name, string placeholder, Vector2 position, Vector2 size)
    {
        GameObject inputObj = new GameObject(name);
        inputObj.transform.SetParent(parent, false);

        RectTransform rect = inputObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image bg = inputObj.AddComponent<Image>();
        bg.color = new Color(0.3f, 0.3f, 0.35f, 1f);

        TMP_InputField inputField = inputObj.AddComponent<TMP_InputField>();

        // テキストエリア
        GameObject textArea = new GameObject("TextArea");
        textArea.transform.SetParent(inputObj.transform, false);
        RectTransform textAreaRect = textArea.AddComponent<RectTransform>();
        textAreaRect.anchorMin = Vector2.zero;
        textAreaRect.anchorMax = Vector2.one;
        textAreaRect.offsetMin = new Vector2(10, 5);
        textAreaRect.offsetMax = new Vector2(-10, -5);
        textArea.AddComponent<RectMask2D>();

        // プレースホルダー
        GameObject placeholderObj = new GameObject("Placeholder");
        placeholderObj.transform.SetParent(textArea.transform, false);
        RectTransform placeholderRect = placeholderObj.AddComponent<RectTransform>();
        placeholderRect.anchorMin = Vector2.zero;
        placeholderRect.anchorMax = Vector2.one;
        placeholderRect.offsetMin = Vector2.zero;
        placeholderRect.offsetMax = Vector2.zero;
        TMP_Text placeholderText = placeholderObj.AddComponent<TextMeshProUGUI>();
        placeholderText.text = placeholder;
        placeholderText.fontSize = 30;
        placeholderText.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        placeholderText.alignment = TextAlignmentOptions.MidlineLeft;

        // 入力テキスト
        GameObject inputTextObj = new GameObject("Text");
        inputTextObj.transform.SetParent(textArea.transform, false);
        RectTransform inputTextRect = inputTextObj.AddComponent<RectTransform>();
        inputTextRect.anchorMin = Vector2.zero;
        inputTextRect.anchorMax = Vector2.one;
        inputTextRect.offsetMin = Vector2.zero;
        inputTextRect.offsetMax = Vector2.zero;
        TMP_Text inputText = inputTextObj.AddComponent<TextMeshProUGUI>();
        inputText.fontSize = 30;
        inputText.color = Color.white;
        inputText.alignment = TextAlignmentOptions.MidlineLeft;

        inputField.textViewport = textAreaRect;
        inputField.textComponent = inputText;
        inputField.placeholder = placeholderText;

        return inputField;
    }

    // 指定のパネルのみ表示
    private void ShowOnlyPanel(GameObject panel)
    {
        playerCountPanel.SetActive(panel == playerCountPanel);
        coverPanel.SetActive(panel == coverPanel);
        topicPanel.SetActive(panel == topicPanel);
        hintInputPanel.SetActive(panel == hintInputPanel);
        answerPanel.SetActive(panel == answerPanel);
        resultPanel.SetActive(panel == resultPanel);
    }

    // UIManagerへの参照を設定
    private void SetupUIManagerReferences()
    {
        UIManager.Instance.playerCountPanel = playerCountPanel;
        UIManager.Instance.coverPanel = coverPanel;
        UIManager.Instance.topicPanel = topicPanel;
        UIManager.Instance.hintInputPanel = hintInputPanel;
        UIManager.Instance.answerPanel = answerPanel;
        UIManager.Instance.resultPanel = resultPanel;

        // テキスト参照も設定
        UIManager.Instance.coverMessageText = coverPanel.GetComponent<CoverPanelHelper>().messageText;
        UIManager.Instance.topicText = topicPanel.GetComponent<TopicPanelHelper>().topicText;
        UIManager.Instance.topicViewersText = topicPanel.GetComponent<TopicPanelHelper>().viewersText;
        UIManager.Instance.hintInputPlayerText = hintInputPanel.GetComponent<HintInputPanelHelper>().playerText;
        UIManager.Instance.hintsDisplayText = answerPanel.GetComponent<AnswerPanelHelper>().hintsText;
        UIManager.Instance.resultText = resultPanel.GetComponent<ResultPanelHelper>().resultText;
    }

    // === ゲームロジック ===

    // ゲーム開始
    private void StartGame()
    {
        GameManager.Instance.InitializePlayerNames();
        GameManager.Instance.SetupNewRound();
        
        // 伏せ画面を表示（回答者は見ないで）
        ShowCoverScreen($"⚠️ {GameManager.Instance.GetAnswererName()}さんは\n画面を見ないでください！\n\n他のプレイヤーがお題を確認します");
    }

    // 伏せ画面を表示
    private void ShowCoverScreen(string message)
    {
        ShowOnlyPanel(coverPanel);
        coverPanel.GetComponent<CoverPanelHelper>().messageText.text = message;
    }

    // 伏せ画面OK
    private void OnCoverOK()
    {
        GameState state = GameManager.Instance.currentState;

        if (state == GameState.CoverScreen)
        {
            // お題表示へ
            ShowTopicPanel();
        }
        else if (state == GameState.HintInput)
        {
            // ヒント入力へ
            ShowHintInputPanel();
        }
        else if (state == GameState.AnswerInput)
        {
            // 回答入力へ
            ShowAnswerPanel();
        }
    }

    // お題表示
    private void ShowTopicPanel()
    {
        ShowOnlyPanel(topicPanel);
        var helper = topicPanel.GetComponent<TopicPanelHelper>();
        helper.topicText.text = $"📋 {GameManager.Instance.topic}";

        string viewers = "確認者:\n";
        for (int i = 0; i < GameManager.Instance.playerCount; i++)
        {
            if (i != GameManager.Instance.answererIndex)
            {
                viewers += $"・{GameManager.Instance.GetPlayerName(i)}\n";
            }
        }
        helper.viewersText.text = viewers;

        GameManager.Instance.currentState = GameState.TopicDisplay;
    }

    // お題確認完了
    private void OnTopicConfirmed()
    {
        // 最初のヒント提供者への交代
        string nextPlayer = GameManager.Instance.GetCurrentHintGiverName();
        ShowCoverScreen($"📱 デバイスを\n{nextPlayer}さんに\n渡してください");
        GameManager.Instance.currentState = GameState.HintInput;
    }

    // ヒント入力画面表示
    private void ShowHintInputPanel()
    {
        ShowOnlyPanel(hintInputPanel);
        var helper = hintInputPanel.GetComponent<HintInputPanelHelper>();
        helper.playerText.text = $"🎯 {GameManager.Instance.GetCurrentHintGiverName()}の番です";
        helper.inputField.text = "";
    }

    // ヒント送信
    private void OnHintSubmit(TMP_InputField inputField, TMP_Text playerText)
    {
        string value = inputField.text.Trim();
        if (string.IsNullOrEmpty(value)) return;

        GameManager.Instance.AddHintValue(value);
        inputField.text = "";

        if (GameManager.Instance.IsAllHintsGiven())
        {
            // 回答者の番
            ShowCoverScreen($"🎯 {GameManager.Instance.GetAnswererName()}さんの番です！\n\nヒントを見てお題を当ててください");
            GameManager.Instance.currentState = GameState.AnswerInput;
        }
        else
        {
            // 次のヒント提供者
            string nextPlayer = GameManager.Instance.GetCurrentHintGiverName();
            ShowCoverScreen($"📱 デバイスを\n{nextPlayer}さんに\n渡してください");
        }
    }

    // 回答画面表示
    private void ShowAnswerPanel()
    {
        ShowOnlyPanel(answerPanel);
        var helper = answerPanel.GetComponent<AnswerPanelHelper>();
        helper.answererText.text = $"🎯 {GameManager.Instance.GetAnswererName()}さん、お題を当ててください！";
        helper.hintsText.text = $"📊 ヒント一覧:\n{GameManager.Instance.GetHintsDisplayText()}";
        helper.inputField.text = "";
    }

    // 回答送信
    private void OnAnswerSubmit(TMP_InputField inputField)
    {
        string answer = inputField.text.Trim();
        if (string.IsNullOrEmpty(answer)) return;

        bool isCorrect = GameManager.Instance.CheckAnswer(answer);
        ShowResultPanel(isCorrect);
    }

    // 結果表示
    private void ShowResultPanel(bool isCorrect)
    {
        ShowOnlyPanel(resultPanel);
        var helper = resultPanel.GetComponent<ResultPanelHelper>();

        if (isCorrect)
        {
            helper.resultText.text = $"🎉 正解！ 🎉\n\nお題: {GameManager.Instance.topic}\n\n{GameManager.Instance.GetAnswererName()}さん、お見事です！";
            helper.resultText.color = successColor;
        }
        else
        {
            helper.resultText.text = $"❌ 残念！\n\n正解は「{GameManager.Instance.topic}」でした\n\n次回頑張りましょう！";
            helper.resultText.color = dangerColor;
        }

        GameManager.Instance.currentState = GameState.Result;
    }

    // 次のラウンド
    private void OnNextRound()
    {
        GameManager.Instance.NextRound();
        ShowCoverScreen($"⚠️ {GameManager.Instance.GetAnswererName()}さんは\n画面を見ないでください！\n\n他のプレイヤーがお題を確認します");
    }

    // 最初から
    private void OnRestart()
    {
        GameManager.Instance.ResetGame();
        ShowOnlyPanel(playerCountPanel);
    }
}

// === ヘルパーコンポーネント ===

public class CoverPanelHelper : MonoBehaviour
{
    public TMP_Text messageText;
}

public class TopicPanelHelper : MonoBehaviour
{
    public TMP_Text topicText;
    public TMP_Text viewersText;
}

public class HintInputPanelHelper : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_InputField inputField;
}

public class AnswerPanelHelper : MonoBehaviour
{
    public TMP_Text answererText;
    public TMP_Text hintsText;
    public TMP_InputField inputField;
}

public class ResultPanelHelper : MonoBehaviour
{
    public TMP_Text resultText;
}
