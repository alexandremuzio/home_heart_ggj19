
using UnityEngine;

[CreateAssetMenu(fileName = "BloodCell", menuName = "GameData/BloodCell", order = 1)]
public class BloodCellData : ScriptableObject
{
  [Header("Movement")]
  public float Speed;

  [Header("Recording")]
  public float RecordPositionInterval;

  [Header("Oxigenation")]
  public float MaxOxigen;
  public float OxigenDonationRate;
}