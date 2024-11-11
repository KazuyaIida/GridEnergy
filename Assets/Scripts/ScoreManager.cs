using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score;
    private int highScore;

    private float scoreMultiplier = 1.0f;  // �X�R�A�{���i�����l��1.0�j

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();  // �Q�[���J�n���ɍō��X�R�A�����[�h
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        // �{����K�p���ăX�R�A�����Z
        score += Mathf.RoundToInt(points * scoreMultiplier);
    }

    public void IncreaseMultiplier(float amount)
    {
        scoreMultiplier += amount;  // �{���𑝉�
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = 1.0f;  // �{�������Z�b�g
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // �f�t�H���g��0
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
    }
}
