using UnityEngine;
using UnityEngine.AdaptivePerformance;
using static State;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    [Header("視界設定")]
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask playerLayer;

    [Header("Find")]
    [SerializeField] private float attackDistance;

    [SerializeField] private LayerMask obstacleLayer;

    private float lostTimer = 0;
    [SerializeField] private float lostTime;

    public EnemyState currentState = EnemyState.Idle;

    [Header("参照")]
    EnemyDateController enemy;
    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyDateController>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Find:
                Find();
                if (!CanSeePlayer(player.transform)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Lost:
                Lost();
                break;

            case EnemyState.Chase:
                Chase();
                if (!CanSeePlayer(player.transform)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Attack:
                Attack();
                if (!CanSeePlayer(player.transform)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState != EnemyState.Chase &&
            currentState != EnemyState.Lost)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private bool CanSeePlayer(Transform target)
    {
        Vector2 direction =
            ((Vector2)target.position -
            (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(
            transform.position,
            target.position
        );

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            obstacleLayer
        );

        // 障害物に当たらなければ発見できる
        return hit.collider == null;
    }

    private void SearchPlayer()//playerを探す
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            transform.position,
            viewDistance,
            playerLayer
            );

        foreach (Collider2D target in targets)
        {
            Vector2 direction =
                ((Vector2)target.transform.position -
                (Vector2)transform.position).normalized;

            // 敵の正面とPlayerの方向の角度を調べる
            float angle = Vector2.Angle(-transform.up, direction);

            if (angle <= viewAngle / 2f)
            {
                if (CanSeePlayer(target.transform))
                {
                    player = target.transform;
                    ChangeState(EnemyState.Find);
                    return;
                }
            }
        }

        currentState = EnemyState.Idle;
    }

    void OnDrawGizmosSelected()// Sceneビューに視界の範囲を表示
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = -transform.up;

        float halfAngle = viewAngle / 2f;

        Vector3 left = Quaternion.Euler(
            0, 0, halfAngle
        ) * forward;

        Vector3 right = Quaternion.Euler(
            0, 0, -halfAngle
        ) * forward;

        Gizmos.DrawRay(transform.position, left * viewDistance);
        Gizmos.DrawRay(transform.position, right * viewDistance);

        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    private void Idle()//何もない間の動き
    {
        SearchPlayer();
    }

    private void Find()
    {
        float fromPlayerDistance = Vector2.Distance(transform.position, player.position);//enemyとplayerの距離

        if (attackDistance < fromPlayerDistance)//距離がある場合移動する

        {
            ChangeState(EnemyState.Attack);
        }
        else
        {
            ChangeState(EnemyState.Chase);
        }
       
        //視界から外れたら、外れた瞬間のplayerの位置へ移動し、索敵する
    }

    private void Lost()
    {
        if (player == null)
        {
            Debug.Log("player == null");
            return;
        }

        if (!CanSeePlayer(playerTrans.transform)) //10秒間視界に入らなかったら、見失う＞ これまだやってない>元の場所に戻る
        {
            Debug.Log(player.transform);

            lostTimer += Time.deltaTime;

            // まだ最後に見た位置に到着していない場合
            if (this.transform.position != player.transform.position)
            {
                Vector2 direction =
           ((Vector2)player.position - rb.position).normalized;

                float targetAngle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

                Quaternion targetRotation =
                    Quaternion.Euler(0f, 0f, targetAngle);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360f * Time.deltaTime
                );

                rb.linearVelocity = direction * enemy.moveSpeed;
            }

            if (lostTimer > lostTime)
            {
                ChangeState(EnemyState.Idle);

                lostTimer = 0;
            }
        }
        else if (CanSeePlayer(playerTrans.transform))
        {
            ChangeState(EnemyState.Find);
            lostTimer = 0;
            player = playerTrans;
        }
    }

    private void Chase()
    {
        if (player == null) return;

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        float targetAngle =
        Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360f * Time.deltaTime
        );

        rb.linearVelocity = direction * enemy.moveSpeed;
    }

    private void Attack()
    {
    }

    private void Dead()
    {
    }

    private void ChangeState(EnemyState newState)//stateを変え、変えたstateに応じた処理を変えた瞬間に一度だけ実行する
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                Debug.Log("待機開始");
                break;

            case EnemyState.Find:
                Debug.Log("見つけた！");

                //playerを向く
                Vector2 direction =
               ((Vector2)player.position - rb.position).normalized;

                float targetAngle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

                Quaternion targetRotation =
                    Quaternion.Euler(0f, 0f, targetAngle);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360f * Time.deltaTime
                );
                break;

            case EnemyState.Lost:
                Debug.Log("見失った");
                lostTimer = 0;

                break;

            case EnemyState.Chase:
                Debug.Log("追跡開始！");
                break;

            case EnemyState.Attack:
                Debug.Log("攻撃開始！");
                break;

            case EnemyState.Dead:
                Debug.Log("死亡");
                break;
        }
    }
}
