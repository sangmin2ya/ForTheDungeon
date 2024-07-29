using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _firstSelection = new List<GameObject>();
    [SerializeField] private List<GameObject> _secondSelection = new List<GameObject>();
    private int firstIndex = 0;
    private int secondIndex = 0;
    private Player[] _players = new Player[2];
    // Start is called before the first frame update
    void Start()
    {
        //ApplyCharacter();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 첫번째 캐릭터 선택
    /// </summary>
    public void ChanageFisrtCharacterRight()
    {
        // 현재 오브젝트 비활성화
        _firstSelection[firstIndex].SetActive(false);
        // 인덱스 증가, 마지막 인덱스를 넘으면 처음으로
        firstIndex = (firstIndex + 1) % _firstSelection.Count;
        // 새로운 오브젝트 활성화
        _firstSelection[firstIndex].SetActive(true);
        //ApplyCharacter();
    }
    public void ChanageFisrtCharacterLeft()
    {
        // 현재 오브젝트 비활성화
        _firstSelection[firstIndex].SetActive(false);
        // 인덱스 증가, 마지막 인덱스를 넘으면 처음으로
        firstIndex = (firstIndex - 1 + _firstSelection.Count) % _firstSelection.Count;
        // 새로운 오브젝트 활성화
        _firstSelection[firstIndex].SetActive(true);
        //ApplyCharacter();
    }
    /// <summary>
    /// 두번째 캐릭터 선택
    /// </summary>
    public void ChanageSecondCharacterRight()
    {
        // 현재 오브젝트 비활성화
        _secondSelection[secondIndex].SetActive(false);
        // 인덱스 증가, 마지막 인덱스를 넘으면 처음으로
        secondIndex = (secondIndex + 1) % _secondSelection.Count;
        // 새로운 오브젝트 활성화
        _secondSelection[secondIndex].SetActive(true);
        //ApplyCharacter();
    }
    public void ChanageSecondCharacterLeft()
    {
        // 현재 오브젝트 비활성화
        _secondSelection[secondIndex].SetActive(false);
        // 인덱스 증가, 마지막 인덱스를 넘으면 처음으로
        secondIndex = (secondIndex - 1 + _secondSelection.Count) % _secondSelection.Count;
        // 새로운 오브젝트 활성화
        _secondSelection[secondIndex].SetActive(true);
        //ApplyCharacter();
    }
    /// <summary>
    /// 파티 구성완료
    /// </summary>
    public void SelectCharacter()
    {
        ApplyCharacter();
        SceneManager.LoadScene("Game");
    }
    private void ApplyCharacter()
    {
        foreach (GameObject obj in _firstSelection)
        {
            if (obj.activeSelf)
            {
                _players[0] = obj.GetComponent<Player>();
            }
        }
        foreach (GameObject obj in _secondSelection)
        {
            if (obj.activeSelf)
            {
                _players[1] = obj.GetComponent<Player>();
            }
        }
        CharacterManager.Instance.ClearCharacter();
        foreach (Player player in _players)
        {
            CharacterManager.Instance.AddFirstCharacter(player);
            Debug.Log(player.Character.Name + " 초기 파티 추가됨");
        }
    }
}
