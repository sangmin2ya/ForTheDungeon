using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class BattleController : MonoBehaviour
{
    [SerializeField] private GameObject _battleUI;
    private Player _player;
    private CoinController _coinController;
    [SerializeField] private bool _selectingSingleTarget = false; //테스트용 [serializeField]
    [SerializeField] private bool _selectingMultiTarget = false; //테스트용 [serializeField]
    private Player _previousTarget;
    private Player _targetPlayer;
    private List<Player> _previousTargets = new List<Player>();
    private List<Player> _targetPlayers = new List<Player>();
    private AttackType _attackType;
    private float _additionalRate = 0;
    public bool _isSelected = false;
    public bool _isCoinUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = gameObject.GetComponent<Player>();
        _coinController = GameObject.Find("CoinController").GetComponent<CoinController>();
    }

    // Update is called once per frame
    void Update()
    {
        Battle();
        FocusedTarget();
        HandleTargetSelection();
    }
    private void Battle()
    {
        if (_player._isTurn && TurnManager.Instance.GetComponent<TurnController>()._whileBattle)
        {
            _battleUI.SetActive(true);
            CoinUse();
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _player.Character.Name + "의 턴";
        }
        else
        {
            _battleUI.SetActive(false);
        }
        _player.transform.Find("BattleCanvas").Find("BattleExplain").gameObject.SetActive(_selectingSingleTarget || _selectingMultiTarget);
    }
    /// <summary>
    /// 공격!
    /// </summary>
    /// <param name="target">공격대상</param>
    /// <param name="attackType">공격타입</param>
    /// <param name="amount">데미지</param>
    public IEnumerator PhysicalSingleAttack(Player target, AttackType attackType, int amount)
    {
        Vector3 originalPosition = _player.transform.position; // 원래 위치 저장
        Vector3 targetPosition = target.transform.position + target.transform.forward * 2; // 타겟 위치 저장

        yield return StartCoroutine(MoveToPosition(targetPosition, 0.5f));

        yield return new WaitForSeconds(0.5f);
        target.TakeDamage(attackType, CalculateDamage(target, attackType, amount), _isCoinUsed);
        _selectingSingleTarget = false;
        Debug.Log(_player.Character.Name + "의 공격!");

        yield return StartCoroutine(MoveToPosition(originalPosition, 0.5f));
        _isCoinUsed = false;
        _player.SkipTurn();
    }
    public IEnumerator PhysicalMultiAttack(List<Player> targets, AttackType attackType, int amount)
    {
        Vector3 originalPosition = _player.transform.position; // 원래 위치 저장

        yield return StartCoroutine(MoveToPosition(new Vector3(-5 - (31 * StageManager.Instance.CurrentRoom), 1, 0), 0.5f));

        yield return new WaitForSeconds(0.5f);
        foreach (var target in targets)
        {
            target.TakeDamage(attackType, CalculateDamage(target, attackType, amount), _isCoinUsed);
        }
        _isCoinUsed = false;
        _selectingSingleTarget = false;
        Debug.Log(_player.Character.Name + "의 공격!");

        yield return StartCoroutine(MoveToPosition(originalPosition, 0.5f));
        _isCoinUsed = false;
        _player.SkipTurn();
    }
    public IEnumerator MagicSingleAttack(Player target, AttackType attackType, int amount)
    {
        yield return new WaitForSeconds(0.5f);
        target.TakeDamage(attackType, CalculateDamage(target, attackType, amount), _isCoinUsed);   
        _selectingSingleTarget = false;
        Debug.Log(_player.Character.Name + "의 공격!");
        yield return new WaitForSeconds(0.5f);
        _isCoinUsed = false;
        _player.SkipTurn();
    }
    /// <summary>
    /// 해당위치로 이동
    /// </summary>
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = _player.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            _player.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _player.transform.position = targetPosition;
    }
    /// <summary>
    /// 공격력과 쉴드를 계산하여 피해량을 계산
    /// </summary>
    /// <param name="target">공격대상</param>
    /// <param name="attackType">공격타입</param>
    /// <param name="amount">데미지</param>
    /// <returns>총 데미지</returns>
    private int CalculateDamage(Player target, AttackType attackType, int amount)
    {
        Debug.Log("피해량:  " + amount);

        if (target.Character.Shield.Item1 == attackType)
        {
            Debug.Log("결과1: " + Math.Max(amount - target.Character.Shield.Item2, 0));
            return Math.Max(amount - target.Character.Shield.Item2, 0);
        }
        else
        {
            Debug.Log("결과2: " + amount);
            return amount;
        }
    }
    /// <summary>
    /// 공격할 타겟 선택
    /// </summary>
    private void HandleTargetSelection()
    {
        if (_selectingSingleTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _targetPlayer = hit.collider.GetComponent<Player>();
                if (_targetPlayer != null && _targetPlayer.Character.Type == CharacterType.Enemy)
                {
                    ShowCoin();
                    if (_previousTarget != null && _previousTarget != _targetPlayer)
                    {
                        // 이전 타겟의 선택을 해제
                        _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    }
                    // 새로운 타겟을 선택
                    _targetPlayer.GetComponent<EnemyBattleController>()._isSelected = true;
                    _previousTarget = _targetPlayer;
                    //자신의 공격타입에 따라 공격

                    if (Input.GetMouseButtonDown(0))
                    {
                        AttackCoinToss();
                        _targetPlayer.GetComponent<EnemyBattleController>()._isSelected = false;
                        _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    }
                }
                else if (_previousTarget != null)
                {
                    // 마우스가 다른 곳으로 이동했을 때, 이전 타겟의 선택 해제
                    _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    _previousTarget = null;
                    GameObject.Find("CoinController").GetComponent<CoinController>().HideCoin();
                }
            }
        }
        if (_selectingMultiTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _targetPlayer = hit.collider.GetComponent<Player>();
                if (_targetPlayer != null && _targetPlayer.Character.Type == CharacterType.Enemy)
                {
                    _targetPlayers = CharacterManager.Instance.enemys;
                    ShowCoin();
                    // 새로운 타겟을 선택
                    foreach (var target in _targetPlayers)
                    {
                        target.GetComponent<EnemyBattleController>()._isSelected = true;
                    }
                    _previousTargets = _targetPlayers;
                    //자신의 공격타입에 따라 공격

                    if (Input.GetMouseButtonDown(0))
                    {
                        AttackCoinToss();
                        foreach (var Target in CharacterManager.Instance.enemys)
                        {
                            if (Target != null)
                            {
                                // 이전 타겟의 선택을 해제
                                Target.GetComponent<EnemyBattleController>()._isSelected = false;
                            }
                        }
                    }
                }
                else if (_previousTargets != null)
                {
                    foreach (var previouseTarget in _previousTargets)
                    {
                        if (previouseTarget != null)
                        {
                            // 이전 타겟의 선택을 해제
                            previouseTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                        }
                    }
                    _previousTargets = null;
                    GameObject.Find("CoinController").GetComponent<CoinController>().HideCoin();
                }
            }
        }
    }
    public void ShowCoin()
    {
        float successRate = 0;
        //성공확률 계산 추후 수정필요
        if (_selectingSingleTarget)
            successRate = (float)_player.Character.Attributes[_player._attackType == AttackType.Physical ? StatType.Strength : StatType.Intelligence] / 100;
        if (_selectingMultiTarget)
            successRate = (float)_player.Character.Attributes[_player._attackType == AttackType.Physical ? StatType.Strength : StatType.Intelligence] / 100 * 0.6f;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate, true);
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        coinImage.SetActive(true);
        coinImage.transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "성공 확률(" + (_player._attackType == AttackType.Physical ? "근력" : "지능") + "): " + (int)((successRate + _additionalRate) * 100) + "%";
    }
    private void CoinUse()
    {
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        if (Input.GetMouseButtonDown(1))
        {
            if (coinImage.activeSelf && _player.Character.Focus > 0)
            {
                //float successRate = _coinController.successProbability + 0.15f;
                //_coinController.Initialize(3, successRate, true);
                _additionalRate += 0.15f;
                _isCoinUsed = true;
                _coinController.UseCoin();
                _player.Character.ConsumeFocus();
            }
            else if (_player.Character.Focus == 0)
            {
                GameManager.Instance.ShowMessage("집중력이 부족합니다.");
            }
            else
            {
                GameManager.Instance.ShowMessage("집중력을 사용할 수 있는 상태가 아닙니다.");
            }
        }
    }
    /// <summary>
    /// 단일공격 동전
    /// </summary>
    public void AttackCoinToss()
    {
        if (_selectingSingleTarget)
            StartCoroutine(_coinController.TossCoins(OnSingleCoinsTossed));
        else if (_selectingMultiTarget)
            StartCoroutine(_coinController.TossCoins(OnMultiCoinsTossed));
        _selectingSingleTarget = false;
        _selectingMultiTarget = false;
    }
    private void OnSingleCoinsTossed(int totlaCoins, int successCoins)
    {
        //성공한 코인만큼 공격
        if (_player._attackType == AttackType.Physical && _player.Character.Name != "사냥꾼")
        {
            StartCoroutine(PhysicalSingleAttack(_targetPlayer, _player._attackType, (int)Math.Round((double)(_player.Character.PhysicalAttack * ((float)successCoins / totlaCoins)))));
        }
        else if (_player.Character.Name == "사냥꾼")
        {
            StartCoroutine(MagicSingleAttack(_targetPlayer, _player._attackType, (int)Math.Round((double)(_player.Character.PhysicalAttack * ((float)successCoins / totlaCoins)))));
        }
        else
        {
            StartCoroutine(MagicSingleAttack(_targetPlayer, _player._attackType, (int)Math.Round((double)(_player.Character.MagicAttack * ((float)successCoins / totlaCoins)))));
        }
        _additionalRate = 0;
    }
    /// <summary>
    /// 다중공격 동전
    /// </summary>
    private void OnMultiCoinsTossed(int totlaCoins, int successCoins)
    {
        List<Player> targetPlayers = CharacterManager.Instance.enemys;
        //성공한 코인만큼 공격
        if (_player._attackType == AttackType.Physical && _player.Character.Name != "사냥꾼")
        {
            StartCoroutine(PhysicalMultiAttack(targetPlayers, _player._attackType, (int)Math.Round((double)(_player.Character.PhysicalAttack * 0.6 * ((float)successCoins / totlaCoins)))));
        }
        else if (_player.Character.Name == "사냥꾼")
        {
            foreach (var target in targetPlayers)
            {
                StartCoroutine(MagicSingleAttack(target, _player._attackType, (int)Math.Round((double)(_player.Character.PhysicalAttack * 0.6 * ((float)successCoins / totlaCoins)))));
            }
        }
        else
        {
            foreach (var target in targetPlayers)
            {
                StartCoroutine(MagicSingleAttack(target, _player._attackType, (int)Math.Round((double)(_player.Character.MagicAttack * 0.6 * ((float)successCoins / totlaCoins)))));
            }
        }
        _additionalRate = 0;
    }
    /// <summary>
    /// 단일공격 선택
    /// </summary>
    public void SelectSingleAttack()
    {
        _player.transform.Find("BattleCanvas").Find("BattleExplain").gameObject.SetActive(true);
        _player.transform.Find("BattleCanvas").Find("BattleExplain").GetChild(0).GetComponent<TextMeshProUGUI>().text
            = "[<color=\"green\">단일 공격</color>]\n하나의 적에게\n<color=\"red\">" + (_player._attackType == AttackType.Physical ? (_player.Character.PhysicalAttack + " 물리피해") : (_player.Character.MagicAttack + " 마법피해"));
        _selectingMultiTarget = false;
        _selectingSingleTarget = true;
    }
    /// <summary>
    /// 다중공격 선택
    /// </summary>
    public void SelectWideAttack()
    {
        _player.transform.Find("BattleCanvas").Find("BattleExplain").gameObject.SetActive(true);
        _player.transform.Find("BattleCanvas").Find("BattleExplain").GetChild(0).GetComponent<TextMeshProUGUI>().text
            = "[<color=\"green\">광역 스킬</color>]\n모든 적에게\n<color=\"red\">" + (_player._attackType == AttackType.Physical ? (_player.Character.PhysicalAttack * 0.6f + " 물리피해") : ((_player.Character.MagicAttack * 0.6f) + " 마법피해"));
        _selectingSingleTarget = false;
        _selectingMultiTarget = true;
    }
    /// <summary>
    /// 타겟이 되면 마커 표시
    /// </summary>
    private void FocusedTarget()
    {
        transform.Find("Focus").gameObject.SetActive(_isSelected);
    }
}
