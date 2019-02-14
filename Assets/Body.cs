using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Body : MonoBehaviour
{
  [SerializeField]
  FloatValue BodyAliveRatio;

  [SerializeField]
  FloatValue TimeAlive;

  [SerializeField]
  FloatValue TotalInitialOxygen;

  void Start()
  {
    TimeAlive.Value = 0;
    var bodyRectangles = GetComponentsInChildren<BodyRectangle>().ToList();
    var totalArea = bodyRectangles.Sum(r => r.Area);
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
    var bodyRectangles = GetComponentsInChildren<OxigenationMeter>().ToList();
    var totalOxygen = bodyRectangles.Sum((b) => b.MaxOxygenLevel);
    var currentOxygen = bodyRectangles.Sum((b) => b.OxygenLevel);
    BodyAliveRatio.Value = currentOxygen / totalOxygen;
  }
}
