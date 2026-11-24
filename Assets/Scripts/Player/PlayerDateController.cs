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
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        Debug.Log("Playeråªç›HP : " + currentHp);
    }

    public void AddHp(float recovery)
    {
        currentHp += recovery;
    }

    public void UseStamina(float stamina)
    {
        currentStamina -= stamina;
    }

    public void AddStamina(float stamina)
    {
        currentStamina += stamina;
    }

    private void SpeedController()
    {
        if (input.DashPressed)
        {
            currentSpeed = basicSpeed + dashAddSpeed;
        }
        else
        {
            currentSpeed = basicSpeed;
        }
    }
}
