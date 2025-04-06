using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField]
    private GeneralEvent _startRecording, _stopRecording;

    public UnityEvent Destroyed;

    private void Awake()
    {
        if (_startRecording == null)
        {
            Debug.LogError("Start Recording event is not assigned.");
        }
        if (_stopRecording == null)
        {
            Debug.LogError("Stop Recording event is not assigned.");
        }

        _startRecording.Raise();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            Debug.Log("Player entered the door trigger.");
            _stopRecording.Raise();
            Destroyed?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
