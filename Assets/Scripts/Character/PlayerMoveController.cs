using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReadyToMove()
    {
        Move();
    }
    private void Move()
    {
        List<Player> players = CharacterManager.Instance.players;
        foreach (var player in players)
        {
            player.gameObject.transform.position = Vector3.MoveTowards(player.transform.position, StageManager.Instance._currentRoom.GetComponent<RoomData>()._doorPos, 2);
        }
    }
}
