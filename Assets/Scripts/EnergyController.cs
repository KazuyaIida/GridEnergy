using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyController : MonoBehaviour
{
    public float speed = 5f; // エネルギーの移動速度
    private Vector3 direction; // エネルギーの移動方向

    private bool isMovingToTerminal = false;
    private Transform terminalTransform;

    private GameObject clearText;  // クリアメッセージ用のUIオブジェクト
    private TMP_Text text;

    private GameObject nextButton;
    private Button button;

    public Vector2 gridSize;  // GridGeneratorから渡されるグリッドサイズ

    private GameController gameController;

    void Start()
    {
        // HierarchyからClearTextオブジェクトを探して取得
        clearText = GameObject.Find("ClearText");
        text = clearText.GetComponent<TMP_Text>();
        text.enabled = false;

        // HierarchyからNextStageButtonオブジェクトを探して取得
        nextButton = GameObject.Find("NextStageButton");
        button = nextButton.GetComponent<Button>();
        button.interactable = false;

        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (isMovingToTerminal && terminalTransform != null)
        {
            // ターミナル中心に向かって移動
            transform.position = Vector3.MoveTowards(transform.position, terminalTransform.position, speed * Time.deltaTime);
            // ターミナル中心に到達したら停止
            if (Vector3.Distance(transform.position, terminalTransform.position) < 0.005f)
            {
                HandleGoalReached();
                isMovingToTerminal = false;  // 移動完了フラグをオフ
            }
        }
        else
        {
            // 通常の移動処理
            transform.position += direction * speed * Time.deltaTime;

            // 範囲外に出た場合のチェック
            if (Mathf.Abs(transform.position.x) > gridSize.x || Mathf.Abs(transform.position.y) > gridSize.y)
            {
                Debug.Log("範囲外に侵入 - ゲームオーバー");
                StopEnergyAndGameOver();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ReflectTile"))
        {
            Debug.Log("ReflectTileに衝突しました");
            ScoreManager.instance.AddScore(10);  // ReflectTileに当たるごとに10ポイント加算

            // 現在の移動方向に基づいて反射
            if (direction == Vector3.right)
            {
                direction = Vector3.up;  // 右方向から来た場合は上方向へ反射
            }
            else if (direction == Vector3.up)
            {
                direction = Vector3.right;  // 上方向から来た場合は右方向へ反射
            }
            else if (direction == Vector3.left)
            {
                direction = Vector3.down;  // 左方向から来た場合は下方向へ反射
            }
            else if (direction == Vector3.down)
            {
                direction = Vector3.left;  // 下方向から来た場合は左方向へ反射
            }

            SnapToGrid();  // 位置をグリッドにスナップ
        }
        else if (collision.gameObject.CompareTag("BlockTile"))
        {
            Debug.Log("BlockTileに衝突 - ゲームオーバー");
            StopEnergyAndGameOver();

            SnapToGrid();  // 位置をグリッドにスナップ
        }
        else if (collision.gameObject.CompareTag("AmplifyTile"))
        {
            Debug.Log("AmplifyTileに衝突しました - スコア倍率増加");
            ScoreManager.instance.IncreaseMultiplier(0.5f);  // 例：倍率を0.5ずつ増加
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SwitchTile"))
        {
            Debug.Log("SwitchTileに衝突 - BlockTileのON/OFF切り替え");
            ToggleAllBlockTiles();
        }
        else if (collision.gameObject.CompareTag("EnergyTerminal"))
        {
            Debug.Log("ゴールに到達しました");
            terminalTransform = collision.transform;
            isMovingToTerminal = true;  // ターミナルへ向かうフラグをオン
        }
    }

    // エネルギーの方向を設定するメソッド
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    public void StopEnergyAndGameOver()
    {
        direction = Vector3.zero;  // エネルギーの動きを停止
        gameController.GameOver();  // ゲームオーバーを通知

        SnapToGrid();
    }

    // ブロックタイルの状態を切り替えるメソッド
    private void ToggleAllBlockTiles()
    {
        BlockTile[] blockTiles = FindObjectsOfType<BlockTile>();
        foreach (BlockTile blockTile in blockTiles)
        {
            blockTile.ToggleState();  // 各BlockTileの状態を切り替え
        }
    }

    // グリッドに沿って位置をスナップさせるメソッド
    void SnapToGrid()
    {
        float gridSpacing = 1.1f; // スナップするセルの間隔（cellSpacingに合わせて設定）
        float snapX = Mathf.Round(transform.position.x / gridSpacing) * gridSpacing;
        float snapY = Mathf.Round(transform.position.y / gridSpacing) * gridSpacing;
        transform.position = new Vector3(snapX, snapY, transform.position.z);
    }

    // ゴールに到達したときの処理
    void HandleGoalReached()
    {
        direction = Vector3.zero;  // エネルギーの移動を停止
        Debug.Log("ステージクリア！");
        ScoreManager.instance.AddScore(100);  // ステージクリア時に100ポイント加算

        // 倍率のリセット
        ScoreManager.instance.ResetMultiplier();

        // ハイスコアの管理
        ScoreManager.instance.SaveHighScore();

        // クリアメッセージの表示
        if (text != null && button != null)
        {
            text.enabled = true;  // クリアメッセージを表示
            button.interactable = true; // 次のステージに進むボタンをインタラクティブ化
        }
        else
        {
            Debug.LogWarning("オブジェクトが見つかりませんでした。");
        }
    }
}