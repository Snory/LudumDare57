using TMPro;
using UnityEngine;

public class ScorePanelUI : MonoBehaviour
{
    private int _score;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        // Initialize the score to 0
        _score = 0;
        UpdateScore();
    }

    public void OnScoreAdd()
    {
        _score++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = $"Depth of mind: {_score}";
    }
}
