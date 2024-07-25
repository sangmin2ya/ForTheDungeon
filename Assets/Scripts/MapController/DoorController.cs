using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (StageManager.Instance.CurrentRoom >= 5)
            {
                Debug.Log("다음 층으로 이동");
                StageManager.Instance.NextStage();
                StartCoroutine(DelayNextRoom());
            }
            else
            {
                Debug.Log("다음 방 이동");
                StageManager.Instance.NextRoom();
                StartCoroutine(DelayNextRoom());
            }
        }
    }
    IEnumerator DelayNextRoom()
    {
        yield return new WaitForSeconds(1.0f);
        StageManager.Instance._enterDoor = true;
    }
}
