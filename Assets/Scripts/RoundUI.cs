using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    public SLayout layout;
    void Update()
    {
        layout.textMeshPro.text = "Round "+GameController.Instance.round.ToString();
    }
}
