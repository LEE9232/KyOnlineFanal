using UnityEngine;

public class PlayerStatData
{
    [field: Header("�ʱ�ȭ �� ����")]
    [field: SerializeField] public int level { get; private set; } = 1;

    [field: Header("�ʱ�ȭ �� �ִ� ü��")]
    [field: SerializeField] public float hpMax { get; private set; }
    [SerializeField][HideInInspector] private float mHpCurrent;
    public float HpCurrent
    {
        get { return mHpCurrent; } //�������� ���� hp ���� �߰�����
    }

    [field: Header("�ʱ�ȭ �� �ִ� ����")]
    [field: SerializeField] public float mpMax { get; private set; }
    [SerializeField][HideInInspector] private float mMpCurrent;
    public float MpCurrent
    {
        get { return mMpCurrent; } //�������� ���� mp���� �߰�����
    }

    [field: Header("�ʱ�ȭ �� �⺻ ���ݷ�")]
    [field: SerializeField] public float baseAttack { get; private set; }

    ///<summary>
    /// ���� ���ݷ�
    /// </summary>
    public float AttackCurrent
    {
        get
        {
            return baseAttack; //��� ������ ���� ���ݷ� ���� �߰����ֱ�
            //�������� ���ݷ� ���� �߰�����
        }
    }

    [field: Header("�ʱ�ȭ �� �⺻ �̵��ӵ�")]
    [field: SerializeField] public float baseMovementSpeed { get; private set; }

    ///<summary>
    /// ���� �̵��ӵ�
    ///</summary>
    public float MovementSpeedCurrent
    {
        get
        {
            return baseMovementSpeed; //buff ���߿� �߰����ֱ�
        }
    }

    [field: Header("�ʱ�ȭ �� �⺻ ����")]
    [field: SerializeField] public float baseDefense { get; private set; }
    ///<summary>
    /// ���� ����
    ///</summary>
    public float DefenseCurrent
    {
        get
        {
            return baseDefense; //��� ������ ���� ���� ���� �߰�����
            //�������� ���� ���� �߰�����
        }
    }
}
