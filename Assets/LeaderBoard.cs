using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private int _currentScore;

    private void Awake()
    {
        // Initialize the score to 0
        _currentScore = 0;
    }

    public void OnAddScoreEvent()
    {
        _currentScore++;
    }
}
