using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _stagePrefab;
    public static MapController Instance { get; private set; }
    private Dictionary<RoomType, float> stageTypeDistribution = new Dictionary<RoomType, float>()
    {
        { RoomType.Battle, 0.55f },  // 50% 전투
        { RoomType.Trap, 0.15f },    // 20% 함정
        { RoomType.Recover, 0.15f }, // 20% 회복
        { RoomType.Reward, 0.15f }   // 15% 보상
    }; 
    // Start is called before the first frame update
    void Start()
    {
        StageManager.Instance._currentRoom = Instantiate(GetStagePrefab(GetRandomStageType()), new Vector3(0, 0, 0), Quaternion.identity);
        SettingRoom();
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
            StageManager.Instance._currentRoom = Instantiate(GetStagePrefab(GetRandomStageType()), new Vector3(-31 * (StageManager.Instance.CurrentRoom % 6), 0, 0), Quaternion.identity);
            for (int i = 0; i < CharacterManager.Instance.players.Count; i++)
            {
                if (CharacterManager.Instance.players[i] != null)
                {
                    CharacterManager.Instance.players[i].gameObject.transform.position = StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[i];
                    Debug.Log(CharacterManager.Instance.players[i].Character.Name + " 캐릭터 " + StageManager.Instance._currentRoom.GetComponent<RoomData>()._playerPos[i] + "로 이동");
                }
            }
            SettingRoom();
        }
    }
    private void SettingRoom()
    {
        switch (StageManager.Instance._currentRoom.GetComponent<RoomData>()._roomType)
        {
            case RoomType.Battle:
                GameObject.Find("CharacterSpawner").GetComponent<CharacterSpawnController>()._isBattleRoom = true;
                break;
            case RoomType.Trap:
                TurnManager.Instance.GetComponent<TurnController>()._startTrap = true;
                break;
            case RoomType.Recover:
                TurnManager.Instance.GetComponent<TurnController>()._startRecover = true;
                break;
            case RoomType.Reward:
                TurnManager.Instance.GetComponent<TurnController>()._startRoot = true;
                break;
            case RoomType.Stair:
                //TurnManager.Instance.GetComponent<TurnController>(). = true;
                break;
        }
    }
    IEnumerator DestroyRoom(GameObject prevRoom)
    {
        yield return new WaitForSeconds(2.0f);
        prevRoom.transform.Find("PlayerCamera").gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Destroy(prevRoom);
    }
    /// <summary>
    /// 랜덤 스테이지 반환 (가중치 on)
    /// </summary>
    /// <returns></returns>
    private RoomType GetRandomStageType()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        foreach (var entry in stageTypeDistribution)
        {
            cumulativeProbability += entry.Value;
            if (randomValue <= cumulativeProbability)
            {
                return entry.Key;
            }
        }

        return RoomType.Battle; // 기본값으로 전투를 반환
    }
    /// <summary>
    /// 스테이지 타입에 따른 프리팹 반환
    /// </summary>
    /// <param name="stageType"></param>
    /// <returns></returns>
    private GameObject GetStagePrefab(RoomType stageType)
    {
        switch (stageType)
        {
            case RoomType.Battle:
                return _stagePrefab[0];
            case RoomType.Trap:
                return _stagePrefab[1];
            case RoomType.Recover:
                return _stagePrefab[2];
            case RoomType.Reward:
                return _stagePrefab[3];
            default:
                return null;
        }
    }
}
