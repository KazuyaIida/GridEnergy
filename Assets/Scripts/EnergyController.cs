using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyController : MonoBehaviour
{
    public float speed = 5f; // �G�l���M�[�̈ړ����x
    private Vector3 direction; // �G�l���M�[�̈ړ�����

    private bool isMovingToTerminal = false;
    private Transform terminalTransform;

    private GameObject clearText;  // �N���A���b�Z�[�W�p��UI�I�u�W�F�N�g
    private TMP_Text text;

    private GameObject nextButton;
    private Button button;

    public Vector2 gridSize;  // GridGenerator����n�����O���b�h�T�C�Y

    private GameController gameController;

    void Start()
    {
        // Hierarchy����ClearText�I�u�W�F�N�g��T���Ď擾
        clearText = GameObject.Find("ClearText");
        text = clearText.GetComponent<TMP_Text>();
        text.enabled = false;

        // Hierarchy����NextStageButton�I�u�W�F�N�g��T���Ď擾
        nextButton = GameObject.Find("NextStageButton");
        button = nextButton.GetComponent<Button>();
        button.interactable = false;

        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (isMovingToTerminal && terminalTransform != null)
        {
            // �^�[�~�i�����S�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, terminalTransform.position, speed * Time.deltaTime);
            // �^�[�~�i�����S�ɓ��B�������~
            if (Vector3.Distance(transform.position, terminalTransform.position) < 0.005f)
            {
                HandleGoalReached();
                isMovingToTerminal = false;  // �ړ������t���O���I�t
            }
        }
        else
        {
            // �ʏ�̈ړ�����
            transform.position += direction * speed * Time.deltaTime;

            // �͈͊O�ɏo���ꍇ�̃`�F�b�N
            if (Mathf.Abs(transform.position.x) > gridSize.x || Mathf.Abs(transform.position.y) > gridSize.y)
            {
                Debug.Log("�͈͊O�ɐN�� - �Q�[���I�[�o�[");
                StopEnergyAndGameOver();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ReflectTile"))
        {
            Debug.Log("ReflectTile�ɏՓ˂��܂���");
            ScoreManager.instance.AddScore(10);  // ReflectTile�ɓ����邲�Ƃ�10�|�C���g���Z

            // ���݂̈ړ������Ɋ�Â��Ĕ���
            if (direction == Vector3.right)
            {
                direction = Vector3.up;  // �E�������痈���ꍇ�͏�����֔���
            }
            else if (direction == Vector3.up)
            {
                direction = Vector3.right;  // ��������痈���ꍇ�͉E�����֔���
            }
            else if (direction == Vector3.left)
            {
                direction = Vector3.down;  // ���������痈���ꍇ�͉������֔���
            }
            else if (direction == Vector3.down)
            {
                direction = Vector3.left;  // ���������痈���ꍇ�͍������֔���
            }

            SnapToGrid();  // �ʒu���O���b�h�ɃX�i�b�v
        }
        else if (collision.gameObject.CompareTag("BlockTile"))
        {
            Debug.Log("BlockTile�ɏՓ� - �Q�[���I�[�o�[");
            StopEnergyAndGameOver();

            SnapToGrid();  // �ʒu���O���b�h�ɃX�i�b�v
        }
        else if (collision.gameObject.CompareTag("AmplifyTile"))
        {
            Debug.Log("AmplifyTile�ɏՓ˂��܂��� - �X�R�A�{������");
            ScoreManager.instance.IncreaseMultiplier(0.5f);  // ��F�{����0.5������
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("SwitchTile"))
        {
            Debug.Log("SwitchTile�ɏՓ� - BlockTile��ON/OFF�؂�ւ�");
            ToggleAllBlockTiles();
        }
        else if (collision.gameObject.CompareTag("EnergyTerminal"))
        {
            Debug.Log("�S�[���ɓ��B���܂���");
            terminalTransform = collision.transform;
            isMovingToTerminal = true;  // �^�[�~�i���֌������t���O���I��
        }
    }

    // �G�l���M�[�̕�����ݒ肷�郁�\�b�h
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    public void StopEnergyAndGameOver()
    {
        direction = Vector3.zero;  // �G�l���M�[�̓������~
        gameController.GameOver();  // �Q�[���I�[�o�[��ʒm

        SnapToGrid();
    }

    // �u���b�N�^�C���̏�Ԃ�؂�ւ��郁�\�b�h
    private void ToggleAllBlockTiles()
    {
        BlockTile[] blockTiles = FindObjectsOfType<BlockTile>();
        foreach (BlockTile blockTile in blockTiles)
        {
            blockTile.ToggleState();  // �eBlockTile�̏�Ԃ�؂�ւ�
        }
    }

    // �O���b�h�ɉ����Ĉʒu���X�i�b�v�����郁�\�b�h
    void SnapToGrid()
    {
        float gridSpacing = 1.1f; // �X�i�b�v����Z���̊Ԋu�icellSpacing�ɍ��킹�Đݒ�j
        float snapX = Mathf.Round(transform.position.x / gridSpacing) * gridSpacing;
        float snapY = Mathf.Round(transform.position.y / gridSpacing) * gridSpacing;
        transform.position = new Vector3(snapX, snapY, transform.position.z);
    }

    // �S�[���ɓ��B�����Ƃ��̏���
    void HandleGoalReached()
    {
        direction = Vector3.zero;  // �G�l���M�[�̈ړ����~
        Debug.Log("�X�e�[�W�N���A�I");
        ScoreManager.instance.AddScore(100);  // �X�e�[�W�N���A����100�|�C���g���Z

        // �{���̃��Z�b�g
        ScoreManager.instance.ResetMultiplier();

        // �n�C�X�R�A�̊Ǘ�
        ScoreManager.instance.SaveHighScore();

        // �N���A���b�Z�[�W�̕\��
        if (text != null && button != null)
        {
            text.enabled = true;  // �N���A���b�Z�[�W��\��
            button.interactable = true; // ���̃X�e�[�W�ɐi�ރ{�^�����C���^���N�e�B�u��
        }
        else
        {
            Debug.LogWarning("�I�u�W�F�N�g��������܂���ł����B");
        }
    }
}