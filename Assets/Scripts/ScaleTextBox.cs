using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScaleTextBox : MonoBehaviour
{
    private void OnEnable()
    {
        var tm = GetComponent<TextMeshProUGUI>();
        tm.ForceMeshUpdate();
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y * tm.textInfo.lineCount);
    }
}
