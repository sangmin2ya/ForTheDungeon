using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "지하 " + StageManager.Instance.CurrentStage + "층\nRoom " + (StageManager.Instance.CurrentRoom + 1);
    }
}
