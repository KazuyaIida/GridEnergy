using UnityEngine;

public class BlockTile : MonoBehaviour
{
    public bool isOn = true;  // 初期状態をInspectorで設定可能
    private SpriteRenderer spriteRenderer;
    private Color onColor;   // 通常表示
    private Color offColor;  // 薄い表示

    private BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        onColor = spriteRenderer.color;
        offColor = new Color(onColor.r, onColor.g, onColor.b, 0.30f);

        boxCollider = GetComponent<BoxCollider2D>();

        UpdateTileDisplay();  // 初期状態に応じて表示を更新
    }

    public void ToggleState()
    {
        isOn = !isOn;
        UpdateTileDisplay();
    }

    private void UpdateTileDisplay()
    {
        // ON/OFFの状態に応じて色を変更
        spriteRenderer.color = isOn ? onColor : offColor;
        boxCollider.enabled = isOn;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ONのときのみEnergyを阻止
        if (collision.CompareTag("Energy") && isOn)
        {
            collision.GetComponent<EnergyController>().StopEnergyAndGameOver();
        }
    }
}
