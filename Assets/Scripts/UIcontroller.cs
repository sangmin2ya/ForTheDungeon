using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInOut(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator FadeInOut(float f)
    {
        // Canvas에서 GoDown 이미지 오브젝트 찾기
        Image image = GameObject.Find("Canvas").transform.Find("GoDown").GetComponent<Image>();
        TextMeshProUGUI text = GameObject.Find("Canvas").transform.Find("GoDown").GetChild(0).GetComponent<TextMeshProUGUI>();
        if (image == null)
        {
            Debug.LogError("GoDown not found in Canvas.");
            yield break;
        }

        // 오브젝트 활성화
        image.gameObject.SetActive(true);

        // 투명도를 1로 유지 (1초 동안)
        yield return new WaitForSeconds(2.0f);

        // 투명도를 1에서 0으로 변화 (1초 동안)
        for (float t = 0; t <= f; t += Time.deltaTime)
        {
            Color color = image.color;
            Color textColor = text.color;
            color.a = Mathf.Lerp(1, 0, t / f);
            textColor.a = Mathf.Lerp(1, 0, t / f);
            image.color = color;
            text.color = textColor;

            yield return null;
        }
        // 오브젝트 비활성화
        image.gameObject.SetActive(false);
    }
}
