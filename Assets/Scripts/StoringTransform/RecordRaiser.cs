using UnityEngine;

public class RecordRaiser : MonoBehaviour
{
    [SerializeField]
    private GeneralEvent _startRecording;

    [SerializeField]
    private GeneralEvent _stopRecording;

    private bool _isRecording = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_isRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }
    }

    private void StartRecording()
    {
        _isRecording = true;
        _startRecording.Raise();
        Debug.Log("Recording started");
    }

    private void StopRecording()
    {
        _isRecording = false;
        _stopRecording.Raise();
        Debug.Log("Recording stopped");
    }
}
