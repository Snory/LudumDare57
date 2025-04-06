using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedTransformBank : MonoBehaviour
{
    [SerializeField]
    private GameObject _replayPrefab;

    [SerializeField]
    private Transform _playerTransform;

    private List<TransformRecordDataGroup> _storedTransformGroups = new List<TransformRecordDataGroup>();

    [SerializeField]
    private float _dangerRadius = 5.0f; // Example danger radius

    public void OnRecordingTransformFinished(EventArgs args)
    {
        StoredTransfromEventArgs storedTransformEventArgs = (StoredTransfromEventArgs)args;
        _storedTransformGroups.Add(storedTransformEventArgs.StoredTransformData);
        Debug.Log("Transform data stored");
    }

    [ContextMenu("Replay Character")]
    public void ReplayCharacters()
    {
        foreach (var transformDataList in _storedTransformGroups)
        {
            if (transformDataList.TransformDataList == null || transformDataList.TransformDataList.Count == 0)
            {
                Debug.LogError("Transform data list is empty or null.");
                continue;
            }

            Vector3 firstPosition = transformDataList.TransformDataList[0].Position;
            StartCoroutine(WaitUntilPlayerIsSafe(firstPosition, transformDataList.TransformDataList));
        }
    }

    private IEnumerator WaitUntilPlayerIsSafe(Vector3 firstPosition, List<TransformRecordData> transformDataList)
    {
        while (IsPlayerInDangerRadius(firstPosition))
        {
            yield return new WaitForSeconds(1.0f); // Check every second
        }

        GameObject replayObject = Instantiate(_replayPrefab);
        ReplayTransform replayTransform = replayObject.GetComponent<ReplayTransform>();
        replayTransform.SetTransformData(transformDataList);
        replayTransform.ReplayMovement();
    }

    private bool IsPlayerInDangerRadius(Vector3 position)
    {
        Vector3 playerPosition = GetPlayerPosition(); // Implement this method to get the player's current position
        return Vector3.Distance(playerPosition, position) < _dangerRadius;
    }

    private Vector3 GetPlayerPosition()
    {
        return _playerTransform.position;
    }
}
