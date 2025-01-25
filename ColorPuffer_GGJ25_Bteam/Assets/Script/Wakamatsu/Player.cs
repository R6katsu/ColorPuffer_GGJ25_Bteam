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
    [SerializeField] private float rotationSpeed = 100f;
    [Tooltip("�͈͎w��i��j")]
    [SerializeField] private float areaTop = 5;
    [Tooltip("�͈͎w��i���j")]
    [SerializeField] private float areaBottom = -5;
    [Tooltip("�͈͎w��i�E�j")]
    [SerializeField] private float areaRight = 5;
    [Tooltip("�͈͎w��i���j")]
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
        // ���͒l���擾
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        MoveArea(); //�͈͎w��
        Move(); //�ړ�
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
    public bool IsHit()
    {
        return isHit;
    }
}
