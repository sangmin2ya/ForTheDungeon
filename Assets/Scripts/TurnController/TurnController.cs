using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private Queue<Player> _turnQueue = new Queue<Player>();
    [SerializeField] private TextMeshProUGUI _queueText;
    public bool _startBattle = false;
    public bool _endTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.ResetTurn();
    }

    // Update is called once per frame
    void Update()
    {
        InitQueue();
        NextTurn();
        _queueText.text = PrintQueue();
    }
    /// <summary>
    /// 유저나 적이 턴을 종료했을 때 다음 턴으로 넘어가고 해당 클래스에게 알림
    /// </summary>
    private void NextTurn()
    {
        if (_endTurn)
        {
            _endTurn = false;
            _turnQueue.Peek()._isTurn = false;
            StartCoroutine(DelayNextTurn());
        }
    }
    IEnumerator DelayNextTurn()
    {
        yield return new WaitForSeconds(1.0f);
        TurnManager.Instance.NextTurn();
        _turnQueue.Enqueue(_turnQueue.Dequeue());
        _turnQueue.Peek()._isTurn = true;
    }
    /// <summary>
    /// 배틀 혹은 룸컨트롤러에서 AddPlayer를 통해 플레이어를 추가하고 턴큐를 초기화
    /// </summary>
    private void InitQueue()
    {
        if (!_startBattle)
        {
            SortQueue();
            _startBattle = true;
        }
    }
    /// <summary>
    /// 방에 들어가면 플레이어를 추가
    /// </summary>
    /// <param name="player">추가할 플레이어 객체</param>
    public void AddPlayer(Player player)
    {
        _turnQueue.Enqueue(player);
    }
    /// <summary>
    /// 사망한 플레이어를 턴큐에서 제거
    /// </summary>
    /// <param name="player">사망한 플레이어</param>
    public void RemovePlayer(Player player)
    {
        Queue<Player> tempQueue = new Queue<Player>();

        while (_turnQueue.Count > 0)
        {
            Player p = _turnQueue.Dequeue();
            if (p != player)
            {
                tempQueue.Enqueue(p); // 해당 플레이어가 아니면 다시 넣음
            }
        }
        //새로운 큐 적용
        _turnQueue = tempQueue;
    }
    /// <summary>
    ///  턴큐를 속도순으로 정렬
    /// </summary>
    private void SortQueue()
    {
        List<Player> characters = new List<Player>(_turnQueue);
        characters.Sort((x, y) => y.Character.Evasion.CompareTo(x.Character.Evasion));
        _turnQueue = new Queue<Player>(characters);
    }
    /// <summary>
    /// 턴큐 출력
    /// </summary>
    private string PrintQueue()
    {
        int count = 10;
        List<Player> characters = new List<Player>(_turnQueue);
        string turnTimeline = "";
        for (int i = 0; i < count; i++)
        {
            turnTimeline += " / " + characters[i % characters.Count].Character.Name;
        }
        return turnTimeline;
    }
}