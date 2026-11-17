using UnityEngine;

public class EnemyDateController : MonoBehaviour
{
    public float moveSpeed = 3f;
    [SerializeField] private float MaxHp;
    private float CurrentHp;

    private void Start()
    {
        CurrentHp = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log("受けたダメージ：" + damage + "敵現在HP：" + CurrentHp);
    }
}
