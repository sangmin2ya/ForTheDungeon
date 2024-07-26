using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager _instance;
    public static TurnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TurnManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(TurnManager).ToString());
                    _instance = singletonObject.AddComponent<TurnManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    //턴 진행 상황
    public int turnCount { get; private set; }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 턴초기화 (다음 칸으로 이동할 때)
    /// </summary>
    public void ResetTurn()
    {
        turnCount = 0;
    }
    /// <summary>
    /// 턴 증가
    /// </summary>
    public void NextTurn()
    {
        turnCount++;
    }
    public void Reset()
    {
        turnCount = 0;
        GetComponent<TurnController>().Reset();
    }
}