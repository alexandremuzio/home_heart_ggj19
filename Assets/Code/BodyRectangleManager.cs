using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BodyRectangleManager : MonoBehaviour
{

    public float probabilityToSplitRectangle;

    public BodyRectangle bodyRectPrefab;

    public List<BodyRectangle> rectangles = new List<BodyRectangle>();

    private void Start() {
        // Create initial rectangles for body
        rectangles.Add(Instantiate(bodyRectPrefab, new Vector3(), Quaternion.identity));
    }

    private void Update() {
        float rdm = Random.Range(0f, 1f);

        if (rdm > probabilityToSplitRectangle)
        {
            // Split event
            // SplitRandomRectangle();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            SplitRandomRectangle();
        }
    }

    private void SplitRandomRectangle()
    {
        BodyRectangle r = rectangles
                    .OrderBy(_ => Guid.NewGuid())
                    .OrderByDescending((x1) => Mathf.Min(x1.VerticalSize, x1.HorizontalSize))
                    .First();

        // choose random point inside rectangle
        float randXMax = r.HorizontalSize / 2f + r.position.x;
        float randXMin = -r.HorizontalSize / 2f + r.position.x;
        float randYMax = r.VerticalSize / 2f + r.position.y;
        float randYMin = -r.VerticalSize / 2f + r.position.y;

        float x = Random.Range(randXMin + r.HorizontalSize / 7, randXMax - r.HorizontalSize / 7);
        float y = Random.Range(randYMin + r.VerticalSize / 7, randYMax - r.VerticalSize / 7);
        
        Array values = Enum.GetValues(typeof(BodySplitType));
        System.Random random = new System.Random();
        BodySplitType randomSplit = (BodySplitType)values.GetValue(random.Next(values.Length));
        SplitRectangle(r, new Vector2(x, y), randomSplit);
    }

    public void SplitRectangle(BodyRectangle r, Vector2 pos, BodySplitType splitType)
    {
        switch (splitType) 
        {
            case BodySplitType.Vertical:
                Vector2 newPos = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
                BodyRectangle Ob1 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity);
                Ob1.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), r.VerticalSize);
                Ob1.transform.position = newPos;
                rectangles.Add(Ob1);

                newPos = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, r.position.y);
                Ob1 = Instantiate(this.bodyRectPrefab, newPos, Quaternion.identity);
                Ob1.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), r.VerticalSize);
                Ob1.transform.position = newPos;
                rectangles.Add(Ob1);
                break;
            case BodySplitType.Horizontal:
                Vector2 newPosV = new Vector2(r.position.x,(r.position.y - r.VerticalSize / 2 + pos.y) / 2);
                BodyRectangle Ob1V = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity);
                Ob1V.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
                Ob1V.transform.position = newPosV;
                rectangles.Add(Ob1V);

                newPosV = new Vector2(r.position.x,(r.position.y + r.VerticalSize / 2 + pos.y) / 2);
                Ob1V = Instantiate(this.bodyRectPrefab, newPosV, Quaternion.identity);
                Ob1V.Initialize(r.HorizontalSize, Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
                Ob1V.transform.position = newPosV;
                rectangles.Add(Ob1V);
                break;
            case BodySplitType.Cross:
                Vector2 newPosC = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
                BodyRectangle Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity);
                Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
                Ob1C.transform.position = newPosC;
                rectangles.Add(Ob1C);

                newPosC = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, (r.position.y - r.VerticalSize / 2 + pos.y) / 2);
                Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity);
                Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y - r.VerticalSize / 2)));
                Ob1C.transform.position = newPosC;
                rectangles.Add(Ob1C);

                newPosC = new Vector2((r.position.x - r.HorizontalSize / 2 + pos.x) / 2, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
                Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity);
                Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x - r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
                Ob1C.transform.position = newPosC;
                rectangles.Add(Ob1C);

                newPosC = new Vector2((r.position.x + r.HorizontalSize / 2 + pos.x) / 2, (r.position.y + r.VerticalSize / 2 + pos.y) / 2);
                Ob1C = Instantiate(this.bodyRectPrefab, newPosC, Quaternion.identity);
                Ob1C.Initialize(Mathf.Abs(pos.x - (r.position.x + r.HorizontalSize / 2)), Mathf.Abs(pos.y - (r.position.y + r.VerticalSize / 2)));
                Ob1C.transform.position = newPosC;
                rectangles.Add(Ob1C);
                break;
        }

        // delete rectangle
        rectangles.Remove(r);
        Destroy(r.gameObject);
    }
}