using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("横移動速度")]
    [SerializeField] private float horizonMoveSpeed = 5f;
    [Tooltip("縦移動速度")]
    [SerializeField] private float verticalMoveSpeed = 5f;
    [Tooltip("回転速度")]
    [SerializeField] private float rotationSpeed = 100f;
    [Tooltip("範囲指定（上）")]
    [SerializeField] private float areaTop = 5;
    [Tooltip("範囲指定（下）")]
    [SerializeField] private float areaBottom = -5;
    [Tooltip("範囲指定（右）")]
    [SerializeField] private float areaRight = 5;
    [Tooltip("範囲指定（左）")]
    [SerializeField] private float areaLeft = -5;

    private float upperLimit = 60f, lowerLimit = -60f;
   

    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] color;

    private ColorType currentColorType = ColorType.Default;
    private ColorType bubbleColor = ColorType.Default;
    private bool isHit = false;




    private Vector3 movement;
    private float moveX, moveY;
    private float rotationAmount;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        movement = Vector2.zero;
        // 入力値を取得
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        MoveArea(); //範囲指定
        Move(); //移動
        Rotation();
        rb.velocity = movement;
        if(Input.GetKeyDown(KeyCode.I))
        {
            currentColorType = ColorType.Red;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentColorType = ColorType.Blue;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentColorType = ColorType.Purple;
        }
        ChangeColor();
    }
    private void Move() //移動
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
    private void MoveArea() //範囲指定
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
        if (currentZAngle > 180f) currentZAngle -= 360f; // 角度補正
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IObstacle obstacles))
        {
            obstacles.HitObstacle(this);
            isHit = true;
        }
        else
        {
            isHit = false;
        }
        if (other.gameObject.TryGetComponent(out Bubble bubble))
        {
            bubble.HitObstacle(this);
            isHit = true;
        }
        else
        {
            isHit = false;
        }
    }
    public void HitObstacle(ColorType bubble)
    {
        bubbleColor = bubble;
        if (bubbleColor == ColorType.Blue && currentColorType == ColorType.Red)
        {
            currentColorType = ColorType.Purple;
        }
        else if (bubbleColor == ColorType.Default)
        {
        }
        else
        {
            currentColorType = bubbleColor;
        }
    }
        private void ChangeColor()
    {
        //switch (currentColorType)
        //{
        //    case ColorType.Default:
        //        spriteRenderer.sprite = color[0];
        //        break;
        //    case ColorType.Red:
        //        spriteRenderer.sprite = color[1];
        //        break;
        //    case ColorType.Blue:
        //        spriteRenderer.sprite = color[2];
        //        break;
        //    case ColorType.Purple:
        //        spriteRenderer.sprite = color[3]; 
        //        break;
        //}
    }
    public bool IsHit()
    {
        return isHit;
    }
}
