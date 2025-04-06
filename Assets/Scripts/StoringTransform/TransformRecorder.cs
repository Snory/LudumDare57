using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    private List<TransformRecordData> _transformDataList = new List<TransformRecordData>();
    private float _startTime;

    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private GeneralEvent _recordingTransformFinishedEvent;

    [SerializeField]
    private Transform _transformToRecord;

    [SerializeField]
    private bool _startRecordingOnStart = false; // New boolean to control automatic start

    private bool _isRecording = false;

    private void Start()
    {
        // Start recording when the game starts if the boolean is set to true
        if (_startRecordingOnStart)
        {
            OnStartRecording();
        }
    }

    private void FixedUpdate()
    {
        if (!_isRecording)
            return;

        // Store the current position, rotation, and states with timestamp
        _transformDataList.Add(new TransformRecordData
        {
            Position = _transformToRecord.position,
            Rotation = _transformToRecord.rotation,
            IsMoving = _playerController.IsMoving,
            IsDashing = _playerController.IsDashing,
            Timestamp = Time.time - _startTime
        });
    }

    public void OnStartRecording()
    {
        _startTime = Time.time;
        _isRecording = true;
    }

    public void OnStopRecording()
    {
        _isRecording = false;
        RaiseStoredTransforms();
        ClearRecordedTransforms();
    }

    private void ClearRecordedTransforms()
    {
        _transformDataList.Clear();
    }

    public List<TransformRecordData> GetTransformData()
    {
        return _transformDataList;
    }

    private void RaiseStoredTransforms()
    {
        _recordingTransformFinishedEvent.Raise(new StoredTransfromEventArgs(new TransformRecordDataGroup(new List<TransformRecordData>(_transformDataList))));
    }
}

public class TransformRecordData
{
    public Vector3 Position;
    public Quaternion Rotation;
    public bool IsMoving;
    public bool IsDashing;
    public float Timestamp;
}

public class TransformRecordDataGroup
{
    public List<TransformRecordData> TransformDataList;

    public TransformRecordDataGroup(List<TransformRecordData> transformDataList)
    {
        TransformDataList = transformDataList;
    }
}
