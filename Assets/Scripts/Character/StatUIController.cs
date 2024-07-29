using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatUIController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _expBar;
    [SerializeField] private GameObject _InfoCanvas;
    [SerializeField] private GameObject _ItemCanvas;
    // Start is called before the first frame update
    void Start()
    {
        _healthBar.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        if (gameObject.transform.GetComponent<Player>()._characterType == CharacterType.Enemy) UpdateShieldBar();
        else
        {
            _InfoCanvas = gameObject.transform.Find("InfoCanvas").Find("Info").gameObject;
            _ItemCanvas = gameObject.transform.Find("InfoCanvas").Find("Item").gameObject;
        }
        gameObject.transform.Find("HPCanvas").Find("Name").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Name;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Find("HPCanvas").gameObject.SetActive(gameObject.GetComponent<Player>()._characterType == CharacterType.Player || TurnManager.Instance.GetComponent<TurnController>()._whileBattle);
        UpdateHealthBar();
        if (gameObject.transform.GetComponent<Player>()._characterType == CharacterType.Player)
        {
            UpdateExpBar();
            UpdateStat();
        }
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
    private void UpdateStat()
    {
        _InfoCanvas.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Name;
        _InfoCanvas.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "레벨 : " + gameObject.GetComponent<Player>().Character.Level;
        _InfoCanvas.transform.Find("Attack").GetComponent<TextMeshProUGUI>().text = "공격력 : " + ((gameObject.GetComponent<Player>()._attackType == AttackType.Physical) ? ("<color=\"blue\">(물리)" + gameObject.GetComponent<Player>().Character.PhysicalAttack) : ("<color=#FF00FF>(마법)" + gameObject.GetComponent<Player>().Character.MagicAttack));
        _InfoCanvas.transform.Find("Evasion").GetComponent<TextMeshProUGUI>().text = "회피확률 : " + gameObject.GetComponent<Player>().Character.Evasion;
        _InfoCanvas.transform.Find("Stat").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<Player>().Character.Attributes[StatType.Strength] + " | " + gameObject.GetComponent<Player>().Character.Attributes[StatType.Vitality] + " | " + gameObject.GetComponent<Player>().Character.Attributes[StatType.Intelligence] + " | " + gameObject.GetComponent<Player>().Character.Attributes[StatType.Speed] + " | " + gameObject.GetComponent<Player>().Character.Attributes[StatType.Vision];

        _InfoCanvas.transform.Find("Dead").gameObject.SetActive(gameObject.GetComponent<Player>().Character.CurrentHealth <= 0);

        for (int i = 0; i < 3; i++)
        {
            _InfoCanvas.transform.Find("Focus").GetChild(i + 3).gameObject.SetActive(false);
        }
        for (int i = 0; i < gameObject.GetComponent<Player>().Character.Focus; i++)
        {
            _InfoCanvas.transform.Find("Focus").GetChild(i + 3).gameObject.SetActive(true);
        }

        for (int i = 0; i < CharacterManager.Instance.players.Count; i++)
        {
            if (CharacterManager.Instance.players[i] != null && CharacterManager.Instance.players[i] == gameObject.GetComponent<Player>())
            {
                _InfoCanvas.transform.localPosition = new Vector3(-700 + (1400 * i), -400, 0);
                _ItemCanvas.transform.localPosition = new Vector3(-700 + (1400 * i), -270, 0);
                _ItemCanvas.transform.Find("Herb").Find("SelectStat").localPosition = new Vector3(700 - (1400 * i), 250, 0);
                _InfoCanvas.transform.Find("Focus").localPosition = new Vector3(230 - (460 * i), 0, 0);

            }
        }
    }
}
