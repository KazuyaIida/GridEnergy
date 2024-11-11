using UnityEngine;

public class ReflectTileDragger : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private bool isDraggable = true;
    private Vector3 originalPosition;  // ���̈ʒu���L��

    void Start()
    {
        originalPosition = transform.position;  // �����ʒu���L��
    }

    void OnMouseDown()
    {
        if (isDraggable)
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            SnapToGrid();

            // ���̃I�u�W�F�N�g�Əd�Ȃ��Ă��邩�`�F�b�N
            if (IsOverlapping())
            {
                transform.position = originalPosition;  // �d�Ȃ��Ă����猳�̈ʒu�ɖ߂�
            }
            else
            {
                originalPosition = transform.position;  // �V�����ʒu�����̈ʒu�Ƃ��ċL��
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void SnapToGrid()
    {
        float cellSize = 1.1f;  // �O���b�h�̃Z���T�C�Y�ɍ��킹��
        float snapX = Mathf.Round(transform.position.x / cellSize) * cellSize;
        float snapY = Mathf.Round(transform.position.y / cellSize) * cellSize;
        transform.position = new Vector3(snapX, snapY, transform.position.z);
    }

    // ���̃I�u�W�F�N�g�Əd�Ȃ��Ă��邩�m�F���郁�\�b�h
    private bool IsOverlapping()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);  // 0.5f�͓����蔻��̔��a
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject &&  // �����ȊO�̃I�u�W�F�N�g��
                (collider.CompareTag("EnergyTerminal") || collider.CompareTag("EnergySource") ||
                 collider.CompareTag("AmplifyTile") || collider.CompareTag("BlockTile") ||
                 collider.CompareTag("ReflectTile") || collider.CompareTag("SwitchTile")))
            {
                return true;
            }
        }
        return false;
    }

    public void DisableDragging()
    {
        isDraggable = false;
    }
}
