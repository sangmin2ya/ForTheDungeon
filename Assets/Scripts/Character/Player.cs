using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
    public GameObject _damgageText;
    public GameObject _magicHeatEffect;
    public GameObject _physicalHeatEffect;

    public bool _isTurn = false;
    private int _statIncreaseTurnCount;
    void Awake()
    {
        _shield = new System.Tuple<AttackType, int>(_shieldType, StageManager.Instance.CurrentStage * _shieldAmount);
        // Character 객체생성
        Character = new Character(_characterType, _name, (StageManager.Instance.CurrentStage - 1) * 2, _strength, _vitality, _intelligence, _vision, _speed, _shield, _attackType);
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
    public void TakeDamage(AttackType attackType, int amount, bool isCoin)
    {
        if (!isCoin && Random.Range(0, 100) <= gameObject.GetComponent<Player>().Character.Evasion)
        {
            StartCoroutine(MoveBackAndForthRoutine());
            Debug.Log("회피!");
        }
        else
        {
            Character.TakeDamage(amount);
            StartCoroutine(AnimateText(attackType, amount.ToString()));
            Debug.Log(attackType.ToString() + " Damage Taken: " + amount);
            Debug.Log(Character.ToString());
        }
    }
    private IEnumerator MoveBackAndForthRoutine()
    {
        Vector3 originalPosition = transform.position;

        // 뒤로 이동
        yield return StartCoroutine(MoveToPosition(originalPosition, originalPosition - transform.forward * 1.5f, 0.2f));
        StartCoroutine(AnimateText(AttackType.Physical, "회피"));
        // 다시 원래 위치로 이동
        yield return StartCoroutine(MoveToPosition(transform.position, originalPosition, 0.2f));
    }
    private IEnumerator MoveToPosition(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end; // 마지막 위치 보정
    }
    private IEnumerator AnimateText(AttackType attackType, string amount)
    {
        if (amount != "회피")
            Destroy(Instantiate(attackType == AttackType.Physical ? _physicalHeatEffect : _magicHeatEffect, transform.position + transform.forward, Quaternion.identity), 3);
        // 초기 크기 설정
        Vector3 initialScale = Vector3.zero;
        Vector3 maxScale = Vector3.one; // 원하는 최대 크기
        GameObject go = Instantiate(_damgageText, gameObject.transform.Find("DamageCanvas"));
        go.GetComponent<TextMeshProUGUI>().text = (amount.Equals("0") ? "막음" : amount) + "!";
        go.GetComponent<TextMeshProUGUI>().color = attackType == AttackType.Physical ? Color.red : Color.magenta;
        // 텍스트 커지는 애니메이션
        float elapsedTime = 0;
        while (elapsedTime < 0.1f)
        {
            go.transform.localScale = Vector3.Lerp(initialScale, maxScale, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 텍스트 최대 크기 유지
        go.transform.localScale = maxScale;
        yield return new WaitForSeconds(1);

        // 텍스트 서서히 사라지는 애니메이션
        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            go.GetComponent<TextMeshProUGUI>().alpha = Mathf.Lerp(1, 0, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 텍스트 오브젝트 삭제
        Destroy(go);
    }

    private IEnumerator AnimateTextHeal(string amount)
    {
        // 초기 크기 설정
        Vector3 initialScale = Vector3.zero;
        Vector3 maxScale = Vector3.one; // 원하는 최대 크기
        GameObject go = Instantiate(_damgageText, gameObject.transform.Find("DamageCanvas"));
        go.GetComponent<TextMeshProUGUI>().text = "+" + amount;
        go.GetComponent<TextMeshProUGUI>().color = Color.red;
        // 텍스트 커지는 애니메이션
        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            go.transform.localScale = Vector3.Lerp(initialScale, maxScale, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 텍스트 최대 크기 유지
        go.transform.localScale = maxScale;
        yield return new WaitForSeconds(1);

        // 텍스트 서서히 사라지는 애니메이션
        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            go.GetComponent<TextMeshProUGUI>().alpha = Mathf.Lerp(1, 0, elapsedTime / 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 텍스트 오브젝트 삭제
        Destroy(go);
    }
    /// <summary>
    /// 회복
    /// </summary>
    public void Heal(int amount)
    {
        Character.Heal(amount);
        Debug.Log("Healed: " + amount);
        StartCoroutine(AnimateTextHeal(amount.ToString()));
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