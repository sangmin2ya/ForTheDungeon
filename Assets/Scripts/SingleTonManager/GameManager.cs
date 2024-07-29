using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField] private GameObject _messagePanel;

    // 싱글턴 인스턴스에 접근할 수 있는 프로퍼티
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 새로운 GameManager 객체를 생성하여 할당
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<GameManager>();
                singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";

                // 싱글턴 객체는 다른 씬으로 전환될 때 파괴되지 않음
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }

    // whileGame 프로퍼티
    public bool whileGame { get; private set; }
    private void Awake()
    {
        // 싱글턴 인스턴스가 이미 존재하는 경우, 새로운 인스턴스를 파괴
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // whileGame 상태를 설정하는 메서드
    public void SetGameState(bool state)
    {
        whileGame = state;
    }
    public void ShowMessage(string message)
    {
        StartCoroutine(PrintMsg(message));
    }
    IEnumerator PrintMsg(string message)
    {
        _messagePanel.SetActive(true);
        _messagePanel.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(3f);
        _messagePanel.SetActive(false);
    }
}
