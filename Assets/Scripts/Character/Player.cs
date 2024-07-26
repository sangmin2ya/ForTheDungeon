using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Character Character { get; private set; }
    public CharacterType _characterType;
    public string _name;
    public int _level;
    public int _strength;
    public int _vitality;
    public int _intelligence;
    public int _vision;
    public int _speed;
    public AttackType _attackType;
    public AttackType _shieldType;
    public int _shieldAmount;
    private System.Tuple<AttackType, int> _shield;

    public bool _isTurn = false;
    private int _statIncreaseTurnCount;
    void Awake()
    {
        _shield = new System.Tuple<AttackType, int>(_shieldType, _shieldAmount);
        // Character 객체생성
        Character = new Character(_characterType, _name, _level, _strength, _vitality, _intelligence, _vision, _speed, _shield, _attackType);
        Debug.Log(Character.ToString());
    }
    void Start()
    {

    }
    void Update()
    {
        ShowTurn();
    }
    /// <summary>
    /// 경험치 획득
    /// </summary>
    /// <param name="amount">획득 경험치 양</param>
    public void GainExperience(int amount)
    {
        Character.GainExperience(amount);
        Debug.Log("Experience Gained: " + amount);
        Debug.Log(Character.ToString());
    }
    /// <summary>
    /// 이상한 사탕 사용! (즉시 레벨업)
    /// </summary>
    public void LevelUp()
    {
        Character.UseLevelUp();
        Debug.Log("Level Up!");
        Debug.Log(Character.ToString());
    }
    /// <summary>
    /// 피해
    /// </summary>
    public void TakeDamage(AttackType attackType, int amount)
    {
        Character.TakeDamage(amount);
        Debug.Log(attackType.ToString() + " Damage Taken: " + amount);
        Debug.Log(Character.ToString());
    }
    /// <summary>
    /// 회복
    /// </summary>
    public void Heal(int amount)
    {
        Character.Heal(amount);
        Debug.Log("Healed: " + amount);
        Debug.Log(Character.ToString());
    }
    /// <summary>
    /// 특정 스텟증가
    /// </summary>
    /// <param name="statType">enum StatType형 스탯값</param>
    /// <param name="amount">증가 값</param>
    public void IncreaseStat(StatType statType, int amount)
    {
        Character.IncreaseStat(statType, amount);
        Debug.Log("Stat Increased: " + statType + " by " + amount);
        Debug.Log(Character.ToString());
    }
    /// <summary>
    /// 특정 턴동안 스텟증가
    /// </summary>
    /// <param name="statType"></param>
    /// <param name="amount"></param>
    /// <param name="duration"></param>
    public IEnumerator IncreaseStatForAWhile(StatType statType, int amount, int duration)
    {
        _statIncreaseTurnCount = TurnManager.Instance.turnCount;
        Character.IncreaseStat(statType, amount);
        Debug.Log("Stat Increased for a while: " + statType + " by " + amount + " for " + duration + " seconds");
        Debug.Log(Character.ToString());

        while (TurnManager.Instance.turnCount < _statIncreaseTurnCount + duration)
        {
            yield return null;
        }
        Character.IncreaseStat(statType, -amount);
        Debug.Log("Stat has Return after " + duration + " seconds");
        Debug.Log(Character.ToString());
    }
    public void SkipTurn()
    {
        TurnManager.Instance.gameObject.GetComponent<TurnController>()._endTurn = true;

        transform.Find("RecoverCanvas").Find("Toss").gameObject.SetActive(false);
        transform.Find("RecoverCanvas").gameObject.SetActive(false);
        
        transform.Find("TrapCanvas").Find("Toss").gameObject.SetActive(false);
        transform.Find("TrapCanvas").gameObject.SetActive(false);

        GameObject.Find("CoinController").GetComponent<CoinController>().HideCoin();
    }
    private void ShowTurn()
    {
        transform.Find("Turn").gameObject.SetActive(_isTurn);
    }
}