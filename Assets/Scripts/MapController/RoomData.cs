using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public Vector3[] _playerPos = new Vector3[2];
    public Vector3 _enemyPos1;
    public Vector3 _enemyPos2;
    public Vector3 _doorPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
    }
    private void UpdatePos()
    {
        int stage = StageManager.Instance.CurrentStage % 5;
        _playerPos[0] = new Vector3(-31 * stage, -3, 0);
        _playerPos[1] = new Vector3(-31 * stage, 3, 0);
        _enemyPos1 = new Vector3(-31 * stage - 9, -3, 0);
        _enemyPos2 = new Vector3(-31 * stage - 9, -3, 0);
        _doorPos = new Vector3(-31 * stage - 19, 6, 0);
    }
}
