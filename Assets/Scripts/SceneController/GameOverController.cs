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
        GameOver();
    }
    private void GameOver()
    {
        if (CharacterManager.Instance._gameOver == true)
        {
            Debug.Log("게임오버");
            SceneManager.LoadScene("GameOver");
        }
    }
}
