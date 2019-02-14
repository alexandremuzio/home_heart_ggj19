using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Body : MonoBehaviour
{
  [SerializeField]
  FloatValue BodyOxygenRatio;

  [SerializeField]
  FloatValue BodyAliveRatio;

  [SerializeField]
  FloatValue TimeAlive;

  [SerializeField]
  FloatValue TotalInitialOxygen;

  float totalArea;

  void Start()
  {
    TimeAlive.Value = 0;
    var bodyRectangles = GetComponentsInChildren<BodyRectangle>().ToList();
    totalArea = bodyRectangles.Sum(r => r.Area);
    bodyRectangles.ForEach(r =>
    {
      var o2 = r.Area / totalArea * TotalInitialOxygen.Value;
      r.oximeter.SetOxygenData(o2, o2);
    });
  }

  // Update is called once per frame
  void Update()
  {
    TimeAlive.Value += Time.deltaTime;

    // Calculate body ratio
    var bodyRectangles = GetComponentsInChildren<BodyRectangle>().ToList();
    var rectanglesOxygen = bodyRectangles.Select(b => b.oximeter);
    var totalOxygen = rectanglesOxygen.Sum((b) => b.MaxOxygenLevel);
    var currentOxygen = rectanglesOxygen.Sum((b) => b.OxygenLevel);
    BodyOxygenRatio.Value = currentOxygen / totalOxygen;

    var aliveArea = bodyRectangles.Where(b => b.oximeter.IsAlive).Sum(b => b.Area);
    BodyAliveRatio.Value = aliveArea / totalArea;
  }
}
