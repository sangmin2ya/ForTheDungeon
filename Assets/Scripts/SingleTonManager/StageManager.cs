using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager: MonoBehaviour
{
    private static StageManager _instance;

    // 싱글턴 인스턴스를 반환
    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StageManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(StageManager).ToString());
                    _instance = singletonObject.AddComponent<StageManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // 현재 스테이지 값을 저장
    public int CurrentStage { get; private set; }
    public int CurrentRoom { get; private set; }
    public bool _enterDoor = false;
    public GameObject _currentRoom;
    // 생성자 - 초기 스테이지 값을 설정
    private StageManager()
    {
        CurrentStage = 1; // 스테이지 초기값
        CurrentRoom = 0; // 방 초기값
    }
    /// <summary>
    /// 다음 방으로 이동
    /// </summary>
    public void NextRoom()
    {
        CurrentRoom++;
    }
    // 스테이지 값을 1 증가시키는 메서드
    public void NextStage()
    {
        CurrentStage++;
        CurrentRoom = 0;
    }
}
