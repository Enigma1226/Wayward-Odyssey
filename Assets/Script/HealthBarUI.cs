using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text label;
    public int maxHP = 100;
    public int currentHP = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = maxHP;
        SetHealth(currentHP);
    }

    // Update is called once per frame
    public void SetHealth(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, maxHP);
        slider.value = currentHP;
        label.text = currentHP + "/" + maxHP;
    }
}
