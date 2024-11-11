using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridGenerator gridGenerator;  // GridGenerator�X�N���v�g�̎Q��
    public float padding = 1.0f;         // �J�����̗]�������p

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        int rows = gridGenerator.rows;
        int columns = gridGenerator.columns;
        float cellSpacing = gridGenerator.cellSpacing;

        // �O���b�h�̕��ƍ������v�Z
        float gridWidth = (columns - 1) * cellSpacing;
        float gridHeight = (rows - 1) * cellSpacing;

        // �J�����̈ʒu�𒆉��ɐݒ�
        transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -10);

        // �J�����̃T�C�Y�𒲐�
        Camera.main.orthographicSize = Mathf.Max(gridWidth, gridHeight) / 2 + padding;
    }
}
