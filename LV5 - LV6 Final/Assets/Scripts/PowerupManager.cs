using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour
{
    public enum PowerupType { HPUp, DamageUp, FireRateUp, ProjectileSpeedUp, DualLaser, TripleLaser }

    [Header("Powerup Panel")]
    [SerializeField] GameObject powerupPanel;
    [SerializeField] TextMeshProUGUI headerText;

    [Header("Option 1")]
    [SerializeField] Button powerupButton1;
    [SerializeField] TextMeshProUGUI powerupName1;
    [SerializeField] TextMeshProUGUI powerupDesc1;

    [Header("Option 2")]
    [SerializeField] Button powerupButton2;
    [SerializeField] TextMeshProUGUI powerupName2;
    [SerializeField] TextMeshProUGUI powerupDesc2;

    private PowerupType option1;
    private PowerupType option2;

    void Start()
    {
        powerupPanel.SetActive(false);
        GameManager.Instance.OnWaveComplete += ShowPowerupSelection;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnWaveComplete -= ShowPowerupSelection;
    }

    private void ShowPowerupSelection(int wave)
    {
        // Pick 2 distinct random powerups
        var pool = new List<PowerupType>((PowerupType[])System.Enum.GetValues(typeof(PowerupType)));

        int i1 = Random.Range(0, pool.Count);
        option1 = pool[i1];
        pool.RemoveAt(i1);

        int i2 = Random.Range(0, pool.Count);
        option2 = pool[i2];

        if (headerText != null) headerText.text = $"Wave {wave} Complete!\nChoose a Powerup:";

        SetOption(powerupName1, powerupDesc1, option1);
        SetOption(powerupName2, powerupDesc2, option2);

        powerupButton1.onClick.RemoveAllListeners();
        powerupButton2.onClick.RemoveAllListeners();
        powerupButton1.onClick.AddListener(() => Choose(option1));
        powerupButton2.onClick.AddListener(() => Choose(option2));

        powerupPanel.SetActive(true);
    }

    private void Choose(PowerupType type)
    {
        // Deselect the button so it doesn't stay in "pressed" state next time the panel opens
        EventSystem.current.SetSelectedGameObject(null);

        Player player = FindFirstObjectByType<Player>();
        if (player != null)
            player.ApplyPowerup(type);

        powerupPanel.SetActive(false);
        GameManager.Instance.PowerupSelected();
    }

    private void SetOption(TextMeshProUGUI nameLabel, TextMeshProUGUI descLabel, PowerupType type)
    {
        if (nameLabel != null) nameLabel.text = GetName(type);
        if (descLabel != null) descLabel.text  = GetDesc(type);
    }

    private string GetName(PowerupType type) => type switch
    {
        PowerupType.HPUp              => "HP Up",
        PowerupType.DamageUp          => "Damage Up",
        PowerupType.FireRateUp        => "Fire Rate Up",
        PowerupType.ProjectileSpeedUp => "Speed Up",
        PowerupType.DualLaser         => "Dual Laser",
        PowerupType.TripleLaser       => "Triple Laser",
        _                             => type.ToString()
    };

    private string GetDesc(PowerupType type) => type switch
    {
        PowerupType.HPUp              => "+100 Max HP",
        PowerupType.DamageUp          => "+50 Damage per shot",
        PowerupType.FireRateUp        => "Shoot 30% faster",
        PowerupType.ProjectileSpeedUp => "Bullets travel faster",
        PowerupType.DualLaser         => "Fire two lasers at once",
        PowerupType.TripleLaser       => "Fire three lasers at once",
        _                             => ""
    };
}
