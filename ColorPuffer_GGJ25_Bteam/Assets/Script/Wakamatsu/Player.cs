using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("���ړ����x")]
    [SerializeField] private float horizonMoveSpeed = 5f;
    [Tooltip("�c�ړ����x")]
    [SerializeField] private float verticalMoveSpeed = 5f;
    [Tooltip("��]���x")]
    [SerializeField] private float rotationSpeed = 200f;
    [Tooltip("�߂鑬�x")]
    [SerializeField] private float resetSpeed = 200f;
    [Tooltip("�͈͎w��i��j")]
    [SerializeField] private float areaTop = 5;
    [Tooltip("�͈͎w��i���j")]
    [SerializeField] private float areaBottom = -5;
    [Tooltip("�͈͎w��i�E�j")]
    [SerializeField] private float areaRight = 5;
    [Tooltip("�͈͎w��i���j")]
    [SerializeField] private float areaLeft = -5;

    private static float upperLimit = 60f, lowerLimit = -60f;
   

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject smallPuffer;
    [SerializeField]
    private GameObject bigPuffer;
    [SerializeField]
    private Renderer small;
    [SerializeField]
    private Renderer big;
    [SerializeField]
    private Material[] color;

    private ColorType currentColorType = ColorType.Default;
    public ColorType CurrentColorType { get; private set; }

    private ColorType bubbleColor = ColorType.Default;
    private ColorType fishColor = ColorType.Default;

    private int bubbleCount = 0;
    private bool addScore = false;
    private Vector3 movement;
    private float moveX, moveY;
    private float rotationAmount;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        smallPuffer.SetActive(true);
        bigPuffer.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) { bubbleCount++; }
        movement = Vector2.zero;
        // ���͒l���擾
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        MoveArea(); //�͈͎w��
        Move(); //�ړ�
        Rotation();
        rb.velocity = movement;
        if(bubbleCount <= 2)
        {
        }
        else
        {
            smallPuffer.SetActive(false);
            bigPuffer.SetActive(true);
        }
        ChangeColor();
    }
    private void Move() //�ړ�
    {
        if (moveX != 0)
        {
            movement.x = moveX * horizonMoveSpeed;
        }
        if (moveY != 0)
        {
            movement.y = moveY * verticalMoveSpeed;
        }
    }
    private void MoveArea() //�͈͎w��
    {
        if(moveX > 0 && rb.position.x >= areaRight)
        {
            moveX = 0;
        }
        if (moveX < 0 && rb.position.x <= areaLeft)
        {
            moveX = 0;
        }
        if (moveY > 0 && rb.position.y >= areaTop)
        {
            moveY = 0;
        }
        if (moveY < 0 && rb.position.y <= areaBottom)
        {
            moveY = 0;
        }
    }
    private void Rotation()
    {
        float currentZAngle = transform.eulerAngles.z;
        if (currentZAngle > 180f) currentZAngle -= 360f; // �p�x�␳
        if (moveY != 0)
        {
            if (currentZAngle <= upperLimit && currentZAngle >= lowerLimit)
            {
                rotationAmount = moveY * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, 0, rotationAmount);
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, resetSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.TryGetComponent(out IObstacle obstacles))
        //{
        //    obstacles.HitObstacle(this);
        //    currentColorType = ColorType.Default;
        //    bubbleCount = 0;
        //}
        //if (other.gameObject.TryGetComponent(out Bubble bubble))
        //{
        //    bubble.HitObstacle(bubbleColor);
        //    bubbleCount++;
        //}
        //if (other.gameObject.TryGetComponent(out PointFish point))
        //{
        //    point.HitObstacle(fishColor);
        //}
    }
    public void HitObstacle(ColorType bubble)
    {
        bubbleColor = bubble;
        if (bubbleColor == ColorType.Blue && currentColorType == ColorType.Red)
        {
            currentColorType = ColorType.Purple;
        }
        else if (currentColorType == ColorType.Purple)
        {
        }
        else
        {
            currentColorType = bubbleColor;
        }
    }
    public void HitPoint(ColorType point)
    {
        fishColor = point;
        if(currentColorType == point)
        {
            addScore = true;
        }
    }
        private void ChangeColor()
    {
        switch (currentColorType)
        {
            case ColorType.Default:
                small.material = color[0];
                break;
            case ColorType.Red:
                small.material = color[1];
                big.material = color[1];
                break;
            case ColorType.Blue:
                small.material = color[2];
                big.material = color[2];
                break;
            case ColorType.Purple:
                small.material = color[3];
                big.material = color[3];
                break;
        }
    }
    public bool AddScore()
    {
        return addScore;
    }
    public void AddedScore()
    {
        addScore=false;
    }
}
