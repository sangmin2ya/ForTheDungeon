using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardController : MonoBehaviour
{
    private Player _player;
    private CoinController _coinController;
    [SerializeField] private GameObject _rewardUI;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _coinController = GameObject.Find("CoinController").GetComponent<CoinController>();
    }

    void Update()
    {
        Reward();
    }
    private void Reward()
    {
        GameObject.Find("CoinCanvas").transform.Find("RewardExplain").gameObject.SetActive(TurnManager.Instance.GetComponent<TurnController>()._whileRoot);
        if (_player._isTurn && TurnManager.Instance.GetComponent<TurnController>()._whileRoot)
        {
            _rewardUI.SetActive(true);
            //코인 갯수와 코인당 성공확률을 표시
            GameObject.Find("Canvas").transform.Find("TurnUser").GetComponent<TextMeshProUGUI>().text = _player.Character.Name + "의 턴";
        }
        else
        {
            _rewardUI.SetActive(false);
        }
    }
    public void ShowCoin()
    {
        //던지기 버튼 활성화
        _rewardUI.transform.Find("Toss").gameObject.SetActive(true);
        //성공확률 계산 추후 수정필요
        float successRate = (float)_player.Character.Attributes[StatType.Vision] / 100;
        //코인 갯수와 성공확률을 전달
        _coinController.Initialize(3, successRate, true);
        GameObject coinImage = GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject;
        coinImage.SetActive(true);
        coinImage.transform.Find("SuccessRate").GetComponent<TextMeshProUGUI>().text = "성공 확률(인지): " + (successRate * 100) + "%";
    }
    public void RewardCoinToss()
    {
        _rewardUI.transform.Find("Toss").gameObject.SetActive(false);
        _rewardUI.SetActive(false);
        _player._isTurn = false;
        StartCoroutine(_coinController.TossCoins(OnCoinsTossed));
    }
    private void OnCoinsTossed(int totlaCoins, int successCoins)
    {
        GameObject mimic = GameObject.FindGameObjectWithTag("Map").transform.Find("Box").gameObject;
        TurnManager.Instance.GetComponent<TurnController>()._whileRoot = false;
        //성공한 코인 갯수가 3개 이상이면 회복
        switch (successCoins)
        {

            case 1:
                Debug.Log("꽝");
                CharacterManager.Instance._clearedRoom = true;
                break;
            case 2:
                Debug.Log("보너스!");
                CharacterManager.Instance._clearedRoom = true;
                string item = "";
                switch (Random.Range(0, 5))
                {
                    case 0:
                        _player.GetComponent<Inventory>().AddItem(ItemType.Potion, 1);
                        item = "<color=\"green\">포션 1개</color>";
                        break;
                    case 1:
                        _player.GetComponent<Inventory>().AddItem(ItemType.Herb, 1);
                        item = "<color=\"green\">허브 1개</color>";
                        break;
                    case 2:
                        _player.GetComponent<Inventory>().AddItem(ItemType.Candy, 1);
                        item = "<color=\"green\">사탕 1개</color>";
                        break;
                    case 3:
                        _player.GetComponent<Inventory>().AddItem(ItemType.Scroll, 1);
                        item = "<color=\"green\">부활스크롤 1개</color>";
                        break;
                    case 4:
                        _player.GetComponent<Inventory>().AddItem(ItemType.Coin, 1);
                        item = "<color=\"green\">코인 1개</color>";
                        break;
                }
                GameManager.Instance.ShowMessage(item + " 획득!");
                break;
            case 3:
                Debug.Log("특별보너스!");
                CharacterManager.Instance._clearedRoom = true;
                string items = "";
                for (int i = 0; i < 3; i++)
                {
                    switch (Random.Range(0, 5))
                    {
                        case 0:
                            _player.GetComponent<Inventory>().AddItem(ItemType.Potion, 1);
                            items += "<color=\"green\">포션 1개</color>/";
                            break;
                        case 1:
                            _player.GetComponent<Inventory>().AddItem(ItemType.Herb, 1);
                            items += "<color=\"green\">허브 1개</color>/";
                            break;
                        case 2:
                            _player.GetComponent<Inventory>().AddItem(ItemType.Candy, 1);
                            items += "<color=\"green\">사탕 1개</color>/";
                            break;
                        case 3:
                            _player.GetComponent<Inventory>().AddItem(ItemType.Scroll, 1);
                            items += "<color=\"green\">부활스크롤 1개</color>/";
                            break;
                        case 4:
                            _player.GetComponent<Inventory>().AddItem(ItemType.Coin, 1);
                            items += "<color=\"green\">코인 1개</color>/";
                            break;
                    }
                    GameManager.Instance.ShowMessage(items + " 획득!");
                }
                break;
            default:
                Debug.Log("미믹!");
                mimic.GetComponent<Player>().enabled = true;
                List<Player> enemy = new List<Player>();
                enemy.Add(mimic.GetComponent<Player>());
                CharacterManager.Instance.UpdateEnemy(enemy);
                _player.GetComponent<Player>()._isTurn = false;

                StartCoroutine(DelayBattle());
                break;
        }
    }
    private IEnumerator DelayBattle()
    {
        yield return new WaitForSeconds(1f);
        TurnManager.Instance.GetComponent<TurnController>()._startBattle = true;
    }
    public void SkipRoom()
    {
        CharacterManager.Instance._clearedRoom = true;
        TurnManager.Instance.GetComponent<TurnController>()._whileRoot = false;
        GameObject.Find("CoinCanvas").transform.GetChild(0).gameObject.SetActive(false);
    }
}
