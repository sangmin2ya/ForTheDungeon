using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUIController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    // Start is called before the first frame update
    void Start()
    {
        _healthBar.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.transform.Find("HPCanvas").Find("Name").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Name;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        _healthBar.fillAmount = (float)gameObject.GetComponent<Player>().Character.CurrentHealth / gameObject.GetComponent<Player>().Character.Health;
    }
}
