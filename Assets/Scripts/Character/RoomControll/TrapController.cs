using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TrapController : MonoBehaviour
{
    private Player _player;
    [SerializeField] private GameObject _trapUI;
    private CoinController _coinController;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _coinController = GameObject.Find("CoinController").GetComponent<CoinController>();
    }

    // Update is called once per frame
    void Update()
    {
        UnTrap();
    }
    private void UnTrap()
    {
        GameObject.Find("CoinCanvas").transform.Find("TrapExplain").gameObject.SetActive(TurnManager.Instance.GetComponent<TurnController>()._whileTrap);
        if (_player._isTurn && TurnManager.Instance.GetComponent<TurnController>()._whileTrap)
        {
            _trapUI.SetActive(true);
            //코인 갯수와 코인당 성공확률을 표시
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _player.Character.Name + "의 턴";
        }
        else
        {
            _trapUI.SetActive(false);
        }
    }
    public void ShowCoin()
    {
        //던지기 버튼 활성화
        _trapUI.transform.Find("Toss").gameObject.SetActive(true);
        //성공확률 계산 추후 수정필요
        float successRate = (float)_player.Character.Attributes[StatType.Vision] / 100;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate + 0.15f, true);
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        coinImage.SetActive(true);
        coinImage.transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "성공 확률(인지): " + (successRate * 100) + "%";
    }
    public void TrapCoinToss()
    {
        _trapUI.transform.Find("Toss").gameObject.SetActive(false);
        _trapUI.SetActive(false);
        _player._isTurn = false;
        StartCoroutine(_coinController.TossCoins(OnCoinsTossed));
    }
    private void OnCoinsTossed(int totlaCoins, int successCoins)
    {
        //성공한 코인 갯수가 2개 이하면 피해
        if (successCoins <= 1)
        {
            CharacterManager.Instance.players.ForEach(player => player.TakeDamage(AttackType.Physical, player.Character.Health / 10));
            TurnManager.Instance.GetComponent<TurnController>()._endTurn = true;
        }
        else if (successCoins <= 2)
        {
            TurnManager.Instance.GetComponent<TurnController>()._endTurn = true;
        }
        else
        {
            CharacterManager.Instance._clearedRoom = true;

            TurnManager.Instance.gameObject.GetComponent<TurnController>()._whileTrap = false;
            StartCoroutine(MoveMap());

            foreach (var player in CharacterManager.Instance.players)
            {
                if (player != null)
                    player.Character.GainExperience(100 * (int)Math.Pow(1.2, StageManager.Instance.CurrentStage - 1) / 3);
            }
        }
    }
    private IEnumerator MoveMap()
    {
        // GameObject 찾기
        GameObject mapObject = GameObject.FindGameObjectWithTag("Map").transform.Find("Bridge").gameObject;

        // 이동할 시간이 1초로 설정
        float duration = 1.0f;
        float elapsedTime = 0f;
        Vector3 startLocation = new Vector3(-15, -0.3f, 0);
        Vector3 endLocation = new Vector3(-5, -0.3f, 0);
        // 초기 위치 설정
        mapObject.transform.localPosition = startLocation;

        while (elapsedTime < duration)
        {
            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;
            // 비율 계산
            float t = elapsedTime / duration;
            // 위치 보간
            mapObject.transform.localPosition = Vector3.Lerp(startLocation, endLocation, t);
            // 다음 프레임까지 대기
            yield return null;
        }

        // 종료 위치에 정확히 배치
        mapObject.transform.localPosition = endLocation;
    }
}
