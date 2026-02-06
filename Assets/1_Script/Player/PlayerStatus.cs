using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // Current Stats
    public float curHP { get; private set; }
    public float curStemina { get; private set; }
    public float curMoveSpeed { get; private set; }
    public float curAtkDamage { get; private set; }
    public float curJumpPower { get; private set; }

    [SerializeField] private PlayerData data;

    private void Awake()
    {
        curHP = data.maxHP;
        curStemina = data.maxStemina;
        curMoveSpeed = data.moveSpeed;
        curAtkDamage = data.atkDamage;
        curJumpPower = data.jumpPower;
    }
}
