using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab; // 플레이어 프리팹
    [SerializeField] private GameObject _enemyPrefab;  // 적 프리팹

    public bool _isBattleRoom = false;
    public bool _startGame = false;
    void Start()
    {

    }
    void Update()
    {
        if (_startGame)
        {
            StartGame();
            _startGame = false;
        }
        if (_isBattleRoom)
        {
            StartBattle();
            _isBattleRoom = false;
        }
    }
    /// <summary>
    /// 게임 시작 시 플레이어를 생성
    /// </summary>
    private void StartGame()
    {
        // 플레이어를 생성하고 CharacterManager에게 알림
        CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab).GetComponent<Player>());
    }
    /// <summary>
    /// 전투 방에 들어가면 적을 생성
    /// </summary>
    private void StartBattle()
    {
        //적을 생성하여 리스트에 추가하고 CharacterManager에게 알림
        List<Player> enemyList = new List<Player>();
        enemyList.Add(Instantiate(_enemyPrefab).GetComponent<Player>());
        CharacterManager.Instance.UpdateEnemy(enemyList);

        // TurnController 싱글턴의 _startBattle 설정
        StartCoroutine(DelayStartBattle());
    }
    IEnumerator DelayStartBattle()
    {
        yield return new WaitForSeconds(1.0f);
        TurnManager.Instance.GetComponent<TurnController>()._startBattle = true;
    }
}
