using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatUIController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _expBar;
    // Start is called before the first frame update
    void Start()
    {
        _healthBar.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        if (gameObject.transform.GetComponent<Player>()._characterType == CharacterType.Enemy) UpdateShieldBar();
        gameObject.transform.Find("HPCanvas").Find("Name").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Name;

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Find("HPCanvas").gameObject.SetActive(gameObject.GetComponent<Player>()._characterType == CharacterType.Player || TurnManager.Instance.GetComponent<TurnController>()._whileBattle);
        UpdateHealthBar();
        if (gameObject.transform.GetComponent<Player>()._characterType == CharacterType.Player)
            UpdateExpBar();
    }
    private void UpdateHealthBar()
    {
        _healthBar.fillAmount = (float)gameObject.GetComponent<Player>().Character.CurrentHealth / gameObject.GetComponent<Player>().Character.Health;
        _healthBar.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.CurrentHealth.ToString() + "/" + gameObject.GetComponent<Player>().Character.Health.ToString();
    }
    private void UpdateShieldBar()
    {
        GameObject go = gameObject.transform.Find("HPCanvas").Find("Shield").gameObject;
        go.GetComponent<Image>().color = gameObject.GetComponent<Player>()._shieldType == AttackType.Physical ? Color.blue : Color.magenta;
        go.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>()._shieldAmount.ToString();
    }
    private void UpdateExpBar()
    {
        _expBar.fillAmount = (float)gameObject.GetComponent<Player>().Character.Experience / gameObject.GetComponent<Player>().Character.ExperienceToNextLevel;
        _expBar.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Experience.ToString() + "/" + gameObject.GetComponent<Player>().Character.ExperienceToNextLevel.ToString();
    }
}
