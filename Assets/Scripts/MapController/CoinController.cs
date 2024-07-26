using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CoinController : MonoBehaviour
{
    [SerializeField] private GameObject coinImage; // 코인 UI 이미지 프리팹
    [SerializeField] private int coinCount; // 동전 갯수
    private float successProbability; // 성공 확률

    // 초기화 메서드
    public void Initialize(int count, float probability)
    {
        coinCount = count;
        successProbability = probability;
    }

    // 성공 확률을 플레이어의 스탯에 따라 설정하는 메서드
    public void SetSuccessProbability(float playerStat)
    {
        successProbability = Mathf.Clamp(playerStat / 100f, 0f, 1f);
    }

    // 동전 던지기 결과를 리턴하는 메서드
    public IEnumerator TossCoins(System.Action<int, int> onResult)
    {
        yield return StartCoroutine(DisplayTossResults(onResult));
    }

    // 동전 던짐 결과를 순차적으로 화면에 표시하는 코루틴
    private IEnumerator DisplayTossResults(System.Action<int, int> onResult)
    {
        Debug.Log("동전 던지기 시작");
        int successCount = 0;

        
        for (int i = 0; i < coinCount; i++)
        {
            // 성공 여부 결정
            bool isSuccess = Random.value <= successProbability;
            coinImage.transform.GetChild(i).GetComponent<Image>().color = isSuccess ? Color.green : Color.red;

            if (isSuccess)
            {
                successCount++;
            }

            // 다음 코인 표시까지 대기
            yield return new WaitForSeconds(0.5f); // 0.5초 대기
        }
        yield return new WaitForSeconds(1f); // 1초 대기
        //색상 초기화
        for (int i = 0; i < coinCount; i++)
        {
            coinImage.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        coinImage.SetActive(false);
        // 성공한 동전 갯수와 전체 동전 갯수를 리턴
        onResult?.Invoke(coinCount, successCount);
    }
    public void HideCoin()
    {
        coinImage.SetActive(false);
    }
}