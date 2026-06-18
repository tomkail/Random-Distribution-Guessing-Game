using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distribution : ScriptableObject {
    public int[] returns;
    int index;

    public int Sample () {
        // var val = returns.GetRepeating(index);
        // index++;
        // return val;
        return returns.Random();
    }
}