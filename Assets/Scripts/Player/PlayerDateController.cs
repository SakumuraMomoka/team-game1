using UnityEngine;

public class PlayerDateController : MonoBehaviour
{
    [SerializeField] private float basicSpeed;
    [SerializeField] private float dashAddSpeed;
    public float currentSpeed;
    [SerializeField] private float maxHp;
    [SerializeField] private float maxStamina;
    private float currentHp;
    private float currentStamina;
    private float timer;
    [SerializeField] private float staminaAddTimer;
    [SerializeField] private float dashStaminaCost;
    public float avoidStaminaCost;
    [SerializeField] private float staminaRecoverySpeed;
    public float avoidDuration;
    public float avoidSpeed;

    private Input input;

    private void Start()
    {
        input = GetComponent<Input>();

        currentHp = maxHp;
        currentStamina = maxStamina;
        currentSpeed = basicSpeed;
    }

    private void Update()
    {
        SpeedController();

        DefaultAddStamina();
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log("Player現在HP : " + currentHp);
    }

    public void AddHp(float recovery)
    {
        currentHp += recovery;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        Debug.Log("Player現在HP : " + currentHp);
    }

    public bool UseStamina(float stamina)
    {
        if (currentStamina < stamina)
        {
            Debug.Log("スタミナが足りない！");
            return false;
        }

        currentStamina -= stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        Debug.Log("Player現在Stamina : " + currentStamina);

        timer = 0;//スタミナを使うたびにタイマーをリセットする

        return true;
    }

    public void AddStamina(float stamina)
    {
        currentStamina += stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        Debug.Log("Player現在Stamina : " + currentStamina);
    }

    private void DefaultAddStamina()//スタミナを使用してから指定した時間がたったら、スタミナを回復する
    {
        if (currentStamina >= maxStamina)
        {
            timer = 0;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= staminaAddTimer)
        {
            AddStamina(staminaRecoverySpeed * Time.deltaTime);
        }
    }

    private void SpeedController()
    {
        if (input.DashPressed && UseStamina(dashStaminaCost * Time.deltaTime))
        {
            currentSpeed = basicSpeed + dashAddSpeed;
        }
        else
        {
            currentSpeed = basicSpeed;
        }
    }
}
