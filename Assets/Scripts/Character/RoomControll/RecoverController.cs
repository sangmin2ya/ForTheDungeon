using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecoverController : MonoBehaviour
{
    private Player _player;
    private CoinController _coinController;
    [SerializeField] private GameObject _healingUI;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _coinController = GameObject.Find("CoinController").GetComponent<CoinController>();
    }

    // Update is called once per frame
    void Update()
    {
        Recover();
    }
    private void Recover()
    {
        if (_player._isTurn && TurnManager.Instance.GetComponent<TurnController>()._whileRecover)
        {
            _healingUI.SetActive(true);
            //코인 갯수와 코인당 성공확률을 표시
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _player.Character.Name + "의 턴";
        }
        else
        {
            _healingUI.SetActive(false);
        }
    }
    public void ShowCoin()
    {
        //던지기 버튼 활성화
        _healingUI.transform.Find("Toss").gameObject.SetActive(true);
        //성공확률 계산 추후 수정필요
        float successRate = (float)_player.Character.Attributes[StatType.Vitality] / 100;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate, true);
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        coinImage.SetActive(true);
        coinImage.transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "성공 확률: " + (successRate * 100) + "%";
    }
    public void RecoverCoinToss()
    {
        _healingUI.transform.Find("Toss").gameObject.SetActive(false);
        _healingUI.SetActive(false);
        StartCoroutine(_coinController.TossCoins(OnCoinsTossed));
    }
    private void OnCoinsTossed(int totlaCoins, int successCoins)
    {
        //성공한 코인 갯수가 3개 이상이면 회복
        if (successCoins >= 3)
        {
            if (CharacterManager.Instance._deadPlayer != null)
                CharacterManager.Instance.ReviveCharacter(CharacterManager.Instance._deadPlayer);
            CharacterManager.Instance.players.ForEach(player => player.Heal(player.Character.Health / 2));
        }
        else if (successCoins >= 2)
        {
            CharacterManager.Instance.players.ForEach(player => player.Heal(player.Character.Health / 4));
        }
        CharacterManager.Instance._clearedRoom = true;
        TurnManager.Instance.GetComponent<TurnController>()._whileRecover = false;
        StartCoroutine(UseHeart());
    }
    private IEnumerator UseHeart()
    {
        // GameObject 찾기
        GameObject mapObject = GameObject.FindGameObjectWithTag("Map").transform.Find("Heart").gameObject;

        // 이동할 시간이 1초로 설정
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 startScale = new Vector3(1, 1, 1);
        Vector3 endScale = new Vector3(0, 0, 0);
        // 초기 위치 설정
        mapObject.transform.localScale = startScale;

        while (elapsedTime < duration)
        {
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
            // 비율 계산
            float t = elapsedTime / duration;
            // 위치 보간
            mapObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            // 다음 프레임까지 대기
            yield return null;
        }

        // 종료 위치에 정확히 배치
        mapObject.transform.localScale = endScale;
    }
}