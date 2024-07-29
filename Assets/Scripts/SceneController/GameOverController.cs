using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterManager.Instance._gameOver == true)
        {
            Invoke("GameOver", 2f);
        }
    }
    private void GameOver()
    {
        Debug.Log("게임오버");
        GameManager.Instance.SetGameState(false);
        SceneManager.LoadScene("GameOver");
    }
}
