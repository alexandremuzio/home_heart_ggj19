using UnityEngine;

public class OxigenationMeter : MonoBehaviour
{

  public Gradient gradient;

  public float oxygenLevel;

  public float maxOxygenLevel;

  public float oxygenDecreaseSpeed;

  public bool IsAlive { get; private set; }= true;

  public void SetOxygenData(float O2Level, float maxO2Level, float O2DecreaseSpeed = -1f)
  {
    oxygenLevel = O2Level;
    maxOxygenLevel = maxO2Level;

    if (O2DecreaseSpeed != -1f)
    {
      oxygenDecreaseSpeed = O2DecreaseSpeed;
    }
  }

  public Color GetColor()
  {
    return this.gradient.Evaluate(oxygenLevel / maxOxygenLevel);
  }
  public float Take(float amount)
  {
    var expectedOxygen = oxygenLevel - amount;
    var removed = Mathf.Min(amount, oxygenLevel);
    oxygenLevel = Mathf.Max(expectedOxygen, 0);
    return removed;
  }

  public float Give(float amount)
  {
    var expectedOxygen = oxygenLevel + amount;
    oxygenLevel = Mathf.Min(expectedOxygen, maxOxygenLevel);
    var overflow = Mathf.Max(0, expectedOxygen - oxygenLevel);
    return overflow;
  }

  private void Start()
  {
    IsAlive = true;
  }

  private void Update()
  {
    this.oxygenLevel -= Time.deltaTime * this.oxygenDecreaseSpeed;

    if (this.oxygenLevel < 0f)
    {
      this.IsAlive = false;
      //print("Body Rectangle is dead!");
    }
  }
}