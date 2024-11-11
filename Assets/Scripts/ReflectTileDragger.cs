using UnityEngine;

public class ReflectTileDragger : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private bool isDraggable = true;
    private Vector3 originalPosition;  // 元の位置を記憶

    void Start()
    {
        originalPosition = transform.position;  // 初期位置を記憶
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

            // 他のオブジェクトと重なっているかチェック
            if (IsOverlapping())
            {
                transform.position = originalPosition;  // 重なっていたら元の位置に戻す
            }
            else
            {
                originalPosition = transform.position;  // 新しい位置を元の位置として記憶
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
        float cellSize = 1.1f;  // グリッドのセルサイズに合わせる
        float snapX = Mathf.Round(transform.position.x / cellSize) * cellSize;
        float snapY = Mathf.Round(transform.position.y / cellSize) * cellSize;
        transform.position = new Vector3(snapX, snapY, transform.position.z);
    }

    // 他のオブジェクトと重なっているか確認するメソッド
    private bool IsOverlapping()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);  // 0.5fは当たり判定の半径
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject &&  // 自分以外のオブジェクトと
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
