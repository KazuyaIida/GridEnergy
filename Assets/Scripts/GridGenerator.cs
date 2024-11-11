using UnityEngine;

[System.Serializable]
public struct BlockTileData
{
    public Vector2 position;  // BlockTile�̔z�u�ʒu
    public bool initialState; // ON/OFF�̏������
}

public class GridGenerator : MonoBehaviour
{
    public GameObject gridCellPrefab;  // GridCell�̃v���n�u
    public int rows = 5;               // �s��
    public int columns = 5;            // ��
    public float cellSpacing = 1.1f;   // �Z���Ԃ̋�������

    public GameObject energySourcePrefab;   // �G�l���M�[���̃v���n�u
    public GameObject energyTerminalPrefab; // �G�l���M�[�E�^�[�~�i���̃v���n�u

    public GameObject energyPrefab; // Energy�v���n�u���i�[

    public GameObject reflectTilePrefab;
    public GameObject blockTilePrefab;
    public GameObject amplifyTilePrefab;
    public GameObject switchTilePrefab;

    [Header("Tile Positions")]
    public Vector2[] reflectTilePositions;
    public Vector2[] amplifyTilePositions;
    public Vector2[] switchTilePositions;

    [Header("Block Tile Configurations")]
    public BlockTileData[] blockTileDataArray;  // BlockTile�̈ʒu�Ə�����Ԃ�z��Őݒ�

    private bool hasLaunchedEnergy = false;  // �G�l���M�[���ˍς݂��ǂ������Ǘ�

    private Vector2 gridSize;

    void Start()
    {
        // �S�̂̃O���b�h�͈͂̕��ƍ������v�Z
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
            LaunchEnergy(Vector3.right); // �G�l���M�[���E�����ɔ���
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
                cell.transform.parent = this.transform;  // GridParent�̎q�v�f�ɐݒ�
            }
        }
    }

    void PlaceEnergySource()
    {
        Vector3 startPosition = new Vector3(0, 0, -1); // Z����-1�ɐݒ�
        Instantiate(energySourcePrefab, startPosition, Quaternion.identity, transform);
    }

    void PlaceEnergyTerminal()
    {
        Vector3 terminalPosition = new Vector3((columns - 1) * cellSpacing, (rows - 1) * cellSpacing, -1); // Z����-1�ɐݒ�
        Instantiate(energyTerminalPrefab, terminalPosition, Quaternion.identity, transform);
    }

    // �G�l���M�[�𔭎˂��郁�\�b�h
    public void LaunchEnergy(Vector3 direction)
    {
        if (!hasLaunchedEnergy)  // �����˂̏ꍇ�̂ݔ���
        {
            Vector3 startPosition = new Vector3(0, 0, -1);  // �G�l���M�[������̈ʒu
            GameObject energy = Instantiate(energyPrefab, startPosition, Quaternion.identity);
            energy.GetComponent<EnergyController>().SetDirection(direction);
            energy.GetComponent<EnergyController>().gridSize = gridSize; // �O���b�h�T�C�Y��n��
            hasLaunchedEnergy = true;  // �G�l���M�[�𔭎ˍς݂ɐݒ�

            // ���ׂĂ�ReflectTile�̃h���b�O�𖳌���
            DisableAllReflectTileDragging();
        }
    }

    // ���˃^�C����z�u���郁�\�b�h
    void GenerateTiles()
    {
        // ReflectTile�̔z�u
        foreach (Vector2 pos in reflectTilePositions)
        {
            Vector3 worldPosition = new Vector3(pos.x * cellSpacing, pos.y * cellSpacing, -1);
            Instantiate(reflectTilePrefab, worldPosition, Quaternion.identity, transform);
        }

        // AmplifyTile�̔z�u
        foreach (Vector2 pos in amplifyTilePositions)
        {
            Vector3 worldPosition = new Vector3(pos.x * cellSpacing, pos.y * cellSpacing, -1);
            Instantiate(amplifyTilePrefab, worldPosition, Quaternion.identity, transform);
        }

        // SwitchTile�̔z�u
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

            // ������Ԃ�ݒ�
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
