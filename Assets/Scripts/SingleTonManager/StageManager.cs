using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private static StageManager _instance;
    private bool _enterDungeon = true;
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
        TurnManager.Instance.ResetTurn();
    }
    // 스테이지 값을 1 증가시키는 메서드
    public void NextStage()
    {
        CurrentStage++;
        CurrentRoom = 0;
        TurnManager.Instance.ResetTurn();
        StartCoroutine(FadeInOut(1.0f));
    }
    private IEnumerator FadeInOut(float f)
    {
        // Canvas에서 GoDown 이미지 오브젝트 찾기
        Image image = GameObject.Find("Canvas").transform.Find("GoDown").GetComponent<Image>();
        TextMeshProUGUI text = GameObject.Find("Canvas").transform.Find("GoDown").GetChild(0).GetComponent<TextMeshProUGUI>();
        if (image == null)
        {
            Debug.LogError("GoDown not found in Canvas.");
            yield break;
        }

        // 오브젝트 활성화
        image.gameObject.SetActive(true);

        // 투명도를 0에서 1로 변화 (1초 동안)
        float fadeDuration = 1.0f;
        for (float t = 0; t <= f; t += Time.deltaTime)
        {
            Color color = image.color;
            Color textColor = text.color;
            color.a = Mathf.Lerp(0, 1, t / f);
            textColor.a = Mathf.Lerp(0, 1, t / f);
            image.color = color;
            text.color = textColor;
            yield return null;
        }

        // 투명도를 1로 유지 (1초 동안)
        yield return new WaitForSeconds(1.0f);

        // 투명도를 1에서 0으로 변화 (1초 동안)
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            Color color = image.color;
            Color textColor = text.color;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            textColor.a = Mathf.Lerp(1, 0, t / fadeDuration);
            image.color = color;
            text.color = textColor;

            yield return null;
        }
        // 오브젝트 비활성화
        image.gameObject.SetActive(false);
    }
    public void Reset()
    {
        CurrentStage = 1;
        CurrentRoom = 0;
        _enterDungeon = true;
        _enterDoor = false;
    }
}
