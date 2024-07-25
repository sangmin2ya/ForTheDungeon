using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    private int _leftPlayerCount;
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
    public List<Player> players { get; private set; } //유저 리스트
    public List<Player> enemys { get; private set; } // 적 리스트
    public bool _clearedRoom { get; private set; }
    public bool _gameOver { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        enemys = new List<Player>();
        _leftPlayerCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle)
        {
            CheckCharacter();
        }
    }
    /// <summary>
    /// 플레이어와 적의 체력을 확인하여 사망한 캐릭터를 제거
    /// 플레이어일 경우 턴에서만 제거
    /// </summary>
    private void CheckCharacter()
    {
        foreach (var player in players)
        {
            if (player.Character.CurrentHealth <= 0)
            {
                TurnManager.Instance.gameObject.GetComponent<TurnController>().RemovePlayer(player);
                CharacterDead(player);
                _leftPlayerCount--;
            }
        }
        foreach (var enemy in enemys)
        {
            if (enemy.Character.CurrentHealth <= 0)
            {
                TurnManager.Instance.gameObject.GetComponent<TurnController>().RemovePlayer(enemy);
                enemys.Remove(enemy);
                CharacterDead(enemy);
            }
        }
        if (_leftPlayerCount == 0)
        {
            _gameOver = true;
            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle = false;
        }
        else if(enemys.Count == 0)
        {
            _clearedRoom = true;
            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileBattle = false;
        }
    }
    /// <summary>
    /// 캐릭터가 죽었을 때 호출
    /// </summary>
    /// <param name="player">죽은 캐릭터</param>
    private void CharacterDead(Player player)
    {
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 force = player.transform.position - new Vector3(-3, 0, 0);
        player.gameObject.GetComponent<Rigidbody>().AddForce(force / 2, ForceMode.Impulse);
        StartCoroutine(PauseCharacter(player));
    }
    /// <summary>
    /// 시간지난 후 캐릭터 자리 고정
    /// </summary>
    /// <param name="player">죽은 캐릭터</param>
    IEnumerator PauseCharacter(Player player)
    {
        yield return new WaitForSeconds(3.0f);
        player.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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
}
