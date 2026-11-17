using UnityEngine;

public class State : MonoBehaviour
{
    public enum EnemyState//“G‚ÌState
    {
        Idle,
        Find,
        Lost,
        Chase,
        Attack,
        Dead
    }

}
