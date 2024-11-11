using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject gameOverText;  // Game Overメッセージ用のUIオブジェクト

    void Start()
    {
        gameOverText.SetActive(false);  // 最初は非表示
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);  // Game Overメッセージを表示
        // 他の必要な処理（エネルギーの停止など）
    }

    // 現在のシーンを再読み込みしてリトライ
    public void RetryStage()
    {
        // スコアリセット
        ScoreManager.instance.ResetScore();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;

        // スコアリセット
        ScoreManager.instance.ResetScore();

        // 次のステージまたは最初のシーンに移動
        if (currentSceneIndex + 1 < totalSceneCount)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
