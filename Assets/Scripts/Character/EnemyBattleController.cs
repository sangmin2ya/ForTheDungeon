using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleController : MonoBehaviour
{
    private Player _enemy;
    public bool _isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponent<Player>();
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
            Debug.Log(_enemy.Character.Name + "의 턴");
            EnemyAttack();
        }
    }
    private void EnemyAttack()
    {
        if (_enemy._isTurn)
        {
            Player target = GetRandomTarget();
            Attack(target, _enemy._attackType, _enemy._attackType == AttackType.Physical ? _enemy.Character.PhysicalAttack : _enemy.Character.MagicAttack);
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
    public void Attack(Player target, AttackType attackType, int amount)
    {
        target.TakeDamage(CalculateDamage(target, attackType, amount));
        Debug.Log(_enemy.Character.Name + "의 공격!");
        TurnManager.Instance.gameObject.GetComponent<TurnController>()._endTurn = true;
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
    private void FocusedTarget()
    {
        transform.Find("Focus").gameObject.SetActive(_isSelected);
    }
}
