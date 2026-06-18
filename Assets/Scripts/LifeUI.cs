using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThisOtherThing.UI.Shapes;

public class LifeUI : MonoBehaviour {
    public SLayout layout;
    public Rectangle rectangle;
    public void SetFilled(bool filled) {
        rectangle.ShapeProperties.DrawFill = filled;
        rectangle.ForceMeshUpdate();
    }
}