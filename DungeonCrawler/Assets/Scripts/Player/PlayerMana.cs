using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMana : MonoBehaviour
{
    private float maxMana = 100f;
    public float MaxMana { get { return maxMana; }}
    private float currentMana;
    public float CurrentMana { get { return currentMana; } }

    [SerializeField]
    private Image manaBar;
    [SerializeField]
    private TextMeshProUGUI manaText;

    private void Awake()
    {
        currentMana = maxMana;
        StartCoroutine(RegenMana());
    }

    /// <summary>
    /// Removes the inputted amount of mana from the player's mana pool
    /// </summary>
    /// <param name="amount">Amount of mana to remove</param>
    /// <returns>Returns true if player has enough mana, false if not</returns>
    public bool RemoveMana(float amount)
    {
        if (Mathf.Abs(amount) > currentMana) { Debug.Log("Not enough mana!"); return false; }

        currentMana -= Mathf.Abs(amount);
        return true;
    }

    public void AddMana(float amount)
    {
        currentMana += Mathf.Abs(amount);

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        float fillAmount = currentMana / maxMana;

        manaBar.fillAmount = fillAmount;
        manaText.SetText(Mathf.RoundToInt(currentMana).ToString());
    }

    private IEnumerator RegenMana()
    {
        while (true)
        {
            if (currentMana < maxMana)
            {
                currentMana += 0.05f;

                if (currentMana > maxMana)
                {
                    currentMana = maxMana;
                }

                UpdateDisplay();
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
