using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Character Character { get; private set; }
    private int _statIncreaseTurnCount;
    void Start()
    {
        // Character 객체생성
        Character = new Character(CharacterType.Player, 1, 10, 10, 5, 5, 8);
        Debug.Log(Character.ToString());
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
    public void TakeDamage(int amount)
    {
        Character.TakeDamage(amount);
        Debug.Log("Damage Taken: " + amount);
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
}