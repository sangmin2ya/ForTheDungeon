using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private GameObject _selectCanvas;
    [SerializeField] private GameObject _InfoCanvas1;
    [SerializeField] private GameObject _InfoCanvas2;
    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Reset();
        StageManager.Instance.Reset();
        TurnManager.Instance.Reset();
        _InfoCanvas1.SetActive(false);
        _InfoCanvas2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        GameObject.Find("NoneCam").SetActive(false);
        GameObject.Find("MainCanvas").SetActive(false);
        StartCoroutine(StartGameCoroutine());
    }
    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _selectCanvas.SetActive(true);
        _InfoCanvas1.SetActive(true);
        _InfoCanvas2.SetActive(true);
    }
}
