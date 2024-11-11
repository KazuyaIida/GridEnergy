using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject gameOverText;  // Game Over���b�Z�[�W�p��UI�I�u�W�F�N�g

    void Start()
    {
        gameOverText.SetActive(false);  // �ŏ��͔�\��
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);  // Game Over���b�Z�[�W��\��
        // ���̕K�v�ȏ����i�G�l���M�[�̒�~�Ȃǁj
    }

    // ���݂̃V�[�����ēǂݍ��݂��ă��g���C
    public void RetryStage()
    {
        // �X�R�A���Z�b�g
        ScoreManager.instance.ResetScore();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;

        // �X�R�A���Z�b�g
        ScoreManager.instance.ResetScore();

        // ���̃X�e�[�W�܂��͍ŏ��̃V�[���Ɉړ�
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
