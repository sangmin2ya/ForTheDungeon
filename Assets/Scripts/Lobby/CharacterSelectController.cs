using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _firstSelection = new List<GameObject>();
    [SerializeField] private List<GameObject> _secondSelection = new List<GameObject>();
    private Player[] _players = new Player[2];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 첫번째 캐릭터 선택
    /// </summary>
    public void ChanageFisrtCharacter()
    {
        if (_firstSelection[0].activeSelf)
        {
            _firstSelection[0].SetActive(false);
            _firstSelection[1].SetActive(true);
        }
        else
        {
            _firstSelection[0].SetActive(true);
            _firstSelection[1].SetActive(false);
        }
            
    }
    /// <summary>
    /// 두번째 캐릭터 선택
    /// </summary>
    public void ChangeSecondCharacter()
    {
        if (_secondSelection[0].activeSelf)
        {
            _secondSelection[0].SetActive(false);
            _secondSelection[1].SetActive(true);
        }
        else
        {
            _secondSelection[0].SetActive(true);
            _secondSelection[1].SetActive(false);
        }
    }
    /// <summary>
    /// 파티 구성완료
    /// </summary>
    public void SelectCharacter()
    {
        _players[0] = _firstSelection[0].activeSelf ? _firstSelection[0].GetComponent<Player>() : _firstSelection[1].GetComponent<Player>();
        _players[1] = _secondSelection[0].activeSelf ? _secondSelection[0].GetComponent<Player>() : _secondSelection[1].GetComponent<Player>();

        foreach (Player player in _players)
        {
            CharacterManager.Instance.AddFirstCharacter(player);
            Debug.Log(player.Character.Name + " 초기 파티 추가됨");
        }
        SceneManager.LoadScene("Game");
    }
}
