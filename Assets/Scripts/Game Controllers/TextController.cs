using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    private static TextMeshProUGUI pulseText;


    void Awake()
    {
        pulseText = GameObject.Find("Main Canvas/Pulse Text").GetComponent<TextMeshProUGUI>();
    }

    public static void UpdatePulseText(string newText)
    {
        pulseText.text = newText;
    }
}
