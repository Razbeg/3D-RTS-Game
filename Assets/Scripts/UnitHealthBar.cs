using System;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour
{
    public GameObject healthContainer;
    public RectTransform healthFill;

    private float maxSize;

    private void Awake()
    {
        maxSize = healthFill.sizeDelta.x;
        healthContainer.SetActive(false);
    }

    public void UpdateHealthBar(int curHp, int maxHp)
    {
        healthContainer.SetActive(true);
        float healthPercentage = (float)curHp / (float)maxHp;
        healthFill.sizeDelta = new Vector2(maxSize * healthPercentage, healthFill.sizeDelta.y);
    }
}
