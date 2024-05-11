using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private TextMeshProUGUI scroreText;
    private int score = 0;

    private void Awake()
    {
        scroreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        RefreshUI();
    }

    public void IncreaseScore(int incrementScore)
    {
        score += incrementScore;
        RefreshUI();
    }

    private void RefreshUI()
    {
        scroreText.text = "Score: " + score;
    }
}
