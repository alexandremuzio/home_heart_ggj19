using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleValue : MonoBehaviour
{
  [SerializeField]
  private BoolValue Value;
  private Toggle toggle;
  // Start is called before the first frame update
  void Start()
  {
    toggle = GetComponent<Toggle>();
  }

  // Update is called once per frame
  void Update()
  {
    Value.Value = toggle.isOn;
  }
}
