using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    private int _leftPlayerCount;
    [SerializeField] private int _playerCount;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(CharacterManager).ToString());
                    _instance = singletonObject.AddComponent<CharacterManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    public List<Player> selectedPlayers { get; private set; } //선택된 플레이어 리스트
    public List<Player> players { get; private set; } //유저 리스트
    public List<Player> enemys { get; private set; } // 적 리스트
    public bool _clearedRoom;
    public bool _gameOver { get; private set; }
    public Player _deadPlayer { get; private set; }

    private bool initialized = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            if (!initialized)
            {
                Initialize();
            }
        }
    }
    private void Initialize()
    {
        players = new List<Player>();
        enemys = new List<Player>();
        selectedPlayers = new List<Player>();
        _leftPlayerCount = 0;
        initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        _playerCount = players.Count;
        if (GameManager.Instance.whileGame)
        {
            if (TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle)
            {
                CheckCharacterBattle();
            }
            else if (TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileRecover)
            {
                CheckCharacterRecover();
            }
            else if (TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileTrap)
            {
                CheckCharacterTrap();
            }
            else if (TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileRoot)
            {
                CheckCharacterRoot();
            }
            else
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i] != null)
                    {
                        players[i].gameObject.GetComponent<Player>()._isTurn = false;
                        TurnManager.Instance.gameObject.GetComponent<TurnController>()._endTurn = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 플레이어와 적의 체력을 확인하여 사망한 캐릭터를 제거
    /// 플레이어일 경우 턴에서만 제거
    /// </summary>
    private void CheckCharacterBattle()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null && players[i].Character.CurrentHealth <= 0)
            {
                TurnManager.Instance.gameObject.GetComponent<TurnController>().RemovePlayer(players[i]);
                CharacterDead(players[i]);
                _deadPlayer = players[i];
                players[i] = null;
                //players.Remove(players[i]);
                _leftPlayerCount--;
                i--;
            }
        }
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] != null && enemys[i].Character.CurrentHealth <= 0)
            {
                TurnManager.Instance.gameObject.GetComponent<TurnController>().RemovePlayer(enemys[i]);
                CharacterDead(enemys[i]);
                enemys[i].tag = "Untagged";
                Destroy(enemys[i].gameObject, 10.0f);
                enemys.Remove(enemys[i]);

                i--;
            }
        }
        if (_leftPlayerCount == 0)
        {
            _gameOver = true;
            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle = false;
        }
        else if (enemys.Count == 0)
        {
            _clearedRoom = true;
            //전투완료시 스테이지에 비례하여 경험ㅊ ㅣ획득
            foreach (var player in players)
            {
                if (player != null)
                    player.Character.GainExperience(100 * (int)Math.Pow(1.2, StageManager.Instance.CurrentStage - 1) / 3 + 10);
            }
            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle = false;
        }
    }
    private void CheckCharacterTrap()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null && players[i].Character.CurrentHealth <= 0)
            {
                TurnManager.Instance.gameObject.GetComponent<TurnController>().RemovePlayer(players[i]);
                CharacterDead(players[i]);
                _deadPlayer = players[i];
                players[i] = null;
                //players.Remove(players[i]);
                _leftPlayerCount--;
                i--;
            }
        }
        if (_leftPlayerCount == 0)
        {
            _gameOver = true;
            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle = false;
        }
    }
    private void CheckCharacterRecover()
    {

    }
    private void CheckCharacterRoot()
    {

    }
    /// <summary>
    /// 캐릭터가 죽었을 때 호출
    /// </summary>
    /// <param name="player">죽은 캐릭터</param>
    private void CharacterDead(Player player)
    {
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 force = -player.transform.forward;
        player.gameObject.GetComponent<Rigidbody>().AddForce(force * 1.5f, ForceMode.Impulse);
        StartCoroutine(PauseCharacter(player));
    }
    public void ReviveCharacter(Player player)
    {
        player.Character.Heal(1);
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                player.gameObject.transform.position = StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[i];
                players[i] = player;
                break;
            }
        }
        TurnManager.Instance.gameObject.GetComponent<TurnController>().AddPlayer(player);
        _leftPlayerCount++;
        _deadPlayer = null;
    }
    /// <summary>
    /// 시간지난 후 캐릭터 자리 고정
    /// </summary>
    /// <param name="player">죽은 캐릭터</param>
    IEnumerator PauseCharacter(Player player)
    {
        yield return new WaitForSeconds(3.0f);
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }
    /// <summary>
    /// 첫 시작 플레이어 추가할때만 호출
    /// TurnController에 플레이어 추가
    /// </summary>
    /// <param name="player">추가할 플레이어</param>
    public void AddCharacter(Player player)
    {
        players.Add(player);
        TurnManager.Instance.gameObject.GetComponent<TurnController>().AddPlayer(player);
        _leftPlayerCount++;
    }
    public void AddFirstCharacter(Player player)
    {
        selectedPlayers.Add(player);
    }
    public void ClearCharacter()
    {
        players.Clear();
        enemys.Clear();
        _leftPlayerCount = 0;
    }
    /// <summary>
    /// 방이 바뀌었을 때마다 spawnController에서 호출
    /// TurnController에 적 추가
    /// </summary>
    /// <param name="enemys">새로 등장한 적</param>
    public void UpdateEnemy(List<Player> enemys)
    {
        this.enemys = enemys;
        foreach (var enemy in enemys)
        {
            TurnManager.Instance.gameObject.GetComponent<TurnController>().AddPlayer(enemy);
        }
    }
    public void Reset()
    {
        _leftPlayerCount = 0;
        players.Clear();
        enemys.Clear();
        _clearedRoom = false;
        _gameOver = false;
        _deadPlayer = null;
    }
}
