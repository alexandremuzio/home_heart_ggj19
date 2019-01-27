using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCellRecorder : MonoBehaviour
{
  [SerializeField]
  private BloodCellData data;
  private List<Vector2> PositionHistory = new List<Vector2>();
  private IEnumerator RecordPositionHistoryCoroutine()
  {
    while (true)
    {
      if (!isRecording) yield break;
      else
      {
        PositionHistory.Add(transform.position);
        yield return new WaitForSeconds(data.RecordPositionInterval);
      }
    }
  }

  int replayHead;
  public IEnumerable<Vector2> GetNextPosition()
  {
    foreach (var p in PositionHistory)
    {
      yield return p;
      replayHead = (replayHead + 1) % PositionHistory.Count;
    }
  }

  public bool isRecording = true;

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(RecordPositionHistoryCoroutine());
  }
}
