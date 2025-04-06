using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private GameObject _outOfMindPanel;

    [SerializeField]
    private GameObject _dwellTooMuchPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _backgroundImage.enabled = false;
        _outOfMindPanel.SetActive(false);
        _dwellTooMuchPanel.SetActive(false);
    }


    public void OnOutOfMind()
    {
        _backgroundImage.enabled = true;
        _outOfMindPanel.SetActive(true);
    }

    public void OnDwellTooMuch()
    {
        _backgroundImage.enabled = true;
        _dwellTooMuchPanel.SetActive(true);
    }
}
