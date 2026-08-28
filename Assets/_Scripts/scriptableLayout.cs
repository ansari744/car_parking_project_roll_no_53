using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "layout", menuName = "Layout")]
public class scriptableLayout : ScriptableObject
{
    public List<Button> buttons = new List<Button>();
}
