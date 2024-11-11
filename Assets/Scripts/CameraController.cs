using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridGenerator gridGenerator;  // GridGeneratorスクリプトの参照
    public float padding = 1.0f;         // カメラの余白調整用

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        int rows = gridGenerator.rows;
        int columns = gridGenerator.columns;
        float cellSpacing = gridGenerator.cellSpacing;

        // グリッドの幅と高さを計算
        float gridWidth = (columns - 1) * cellSpacing;
        float gridHeight = (rows - 1) * cellSpacing;

        // カメラの位置を中央に設定
        transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -10);

        // カメラのサイズを調整
        Camera.main.orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2 + padding;
    }
}
