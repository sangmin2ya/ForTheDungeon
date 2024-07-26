using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private GameObject _readyBtn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShowReadyBtn();
    }
    public void ReadyToMove()
    {
        Move();
    }
    private void Move()
    {
        float delay = 0f;
        List<Player> players = CharacterManager.Instance.players;
        foreach (var player in players)
        {
            StartCoroutine(MovePlayer(player, delay));
            delay += 1f;
        }
    }
    private void ShowReadyBtn()
    {
        if (CharacterManager.Instance._clearedRoom)
        {
            CharacterManager.Instance._clearedRoom = false;
            _readyBtn.SetActive(true);
        }
    }
    private IEnumerator MovePlayer(Player player, float delay)
    {
        _readyBtn.SetActive(false);
        yield return new WaitForSeconds(delay);
        Vector3 startPosition = player.transform.position;
        Vector3 targetPosition = StageManager.Instance._currentRoom.GetComponent<RoomData>()._doorPos;
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            player.gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.gameObject.transform.position = targetPosition; // 이동이 끝난 후 정확한 위치 설정
    }
}
