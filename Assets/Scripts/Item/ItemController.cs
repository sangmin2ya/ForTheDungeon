using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ItemController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item { get; private set; }

    public ItemType _itemType;
    [SerializeField] private TextMeshProUGUI tooltipText; // 설명 텍스트
    [SerializeField] private GameObject choicePanel;
    private Player _player;
    void Awake()
    {
        Item = new Item(_itemType, 0);
    }
    void Update()
    {
        UpdateIcon();
    }
    void Start()
    {
        tooltipText = transform.Find("Explain").GetChild(0).GetComponent<TextMeshProUGUI>();
        //transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = Item.itemType.ToString();
        _player = transform.parent.parent.parent.GetComponent<Player>();
        switch (_itemType)
        {
            case ItemType.Potion:
                tooltipText.text = "전체 체력의 30퍼센트를 회복시키는 포션.\n맛이 좋아서 모험가들에게 인기가 많다.";
                break;
            case ItemType.Herb:
                tooltipText.text = "영구적으로 특정 능력치를 2증가시키는 허브.\n던전이 주요 자생지이다.";
                break;
            case ItemType.Candy:
                tooltipText.text = "레벨을 즉시\n1 증가 시켜주는 사탕.\n이상한 사탕이다.";
                break;
            case ItemType.Scroll:
                tooltipText.text = "사망한 아군을 체력 1로 부활시키는 스크롤.\n던전 밖에서 매우 높은 가격에 거래된다.";
                break;
            case ItemType.Coin:
                tooltipText.text = "집중도를 모두 회복시키는 동전.\n왠지 운이 좋아지는 듯한 기분이다.";
                break;
        }
    }
    public void SetItem(Item item)
    {
        Item = item;
    }
    private void UpdateIcon()
    {
        transform.Find("ItemCount").GetComponent<TextMeshProUGUI>().text = Item.quantity.ToString();
    }
    // 마우스를 버튼에 올렸을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.transform.parent.gameObject.SetActive(true); // 텍스트 활성화
            tooltipText.gameObject.SetActive(true); // 텍스트 활성화
        }
    }

    // 마우스를 버튼에서 벗어날 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipText != null)
        {
            tooltipText.gameObject.SetActive(false); // 텍스트 비활성화
            tooltipText.transform.parent.gameObject.SetActive(false); // 텍스트 활성화
        }
    }
    public void UseItem()
    {
        if (this.Item.quantity > 0 && (_player._isTurn || CharacterManager.Instance._clearedRoom))
        {
            switch (_itemType)
            {
                case ItemType.Potion:
                    Debug.Log("포션 사용");
                    _player.GetComponent<Inventory>().ConsumeItem(ItemType.Potion, 1);
                    _player.Heal((int)Math.Round(_player.Character.Health * 0.3f));
                    break;
                case ItemType.Herb:
                    Debug.Log("허브 사용");
                    _player.GetComponent<Inventory>().ConsumeItem(ItemType.Herb, 1);
                    ShowChoice();
                    // 능력치 영구 증가
                    break;
                case ItemType.Candy:
                    Debug.Log("사탕 사용");
                    _player.GetComponent<Inventory>().ConsumeItem(ItemType.Candy, 1);
                    _player.LevelUp();
                    // 레벨업
                    break;
                case ItemType.Scroll:
                    Debug.Log("스크롤 사용");
                    if (CharacterManager.Instance._deadPlayer == null)
                    {
                        GameManager.Instance.ShowMessage("사망한 아군이 없습니다.");
                        return;
                    }
                    else
                        _player.GetComponent<Inventory>().ConsumeItem(ItemType.Scroll, 1);
                    // 아군 부활
                    if (CharacterManager.Instance._deadPlayer != null)
                        CharacterManager.Instance.ReviveCharacter(CharacterManager.Instance._deadPlayer);
                    break;
                case ItemType.Coin:
                    Debug.Log("동전 사용");
                    _player.GetComponent<Inventory>().ConsumeItem(ItemType.Coin, 1);
                    _player.Character.GainFocus();
                    // 집중도 회복
                    break;
            }
        }
        else if(!(_player._isTurn || CharacterManager.Instance._clearedRoom))
        {
            GameManager.Instance.ShowMessage("자신의 턴과 휴식때만 아이템을 사용할 수\n있습니다.");
            Debug.Log("캐릭터의 턴 혹은 준비시간에만 사용할 수 있습니다.");
        }
        else{
            GameManager.Instance.ShowMessage("해당 아이템을 보유하고 있지 않습니다.");
            
        }
    }
    private void ShowChoice()
    {
        // 선택지 표시
        choicePanel.SetActive(true);

    }
    public void IncreaseStrength()
    {
        choicePanel.SetActive(false);
        _player.IncreaseStat(StatType.Strength, 2);
    }
    public void IncreaseVitality()
    {
        choicePanel.SetActive(false);
        _player.IncreaseStat(StatType.Vitality, 2);
    }
    public void IncreaseIntelligence()
    {
        choicePanel.SetActive(false);
        _player.IncreaseStat(StatType.Intelligence, 2);
    }
    public void IncreaseVision()
    {
        choicePanel.SetActive(false);
        _player.IncreaseStat(StatType.Vision, 2);
    }
    public void IncreaseSpeed()
    {
        choicePanel.SetActive(false);
        _player.IncreaseStat(StatType.Speed, 2);
    }
}