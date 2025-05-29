using TMPro;
using UnityEngine;

public class TextSwitcher : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public TextAsset textFile;
    public float switchDelay = 3f;

    private string[] lines;
    private int lastLineIndex = -1;
    private float timer;

    void Start()
    {
        if (textFile != null)
        {
            lines = textFile.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            ShowNextLine();
            timer = switchDelay;
        }
        else
        {
            uiText.text = "Chào mừng đến với Âm Vọng Ngàn Năm!";
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Auto switch when time is up
        if (timer <= 0f)
        {
            ShowNextLine();
            timer = switchDelay;
        }

        // Detect mouse click (PC) or touch (Mobile)
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            ShowNextLine();
            timer = switchDelay;
        }
    }

    void ShowNextLine()
    {
        if (lines == null || lines.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, lines.Length);
        } while (newIndex == lastLineIndex && lines.Length > 1);

        lastLineIndex = newIndex;
        uiText.text = lines[newIndex].Trim();
    }
}
