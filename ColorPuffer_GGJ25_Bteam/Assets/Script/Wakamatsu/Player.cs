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

    [SerializeField]
    private Rigidbody2D rb;

    private bool isHit = false;



    private Vector3 movement;
    private float moveX, moveY;

    void Start()
    {
    }

    void Update()
    {
        movement = Vector2.zero;
        // 入力値を取得
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        MoveArea(); //範囲指定
        Move(); //移動
        rb.velocity = movement;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IObstacle obstacles))
        {
            obstacles.HitObstacle(this.transform);
            isHit = true;
        }
        else
        {
            isHit = false;
        }
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
    public bool IsHit()
    {
        return isHit;
    }
}
