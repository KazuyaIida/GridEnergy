using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    private TMP_Text highScoreText;

    void Start()
    {
        highScoreText = GetComponent<TMP_Text>();
        UpdateHighScoreText();
    }

    public void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + ScoreManager.instance.GetHighScore();
    }
}
