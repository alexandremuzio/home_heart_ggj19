using UnityEngine;

public class OxigenationMeter : MonoBehaviour
{

    public Gradient gradient;

    public float oxygenLevel;

    public float maxOxygenLevel;

    public float oxygenDecreaseSpeed;

    public bool isAlive;

    public bool IsAlive() => isAlive;
    
    public Color GetColor()
    {
        return this.gradient.Evaluate(oxygenLevel / maxOxygenLevel);
    }

    private void Start()
    {
        isAlive = true;
    }

    private void Update()
    {
        this.oxygenLevel -= Time.deltaTime * this.oxygenDecreaseSpeed;

        if (this.oxygenLevel < 0f) {
            this.isAlive = false;
            print("Body Rectangle is dead!");
        }
    }
}