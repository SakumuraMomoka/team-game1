using UnityEngine;

public class TamaController : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector2 direction;
    private float duration;
    private float attenuationValue;
    private float timer;

    private Rigidbody2D rb;

    public void SetDirection(Vector2 direction)//弾の進む向きを変更する関数
    {
        this.direction = direction.normalized;
    }

    private void Update()
    {
        destroy();
    }

    public void Initialize
        (float damage, float speed, float duration, float attenuationValue)//weaponのデータをゲットし、発射
    {
        this.damage = damage;
        this.speed = speed;
        this.duration = duration;
        this.attenuationValue = attenuationValue;

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)//collision
    {
        if (other.gameObject.CompareTag("Enemy"))//敵とぶつかったとき
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage - attenuationValue * timer);
            }
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Ground"))//壁や地面にぶつかったとき
        {
            Destroy(this.gameObject);
        }
    }

    private void destroy()//指定距離進んだら、球を削除する
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(this.gameObject);
        }
    }
}
