using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderAndSizeMatch : MonoBehaviour
{
  public BoxCollider2D collider2D;
  public SpriteRenderer renderer;
  // Start is called before the first frame update
  void Start()
  {
    renderer = GetComponent<SpriteRenderer>();
    collider2D = GetComponent<BoxCollider2D>();
  }

  // Update is called once per frame
  void Update()
  {
    collider2D.size = renderer.size;
  }
}
