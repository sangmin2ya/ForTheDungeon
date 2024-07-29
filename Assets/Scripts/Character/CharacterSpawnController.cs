using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _playerPrefab; // 플레이어 프리팹
    [SerializeField] private List<GameObject> _enemyPrefab;  // 적 프리팹
    [SerializeField] private List<GameObject> _bossPrefab;   // 보스 프리팹

    public bool _isBattleRoom = false;
    public bool _startGame = false;
    void Start()
    {
        _startGame = true;
    }
    void Update()
    {
        if (_startGame)
        {
            StartGame();
            GameManager.Instance.SetGameState(true);
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
        //선택된 캐릭터에 따라 플레이어를 생성하여 리스트에 추가하고 CharacterManager에게 알림
        switch (CharacterManager.Instance.selectedPlayers[0].Character.Name)
        {
            case "검투사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[1], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("검사생성");
                break;
            case "마법사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[0], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("법사생성");
                break;
            case "사냥꾼":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[2], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("사냥꾼생성");
                break;
            case "주술사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[3], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("주술사생성");
                break;
            case "중갑기사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[4], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("중갑기사생성");
                break;
            case "마검사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[5], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[0], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("마검사생성");
                break;
        }
        switch (CharacterManager.Instance.selectedPlayers[1].Character.Name)
        {
            case "검투사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[1], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("검사생성");
                break;
            case "마법사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[0], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("법사생성");
                break;
            case "사냥꾼":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[2], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("사냥꾼생성");
                break;
            case "주술사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[3], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("주술사생성");
                break;
            case "중갑기사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[4], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("중갑기사생성");
                break;
            case "마검사":
                CharacterManager.Instance.AddCharacter(Instantiate(_playerPrefab[5], StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[1], Quaternion.Euler(new Vector3(0, -90, 0))).GetComponent<Player>());
                Debug.Log("마검사생성");
                break;
        }
    }
    /// <summary>
    /// 전투 방에 들어가면 적을 생성
    /// </summary>
    private void StartBattle()
    {
        if (StageManager.Instance.CurrentRoom % 6 == 5)
        {
            //보스 생성
            List<Player> bossList = new List<Player>();
            bossList.Add(Instantiate(_bossPrefab[Random.Range(0, _bossPrefab.Count)], StageManager.Instance._currentRoom.GetComponent<RoomData>()._enemyPos2, Quaternion.Euler(new Vector3(0, 90, 0))).GetComponent<Player>());
            CharacterManager.Instance.UpdateEnemy(bossList);
        }
        else
        {
            //적을 생성하여 리스트에 추가하고 CharacterManager에게 알림
            List<Player> enemyList = new List<Player>();
            enemyList.Add(Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Count)], StageManager.Instance._currentRoom.GetComponent<RoomData>()._enemyPos1, Quaternion.Euler(new Vector3(0, 90, 0))).GetComponent<Player>());
            if (Random.Range(0, 4) > 2)
                enemyList.Add(Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Count)], StageManager.Instance._currentRoom.GetComponent<RoomData>()._enemyPos2, Quaternion.Euler(new Vector3(0, 90, 0))).GetComponent<Player>());
            enemyList.Add(Instantiate(_enemyPrefab[Random.Range(0, _enemyPrefab.Count)], StageManager.Instance._currentRoom.GetComponent<RoomData>()._enemyPos3, Quaternion.Euler(new Vector3(0, 90, 0))).GetComponent<Player>());
            CharacterManager.Instance.UpdateEnemy(enemyList);
        }


        // TurnController 싱글턴의 _startBattle 설정
        StartCoroutine(DelayStartBattle());
    }
    IEnumerator DelayStartBattle()
    {
        yield return new WaitForSeconds(3.0f);
        TurnManager.Instance.GetComponent<TurnController>()._startBattle = true;
    }
}
