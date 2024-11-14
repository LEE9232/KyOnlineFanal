using UnityEngine;

public class PlayerStatData
{
    [field: Header("초기화 시 레벨")]
    [field: SerializeField] public int level { get; private set; } = 1;

    [field: Header("초기화 시 최대 체력")]
    [field: SerializeField] public float hpMax { get; private set; }
    [SerializeField][HideInInspector] private float mHpCurrent;
    public float HpCurrent
    {
        get { return mHpCurrent; } //레벨업에 따른 hp 증가 추가예정
    }

    [field: Header("초기화 시 최대 마나")]
    [field: SerializeField] public float mpMax { get; private set; }
    [SerializeField][HideInInspector] private float mMpCurrent;
    public float MpCurrent
    {
        get { return mMpCurrent; } //레벨업에 따른 mp증가 추가예정
    }

    [field: Header("초기화 시 기본 공격력")]
    [field: SerializeField] public float baseAttack { get; private set; }

    ///<summary>
    /// 현재 공격력
    /// </summary>
    public float AttackCurrent
    {
        get
        {
            return baseAttack; //장비 장착에 따른 공격력 증가 추가해주기
            //레벨업시 공격력 증가 추가예정
        }
    }

    [field: Header("초기화 시 기본 이동속도")]
    [field: SerializeField] public float baseMovementSpeed { get; private set; }

    ///<summary>
    /// 현재 이동속도
    ///</summary>
    public float MovementSpeedCurrent
    {
        get
        {
            return baseMovementSpeed; //buff 나중에 추가해주기
        }
    }

    [field: Header("초기화 시 기본 방어력")]
    [field: SerializeField] public float baseDefense { get; private set; }
    ///<summary>
    /// 현재 방어력
    ///</summary>
    public float DefenseCurrent
    {
        get
        {
            return baseDefense; //장비 장착에 따른 방어력 증가 추가예정
            //레벨업시 방어력 증가 추가예정
        }
    }
}
