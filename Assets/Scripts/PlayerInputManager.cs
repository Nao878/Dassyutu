using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    public InputField valueInput;
    public Text playerNumberText;
    private int currentPlayer = 1;

    public GameManager gameManager;
    public UIManager uiManager;

    public void StartInput()
    {
        currentPlayer = 1;
        ShowCurrentPlayer();
    }

    public void OnSubmitValue()
    {
        int value;
        if (int.TryParse(valueInput.text, out value))
        {
            gameManager.AddPlayerValue(value);
            currentPlayer++;
            valueInput.text = "";
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
                ShowCurrentPlayer();
            }
        }
    }

    private void ShowCurrentPlayer()
    {
        playerNumberText.text = $"プレイヤー{currentPlayer}の入力";
    }
}
