using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject _stagePrefab;
    public static MapController Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        StageManager.Instance._currentRoom = Instantiate(_stagePrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        CreateRoom();
    }
    /// <summary>
    /// 새로운 방을 생성하고 카메라 전환, 1초뒤 이전 방 삭제
    /// </summary>
    private void CreateRoom()
    {
        if (StageManager.Instance._enterDoor)
        {
            StageManager.Instance._enterDoor = false;
            StartCoroutine(DestroyRoom(StageManager.Instance._currentRoom));
            StageManager.Instance._currentRoom = Instantiate(_stagePrefab, new Vector3(-31 * StageManager.Instance.CurrentRoom, 0, 0), Quaternion.identity);
            for(int i = 0; i < CharacterManager.Instance.players.Count; i++)
            {
                CharacterManager.Instance.players[i].gameObject.transform.position = StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[i];
            }
        }
    }
    IEnumerator DestroyRoom(GameObject prevRoom)
    {
        yield return new WaitForSeconds(1.0f);
        prevRoom.transform.Find("PlayerCamera").gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Destroy(prevRoom);
    }
}
