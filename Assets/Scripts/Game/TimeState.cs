using UnityEngine;
using TMPro;  // Import the TextMesh Pro namespace

// This script is a conceptual implementation for tracking and displaying various game stats.
// It's an initial idea and will be further discussed and refined if deemed necessary.
// This is located in the UI objects folder and is in a subfolder called UI Text.
// Again this is just a concept at this point.

public class TimeState : MonoBehaviour
{
    public int Health { get; private set; } = 50;
    public int Wealth { get; private set; } = 50;
    public int Happiness { get; private set; } = 50; 
    public int Year { get; private set; } = 2024;

    public TMP_Text healthText;
    public TMP_Text wealthText;
    public TMP_Text happinessText;
    public TMP_Text yearText;

    public void ChangeHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, 100); // do we want max as 100?
        UpdateUI();
    }

    public void ChangeWealth(int amount)
    {
        Wealth = Mathf.Clamp(Wealth + amount, 0, 100); // do we want max as 100?
        UpdateUI();
    }

    public void ChangeHappiness(int amount)
    {
        Happiness = Mathf.Clamp(Happiness + amount, 0, 100); // do we want max as 100?
        UpdateUI();
    }

    public void ChangeYear(int amount)
    {
        Year += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthText.text = $"Health: {Health}";
        wealthText.text = $"Wealth: {Wealth}";
        happinessText.text = $"Happiness: {Happiness}";
        yearText.text = $"Year: {Year}";
    }
}
