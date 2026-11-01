using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsUIManager : MonoBehaviour
{
    public static HeartsUIManager Instance;

    [SerializeField] private Image heartPrefab; // Prefab for a single heart image
    [SerializeField] private Transform heartsContainer; // Parent container for hearts
    [SerializeField] private Color fullHeartColor = Color.white;
    [SerializeField] private Color emptyHeartColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private List<Image> hearts = new List<Image>();
    private int maxHearts = 3;
    private int currentHearts = 3;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeHearts(maxHearts);
        UpdateHeartsDisplay();
    }

    /// <summary>
    /// Initialize the hearts UI with a specified amount of hearts.
    /// </summary>
    public void InitializeHearts(int heartCount)
    {
        maxHearts = heartCount;
        currentHearts = heartCount;

        // Clear existing hearts
        foreach (Image heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();

        // Create new hearts
        for (int i = 0; i < heartCount; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartsContainer);
            hearts.Add(newHeart);
        }

        UpdateHeartsDisplay();
    }

    /// <summary>
    /// Reduce hearts by the specified amount and update the UI.
    /// </summary>
    public void LoseHeart(int amount = 1)
    {
        currentHearts = Mathf.Max(0, currentHearts - amount);
        UpdateHeartsDisplay();
    }

    /// <summary>
    /// Gain hearts and update the UI.
    /// </summary>
    public void GainHeart(int amount = 1)
    {
        currentHearts = Mathf.Min(maxHearts, currentHearts + amount);
        UpdateHeartsDisplay();
    }

    /// <summary>
    /// Set hearts to a specific amount and update the UI.
    /// </summary>
    public void SetHearts(int amount)
    {
        currentHearts = Mathf.Clamp(amount, 0, maxHearts);
        UpdateHeartsDisplay();
    }

    /// <summary>
    /// Update the visual display of hearts based on current heart count.
    /// </summary>
    private void UpdateHeartsDisplay()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].color = fullHeartColor;
            }
            else
            {
                hearts[i].color = emptyHeartColor;
            }
        }

        Debug.Log($"Hearts: {currentHearts}/{maxHearts}");
    }

    /// <summary>
    /// Get the current number of hearts.
    /// </summary>
    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    /// <summary>
    /// Get the maximum number of hearts.
    /// </summary>
    public int GetMaxHearts()
    {
        return maxHearts;
    }

    /// <summary>
    /// Check if the player is out of hearts.
    /// </summary>
    public bool IsOutOfHearts()
    {
        return currentHearts <= 0;
    }
}
