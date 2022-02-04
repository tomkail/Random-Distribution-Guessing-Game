using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLayout : MonoBehaviour
{
    public SLayout[] boxes;
    public float radius;

    void OnValidate () {
        for (int i = 0; i < boxes.Length; i++) {
            var layout = boxes[i];
            var degrees = MathX.DegreesFromRange(i, boxes.Length);
            layout.center = MathX.DegreesToVector2(degrees) * radius;
        }
    }
}
