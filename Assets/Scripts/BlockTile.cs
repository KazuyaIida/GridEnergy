using UnityEngine;

public class BlockTile : MonoBehaviour
{
    public bool isOn = true;  // ������Ԃ�Inspector�Őݒ�\
    private SpriteRenderer spriteRenderer;
    private Color onColor;   // �ʏ�\��
    private Color offColor;  // �����\��

    private BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        onColor = spriteRenderer.color;
        offColor = new Color(onColor.r, onColor.g, onColor.b, 0.30f);

        boxCollider = GetComponent<BoxCollider2D>();

        UpdateTileDisplay();  // ������Ԃɉ����ĕ\�����X�V
    }

    public void ToggleState()
    {
        isOn = !isOn;
        UpdateTileDisplay();
    }

    private void UpdateTileDisplay()
    {
        // ON/OFF�̏�Ԃɉ����ĐF��ύX
        spriteRenderer.color = isOn ? onColor : offColor;
        boxCollider.enabled = isOn;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ON�̂Ƃ��̂�Energy��j�~
        if (collision.CompareTag("Energy") && isOn)
        {
            collision.GetComponent<EnergyController>().StopEnergyAndGameOver();
        }
    }
}
