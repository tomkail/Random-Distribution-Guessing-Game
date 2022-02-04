using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribution : ScriptableObject {
    public int[] returns;

    public int Sample () {
        return returns.Random();
    }
}