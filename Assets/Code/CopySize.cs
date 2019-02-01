using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySize : MonoBehaviour
{
  public SpriteRenderer copy;
  public SpriteRenderer self;
  // Start is called before the first frame update
  private void Awake()
  {
    self = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
    if (copy)
    {
      self.size = copy.size;
    }

  }
}
