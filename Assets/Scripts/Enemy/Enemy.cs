using UnityEngine;
using static State;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MaxHp;
    private float CurrentHp;

    public EnemyState currentState = EnemyState.Idle;

    private void Start()
    {
        CurrentHp = MaxHp;
    }
     
    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log("ó‚¯‚½ƒ_ƒ[ƒWF"+ damage + "“GŒ»İHPF" + CurrentHp);
    }

    private void SearchPlayer()//player‚ğ’T‚·
    { 

    }

    private void ChangeState()
    {

    }

    private void Idle()
    {
        Debug.Log("‘Ò‹@’†");
    }

    private void Chase()
    {
        Debug.Log("’ÇÕ’†");
    }

    private void Attack()
    {
        Debug.Log("UŒ‚’†");
    }

    private void Dead()
    {
        Debug.Log("€–S");
    }
}
