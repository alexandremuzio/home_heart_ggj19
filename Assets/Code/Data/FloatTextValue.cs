using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class FloatTextValue : MonoBehaviour
{
  [SerializeField]
  FloatValue value;

  Text textValue;

  public string Format;

  // Start is called before the first frame update
  void Start()
  {
    textValue = GetComponent<Text>();
  }

  // Update is called once per frame
  void Update()
  {
    textValue.text = value.Value.ToString(Format);
  }
}
