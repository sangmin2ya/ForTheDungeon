using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;
    // Start is called before the first frame update
    void Start()
    {
        _score.text = "당신의 파티는 던전 " + StageManager.Instance.CurrentStage + "층에서 전멸했습니다..";
    }

    // Update is called once per frame
    void Update()
    {
        Restart();
    }
    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("재시작");
            CharacterManager.Instance.Reset();
            StageManager.Instance.Reset();
            TurnManager.Instance.Reset();
            SceneManager.LoadScene("Lobby");
        }
    }
}
