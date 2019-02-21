using UnityEngine;

public class OxigenationMeter : MonoBehaviour
{
  public Gradient gradient;

  [SerializeField]
  private float oxygenLevel;

  public FloatValue oxygenMultiplier;

  public float OxygenLevel
  {
    get
    {
      return oxygenLevel;
    }
    set
    {
      oxygenLevel = Mathf.Clamp(value, -0.01f, MaxOxygenLevel);
      oxygenRatio = oxygenLevel / MaxOxygenLevel;
    }
  }


  public float OxygenRatio
  {
    get
    {
      return oxygenRatio;
    }

    set
    {
      oxygenRatio = value;
      oxygenLevel = value * MaxOxygenLevel;
    }
  }

  [SerializeField]
  private float oxygenRatio;

  public float view;
  public float MaxOxygenLevel => InternalMaxOxygenLevel * oxygenMultiplier.Value;

  [SerializeField]
  private float InternalMaxOxygenLevel;

  float oxygenDecreaseSpeed => OxygenDecreaseSpeed?.Value ?? 0.0f;
  public FloatValue OxygenDecreaseSpeed;

  public bool IsAlive { get; private set; } = true;

  public void SetOxygenData(float O2LevelRatio, float maxO2Level)
  {
    InternalMaxOxygenLevel = maxO2Level;
    OxygenRatio = O2LevelRatio;
  }

  public Color GetColor()
  {
    return this.gradient.Evaluate(OxygenRatio);
  }
  public float Take(float amount)
  {
    var expectedOxygen = OxygenLevel - amount;
    var removed = Mathf.Min(amount, OxygenLevel);
    OxygenLevel = Mathf.Max(expectedOxygen, 0);
    return removed;
  }

  public float Give(float amount)
  {
    var expectedOxygen = OxygenLevel + amount;
    OxygenLevel = Mathf.Min(expectedOxygen, MaxOxygenLevel);
    var overflow = Mathf.Max(0, expectedOxygen - OxygenLevel);
    return overflow;
  }

  private void Start()
  {
    IsAlive = true;
  }

  private void Update()
  {
    this.OxygenLevel -= Time.deltaTime * this.oxygenDecreaseSpeed;

    if (this.OxygenLevel <= 0f)
    {
      this.IsAlive = false;
      //print("Body Rectangle is dead!");
    }

    view = MaxOxygenLevel;
  }
}