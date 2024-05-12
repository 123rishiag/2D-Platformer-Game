using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    #region Variables
    // Reference to the TextMeshProUGUI component which displays the score on the UI.
    private TextMeshProUGUI scoreText;

    // Variable to keep track of the player's score.
    private int score = 0;
    #endregion

    #region Unity Lifecycle Methods
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialize scoreText by getting the TextMeshProUGUI component attached to the same GameObject.
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        // Update the UI to show the initial score when the game starts.
        RefreshUI();
    }
    #endregion

    #region Score Management
    // Public method to increase the score by a specific increment.
    public void IncreaseScore(int incrementScore)
    {
        // Add the increment to the current score.
        score += incrementScore;

        // Update the score display after changing the score.
        RefreshUI();
    }

    // Private method to update the score display in the UI.
    private void RefreshUI()
    {
        // Set the scoreText to display the current score prefixed with "Score: ".
        scoreText.text = "Score: " + score;
    }
    #endregion
}
