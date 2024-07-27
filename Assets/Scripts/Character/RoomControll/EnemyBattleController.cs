using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBattleController : MonoBehaviour
{
    private Player _enemy;
    private CoinController _coinController;
    public bool _isSelected = false;
    private bool _attackOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponent<Player>();
        _coinController = GameObject.Find("CoinController").GetComponent<CoinController>();

    }

    // Update is called once per frame
    void Update()
    {
        Battle();
        FocusedTarget();
    }
    private void Battle()
    {
        if (_enemy._isTurn)
        {
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _enemy.Character.Name + "의 턴";
            EnemyAttack();
        }
    }
    private void EnemyAttack()
    {
        if (_enemy._isTurn && _attackOnce)
        {
            ShowCoinAndAttack();
            _attackOnce = false;
        }
    }
    /// <summary>
    /// 체력이 높은 플레이어를 우선으로 랜덤 타겟 선택
    /// </summary>
    /// <returns>타겟</returns>
    private Player GetRandomTarget()
    {
        List<Player> players = CharacterManager.Instance.players;

        // 체력이 높은 순으로 정렬
        players.Sort((x, y) => y.Character.Health.CompareTo(x.Character.Health));

        // 전체 체력 합산
        int totalHealth = 0;
        foreach (Player player in players)
        {
            totalHealth += player.Character.Health;
        }

        // 가중치 계산 (최소 가중치 20% 보장)
        List<float> weights = new List<float>();
        foreach (Player player in players)
        {
            float weight = (float)player.Character.Health / totalHealth;
            weights.Add(Mathf.Max(weight, 0.2f)); // 최소 20% 보장
        }

        // 총 가중치 합계
        float totalWeight = 0;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        // 랜덤 선택
        float randomWeightPoint = UnityEngine.Random.Range(0, totalWeight);
        float currentWeightSum = 0;
        //랜덤값이 누적 가중치보다 작으면 해당 플레이어 선택
        for (int i = 0; i < players.Count; i++)
        {
            currentWeightSum += weights[i];
            if (randomWeightPoint < currentWeightSum)
            {
                return players[i];
            }
        }

        //모든 가중치가 0일 경우 첫 번째 플레이어를 리턴
        return players.Count > 0 ? players[0] : null;
    }
    /// <summary>
    /// 공격!
    /// </summary>
    /// <param name="target">공격대상</param>
    /// <param name="attackType">공격타입</param>
    /// <param name="amount">데미지</param>
    public IEnumerator Attack(Player target, AttackType attackType, int amount)
    {
        Vector3 originalPosition = _enemy.transform.position; // 원래 위치 저장
        Vector3 targetPosition = target.transform.position + target.transform.forward * 2; // 타겟 위치 저장

        yield return StartCoroutine(MoveToPosition(targetPosition, 0.5f));

        yield return new WaitForSeconds(0.5f);
        target.TakeDamage(attackType, CalculateDamage(target, attackType, amount));
        Debug.Log(_enemy.Character.Name + "의 공격!");
        _enemy._isTurn = false;
        _attackOnce = true;
        yield return StartCoroutine(MoveToPosition(originalPosition, 0.5f));

        TurnManager.Instance.gameObject.GetComponent<TurnController>()._endTurn = true;
    }
    /// <summary>
    /// 해당위치로 이동
    /// </summary>
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = _enemy.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            _enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _enemy.transform.position = targetPosition;
    }
    /// <summary>
    /// 공격력과 쉴드를 계산하여 피해량을 계산
    /// </summary>
    /// <param name="target">공격대상</param>
    /// <param name="attackType">공격타입</param>
    /// <param name="amount">데미지</param>
    /// <returns></returns>
    private int CalculateDamage(Player target, AttackType attackType, int amount)
    {
        if (target.Character.Shield.Item1 == attackType)
        {
            return Math.Max(amount - target.Character.Shield.Item2, 0);
        }
        else
        {
            return amount;
        }
    }
    public void ShowCoinAndAttack()
    {
        //성공확률 계산 추후 수정필요
        float successRate = (float)_enemy.Character.Attributes[_enemy._attackType == AttackType.Physical ? StatType.Strength : StatType.Intelligence] / 100;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate, false);
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        coinImage.SetActive(true);
        coinImage.transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "성공 확률: " + (successRate * 100) + "%";
        AttackCoinToss();
    }
    public void AttackCoinToss()
    {
        StartCoroutine(_coinController.TossCoins(OnCoinsTossed));
    }
    private void OnCoinsTossed(int totlaCoins, int successCoins)
    {
        //성공한 코인만큼 공격
        Player target = GetRandomTarget();
        StartCoroutine(Attack(target, _enemy._attackType, (int)Math.Round((double)(_enemy._attackType == AttackType.Physical ? _enemy.Character.PhysicalAttack : _enemy.Character.MagicAttack) * ((float)successCoins / totlaCoins))));
    }
    private void FocusedTarget()
    {
        transform.Find("Focus").gameObject.SetActive(_isSelected);
    }
}
