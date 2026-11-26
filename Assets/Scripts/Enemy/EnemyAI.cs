using UnityEngine;
using Pathfinding;
using static State;

public class EnemyAI : MonoBehaviour
{
    [Header("視界設定")]
    [SerializeField] private float viewDistance;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask playerLayer;

    [Header("Find")]
    [SerializeField] private float attackDistance;

    [SerializeField] private LayerMask obstacleLayer;

    private Vector2 lastPlayerPosition;
    private bool reachedLastPosition = false;
    [SerializeField] private float arrivalDistance;

    private float lostTimer = 0;
    [SerializeField] private float lostTime;

    public EnemyState currentState = EnemyState.Idle;

    [Header("参照")]
    EnemyDateController enemy;
    private Transform player;
    [SerializeField] Transform currentPlayerTrans;

    [SerializeField] private WeaponDate weaponDate;
    private Weapon weapon;

    private AIPath aiPath;

    private void Start()
    {
        enemy = GetComponent<EnemyDateController>();
        weapon = GetComponent<Weapon>();
        aiPath = GetComponent<AIPath>();

        aiPath.maxSpeed = enemy.moveSpeed;
        aiPath.isStopped = true;
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
                if (!CanSeePlayer(player)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Lost:
                Lost();
                break;

            case EnemyState.Chase:
                Chase();
                if (!CanSeePlayer(player)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Attack:
                Attack();
                if (!CanSeePlayer(player)) ChangeState(EnemyState.Lost);
                break;

            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)//collision
    {
        if (other.gameObject.CompareTag("Tama"))//敵とぶつかったとき
        {
            player = currentPlayerTrans;
            ChangeState(EnemyState.Chase);
        }
    }

    private bool CanSeePlayer(Transform target)//playerが見えたか判定
    {
        if (target == null) return false;

        // Playerへの方向
        Vector2 direction =
            ((Vector2)target.position -
            (Vector2)transform.position).normalized;

        // Playerまでの距離
        float distance = Vector2.Distance(
            transform.position,
            target.position
        );

        // ① 視界距離の判定
        if (distance > viewDistance)
        {
            return false;
        }

        // ② 視野角の判定
        // 敵の正面は -transform.up
        float angle = Vector2.Angle(-transform.up, direction);

        if (angle > viewAngle / 2f)
        {
            return false;
        }

        // ③ 遮蔽物の判定
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            obstacleLayer
        );

        if (hit.collider != null)
        {
            return false;
        }

        return true;
    }

    private void SearchPlayer() // Playerを探す動き
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(
            transform.position,
            viewDistance,
            playerLayer
        );

        foreach (Collider2D target in targets)
        {
            if (CanSeePlayer(target.transform))
            {
                player = target.transform;
                ChangeState(EnemyState.Find);
                return;
            }
        }
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

    private void RotateToMovement()
    {
        Vector2 direction = aiPath.desiredVelocity;

        if (direction.sqrMagnitude < 0.01f) return;

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360f * Time.deltaTime
        );
    }

    private void Idle()//何もない間の動き
    {
        SearchPlayer();
    }

    private void Find()
    {
        float fromPlayerDistance = Vector2.Distance(transform.position, player.position);//enemyとplayerの距離

        if (attackDistance > fromPlayerDistance)//距離内の時攻撃し、距離がある場合移動する

        {
            ChangeState(EnemyState.Attack);
        }
        else
        {
            ChangeState(EnemyState.Chase);
        }
    }

    private void Lost()
    {
        // Playerを再発見した場合
        if (CanSeePlayer(player))
        {
            lostTimer = 0;
            reachedLastPosition = false;

            ChangeState(EnemyState.Find);
            return;
        }

        // 最後に見た位置を目的地に設定
        aiPath.destination = lastPlayerPosition;

        aiPath.isStopped = false;

        RotateToMovement();

        // 経路計算が完了し、目的地に到着したか確認
        if (!aiPath.pathPending && aiPath.reachedDestination)
        {
            aiPath.isStopped = true;

            if (!reachedLastPosition)
            {
                reachedLastPosition = true;
                lostTimer = 0;

                Debug.Log("最後に見た位置に到着！索敵開始");
            }

            lostTimer += Time.deltaTime;

            if (lostTimer >= lostTime)
            {
                ChangeState(EnemyState.Idle);
            }
        }
    }

    private void Chase()
    {
        if (player == null) return;

        // Playerの位置を記憶
        lastPlayerPosition = player.position;

        // Playerを目的地にする
        aiPath.destination = player.position;

        // 移動開始
        aiPath.isStopped = false;

        RotateToMovement();

        float fromPlayerDistance = Vector2.Distance(transform.position, player.position);//enemyとplayerの距離

        if (attackDistance > fromPlayerDistance)//距離が一定以内になったら攻撃する

        {
            ChangeState(EnemyState.Attack);
        }
    }

    private void Attack()
    {
        if (player == null) return;

        RotateToMovement();

        // 敵からPlayerに向かう方向
        Vector2 direction =
            ((Vector2)player.position -
            (Vector2)transform.position).normalized;

        //playerの方向へ回転
        if (direction.sqrMagnitude < 0.01f) return;

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            360f * Time.deltaTime
        );

        // Playerに向かって発射
        weapon.TryShoot(direction);

        float fromPlayerDistance = Vector2.Distance(transform.position, player.position);//enemyとplayerの距離

        if (attackDistance < fromPlayerDistance)//距離がある場合移動する

        {
            ChangeState(EnemyState.Chase);
        }
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
                aiPath.isStopped = true;
                break;

            case EnemyState.Find:
                Debug.Log("見つけた！");
                aiPath.isStopped = true;
                break;

            case EnemyState.Chase:
                Debug.Log("追跡開始！");
                aiPath.isStopped = false;
                break;

            case EnemyState.Lost:
                Debug.Log("見失った");

                // 見失った瞬間の位置を保存
                if (player != null)
                {
                    lastPlayerPosition = player.position;
                }

                lostTimer = 0;
                reachedLastPosition = false;

                aiPath.isStopped = false;
                break;

            case EnemyState.Attack:
                Debug.Log("攻撃開始！");
                aiPath.isStopped = true;
                break;

            case EnemyState.Dead:
                Debug.Log("死亡");
                aiPath.isStopped = true;
                break;
        }
    }
}
