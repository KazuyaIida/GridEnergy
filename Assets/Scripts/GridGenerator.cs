using UnityEngine;

[System.Serializable]
public struct BlockTileData
{
    public Vector2 position;  // BlockTileの配置位置
    public bool initialState; // ON/OFFの初期状態
}

public class GridGenerator : MonoBehaviour
{
    public GameObject gridCellPrefab;  // GridCellのプレハブ
    public int rows = 5;               // 行数
    public int columns = 5;            // 列数
    public float cellSpacing = 1.1f;   // セル間の距離調整

    public GameObject energySourcePrefab;   // エネルギー源のプレハブ
    public GameObject energyTerminalPrefab; // エネルギー・ターミナルのプレハブ

    public GameObject energyPrefab; // Energyプレハブを格納

    public GameObject reflectTilePrefab;
    public GameObject blockTilePrefab;
    public GameObject amplifyTilePrefab;
    public GameObject switchTilePrefab;

    [Header("Tile Positions")]
    public Vector2[] reflectTilePositions;
    public Vector2[] amplifyTilePositions;
    public Vector2[] switchTilePositions;

    [Header("Block Tile Configurations")]
    public BlockTileData[] blockTileDataArray;  // BlockTileの位置と初期状態を配列で設定

    private bool hasLaunchedEnergy = false;  // エネルギー発射済みかどうかを管理

    private Vector2 gridSize;

    void Start()
    {
        // 全体のグリッド範囲の幅と高さを計算
        float gridWidth = columns * cellSpacing;
        float gridHeight = rows * cellSpacing;
        gridSize = new Vector2(gridWidth, gridHeight);

        GenerateGrid();
        PlaceEnergySource();
        PlaceEnergyTerminal();
        GenerateTiles();
        GenerateBlockTiles();

        hasLaunchedEnergy = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchEnergy(Vector3.right); // エネルギーを右方向に発射
        }
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * cellSpacing, row * cellSpacing, 0);
                GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity);
                cell.transform.parent = this.transform;  // GridParentの子要素に設定
            }
        }
    }

    void PlaceEnergySource()
    {
        Vector3 startPosition = new Vector3(0, 0, -1); // Z軸を-1に設定
        Instantiate(energySourcePrefab, startPosition, Quaternion.identity, transform);
    }

    void PlaceEnergyTerminal()
    {
        Vector3 terminalPosition = new Vector3((columns - 1) * cellSpacing, (rows - 1) * cellSpacing, -1); // Z軸を-1に設定
        Instantiate(energyTerminalPrefab, terminalPosition, Quaternion.identity, transform);
    }

    // エネルギーを発射するメソッド
    public void LaunchEnergy(Vector3 direction)
    {
        if (!hasLaunchedEnergy)  // 未発射の場合のみ発射
        {
            Vector3 startPosition = new Vector3(0, 0, -1);  // エネルギー源からの位置
            GameObject energy = Instantiate(energyPrefab, startPosition, Quaternion.identity);
            energy.GetComponent<EnergyController>().SetDirection(direction);
            energy.GetComponent<EnergyController>().gridSize = gridSize; // グリッドサイズを渡す
            hasLaunchedEnergy = true;  // エネルギーを発射済みに設定

            // すべてのReflectTileのドラッグを無効化
            DisableAllReflectTileDragging();
        }
    }

    // 反射タイルを配置するメソッド
    void GenerateTiles()
    {
        // ReflectTileの配置
        foreach (Vector2 pos in reflectTilePositions)
        {
            Vector3 worldPosition = new Vector3(pos.x * cellSpacing, pos.y * cellSpacing, -1);
            Instantiate(reflectTilePrefab, worldPosition, Quaternion.identity, transform);
        }

        // AmplifyTileの配置
        foreach (Vector2 pos in amplifyTilePositions)
        {
            Vector3 worldPosition = new Vector3(pos.x * cellSpacing, pos.y * cellSpacing, -1);
            Instantiate(amplifyTilePrefab, worldPosition, Quaternion.identity, transform);
        }

        // SwitchTileの配置
        foreach (Vector2 pos in switchTilePositions)
        {
            Vector3 worldPosition = new Vector3(pos.x * cellSpacing, pos.y * cellSpacing, -1);
            Instantiate(switchTilePrefab, worldPosition, Quaternion.identity, transform);
        }
    }

    void GenerateBlockTiles()
    {
        foreach (BlockTileData data in blockTileDataArray)
        {
            Vector3 worldPosition = new Vector3(data.position.x * cellSpacing, data.position.y * cellSpacing, -1);
            GameObject blockTile = Instantiate(blockTilePrefab, worldPosition, Quaternion.identity, transform);

            // 初期状態を設定
            BlockTile tileScript = blockTile.GetComponent<BlockTile>();
            tileScript.isOn = data.initialState;
        }
    }

    private void DisableAllReflectTileDragging()
    {
        ReflectTileDragger[] reflectTiles = FindObjectsOfType<ReflectTileDragger>();
        foreach (ReflectTileDragger tile in reflectTiles)
        {
            tile.DisableDragging();
        }
    }
}
