using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private GameObject _battleUI;
    private Player _player;
    [SerializeField] private bool _selectingTarget = false; //테스트용 [serializeField]
    private Player _previousTarget;
    private AttackType _attackType;
    public bool _isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = gameObject.GetComponent<Player>();
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
        if (_player._isTurn)
        {
            _battleUI.SetActive(true);
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _player.Character.Name + "의 턴";
        }
        else
        {
            _battleUI.SetActive(false);
        }
    }
    /// <summary>
    /// 공격!
    /// </summary>
    /// <param name="target">공격대상</param>
    /// <param name="attackType">공격타입</param>
    /// <param name="amount">데미지</param>
    public IEnumerator Attack(Player target, AttackType attackType, int amount)
    {
        Vector3 originalPosition = _player.transform.position; // 원래 위치 저장
        Vector3 targetPosition = target.transform.position + target.transform.forward * 2; // 타겟 위치 저장

        yield return StartCoroutine(MoveToPosition(targetPosition, 0.5f));

        yield return new WaitForSeconds(0.5f);
        target.TakeDamage(attackType, CalculateDamage(target, attackType, amount));
        _selectingTarget = false;
        Debug.Log(_player.Character.Name + "의 공격!");

        yield return StartCoroutine(MoveToPosition(originalPosition, 0.5f));

        TurnManager.Instance.gameObject.GetComponent<TurnController>()._endTurn = true;
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
        if (target.Character.Shield.Item1 == attackType)
        {
            return Math.Max(amount - target.Character.Shield.Item2, 0);
        }
        else
        {
            return amount;
        }
    }
    /// <summary>
    /// 공격할 타겟 선택
    /// </summary>
    private void HandleTargetSelection()
    {
        if (_selectingTarget)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Player targetPlayer = hit.collider.GetComponent<Player>();
                if (targetPlayer != null && targetPlayer.Character.Type == CharacterType.Enemy)
                {
                    if (_previousTarget != null && _previousTarget != targetPlayer)
                    {
                        // 이전 타겟의 선택을 해제
                        _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    }
                    // 새로운 타겟을 선택
                    targetPlayer.GetComponent<EnemyBattleController>()._isSelected = true;
                    _previousTarget = targetPlayer;
                    //자신의 공격타입에 따라 공격
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(Attack(targetPlayer, _player._attackType, _player._attackType == AttackType.Physical ? _player.Character.PhysicalAttack : _player.Character.MagicAttack));
                        _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    }
                }
                else if (_previousTarget != null)
                {
                    // 마우스가 다른 곳으로 이동했을 때, 이전 타겟의 선택 해제
                    _previousTarget.GetComponent<EnemyBattleController>()._isSelected = false;
                    _previousTarget = null;
                }
            }
        }
    }
    /// <summary>
    /// 단일공격 선택
    /// </summary>
    public void SelectSingleAttack()
    {
        _selectingTarget = true;

    }
    /// <summary>
    /// 다중공격 선택
    /// </summary>
    public void SelectWideAttack()
    {
        _selectingTarget = true;
    }
    /// <summary>
    /// 타겟이 되면 마커 표시
    /// </summary>
    private void FocusedTarget()
    {
        transform.Find("Focus").gameObject.SetActive(_isSelected);
    }
}
