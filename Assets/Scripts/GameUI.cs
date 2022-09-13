using System;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI foodText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateUnitCountText(int value)
    {
        unitCountText.text = value.ToString();
    }

    public void UpdateFoodText(int value)
    {
        foodText.text = value.ToString();
    }
}
