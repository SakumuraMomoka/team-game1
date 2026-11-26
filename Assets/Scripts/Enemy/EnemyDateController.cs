using UnityEngine;

public class EnemyDateController : MonoBehaviour
{
    public float moveSpeed = 3f;
    [SerializeField] private float maxHp;
    private float currentHp;

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log("受けたダメージ：" + damage + "敵現在HP：" + currentHp);
    }
}
