using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHp;
    private float CurrentHp;

    private void Start()
    {
        CurrentHp = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log("受けたダメージ："+ damage + "敵現在HP：" + CurrentHp);
    }

}
