using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BloodCellRecorder))]
public class BloodCellMovement : MonoBehaviour
{
  private float PositionRecordInterval;
  public BloodCellData data;
  private BloodCellRecorder recorder;
  public CellMoveMode Mode { get; private set; }

  private IEnumerator ReplayRecording()
  {

    while (true)
    {
      foreach (var target in recorder.GetNextPosition())
      {
        for (float t = 0; t < data.RecordPositionInterval; t += Time.deltaTime)
        {
          Vector3 direction = (target - (Vector2)transform.position).normalized;
          transform.position += Time.deltaTime * data.Speed * direction;
          yield return null;
        }
      }
    }
  }

  private int currentRecordingIndex;
  internal bool leftHomeBase = false;

  void Start()
  {
    recorder = GetComponent<BloodCellRecorder>();
    StartCoroutine(ModeAnimation());
  }

  public void OnCellBack()
  {
    Mode = CellMoveMode.Replay;
    StartCoroutine(ReplayRecording());
    recorder.isRecording = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (Mode == CellMoveMode.Play)
    {
      var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
      transform.position += (Vector3)input * Time.deltaTime * data.Speed;
    }
  }

  IEnumerator ModeAnimation()
  {
    while (true)
    {
      while (Mode == CellMoveMode.Play)
        yield return null;

      print("stopping animation");
      GetComponent<Animator>().SetTrigger("toggle");

      while (Mode == CellMoveMode.Replay)
        yield return null;

      GetComponent<Animator>().SetTrigger("toggle");
    }
  }

  public void OnTriggerExit2D(Collider2D c)
  {
    var home = c.gameObject.GetComponent<BloodCellHome>();
    if (home)
    {
      leftHomeBase = true;

    }
  }
}
