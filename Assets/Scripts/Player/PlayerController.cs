using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("パラメーター")]
    public float moveSpeed;
    public Vector2 playerDirection;

    [Header("参照")]
    private Input input;
    private Rigidbody2D rb;
    private Camera mainCamera;

    private void Awake()
    {
        input = GetComponent<Input>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // プレイヤーからカーソルへの方向を取得
        Vector3 worldPosition = GetMouseWorldPosition(transform.position.z);
        playerDirection = worldPosition - transform.position;

        Move1();

        /*if (input.SpacePressed)
        {
            Move2();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }*/

        Rotate();
    }

    private Vector3 GetMouseWorldPosition(float targetZ)//カーソルの位置を求める
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        float distance = targetZ - mainCamera.transform.position.z;

        return mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, distance)
        );
    }

    private void Move1()//wasd移動
    {
        Vector2 walk = input.MoveInput.normalized;//移動入力の値を取得

        rb.linearVelocity = walk * moveSpeed;
    }

    private void Move2()//カーソルの方向に進む移動
    {
        // カーソルが近すぎる場合は停止
        if (playerDirection.sqrMagnitude < 0.0001f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        playerDirection.Normalize();//ベクトルの大きさを１にする

        // カーソルの方向に移動
        rb.linearVelocity = playerDirection * moveSpeed;
    }

    private void Rotate()//カーソルの位置に合わせてplayerを回転させる
    {
        if (playerDirection.sqrMagnitude < 0.0001f)//近すぎるときはreturn
            return;

        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
    }
}
