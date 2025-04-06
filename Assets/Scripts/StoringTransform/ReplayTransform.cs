using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReplayTransform : MonoBehaviour
{
    private List<TransformRecordData> _transformDataList;

    [SerializeField]
    private float replayDelay = 1.0f; // Delay before starting the replay

    [SerializeField]
    private LineRenderer _lineRenderer;

    private bool _playerIsMoving;

    public void SetTransformData(List<TransformRecordData> transformDataList)
    {
        _transformDataList = transformDataList;
    }

    public void ReplayMovement()
    {
        DrawLine();
        StartCoroutine(ReplayCoroutine());
    }

    private IEnumerator ReplayCoroutine()
    {
        if (_transformDataList == null || _transformDataList.Count == 0)
        {
            yield break;
        }

        // Set the first position immediately
        transform.SetPositionAndRotation(_transformDataList[0].Position, _transformDataList[0].Rotation);

        // Add initial delay before starting the replay
        yield return new WaitForSeconds(replayDelay);

        float startTime = Time.time;
        float pausedTime = 0f;
        float maxWaitTime = 1.0f; // Maximum wait time threshold

        for (int i = 1; i < _transformDataList.Count; i++)
        {
            var data = _transformDataList[i];
            float waitTime = data.Timestamp - (Time.time - startTime - pausedTime);
            if (waitTime > maxWaitTime)
            {
                waitTime = maxWaitTime;
            }
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            while (!_playerIsMoving)
            {
                float pauseStartTime = Time.time;
                yield return null;
                pausedTime += Time.time - pauseStartTime;
            }

            transform.SetPositionAndRotation(data.Position, data.Rotation);

            // Remove the passed points from the LineRenderer
            _lineRenderer.positionCount = _transformDataList.Count - i;
            _lineRenderer.SetPositions(_transformDataList.Skip(i).Select(s => s.Position).ToArray());

            // Optionally, you can use the state data (IsMoving, IsDashing, IsJumping) here
            // to trigger animations or other behaviors if needed.
        }

        Destroy();
    }

    private void DrawLine()
    {
        if (_transformDataList == null || _transformDataList.Count < 2)
        {
            return;
        }

        _lineRenderer.positionCount = _transformDataList.Count;
        _lineRenderer.SetPositions(_transformDataList.Select(s => s.Position).ToArray());
    }

    public void OnPlayerMoving()
    {
        _playerIsMoving = true;
    }

    public void OnPlayerStoppedMoving()
    {
        _playerIsMoving = false;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
