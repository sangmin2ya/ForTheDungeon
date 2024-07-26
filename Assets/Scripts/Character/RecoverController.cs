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
        float successRate = (float)_player.Character.Attributes[StatType.Vitality] / 10;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate);
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
            CharacterManager.Instance.players.ForEach(player => player.Heal(player.Character.Health / 2));
        }
        CharacterManager.Instance._clearedRoom = true;
        TurnManager.Instance.GetComponent<TurnController>()._whileRecover = false;
    }
}
