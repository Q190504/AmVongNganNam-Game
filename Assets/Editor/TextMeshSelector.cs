using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class SelectTMPObjects : EditorWindow
{
    [MenuItem("Tools/Select All TMP Objects")]
    public static void ShowWindow()
    {
        GetWindow<SelectTMPObjects>("Select TMP Objects");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Select All TextMeshProUGUI in Scene"))
        {
            SelectAllTMPUGUI();
        }
        if (GUILayout.Button("Select All TextMeshPro (3D) in Scene"))
        {
            SelectAllTMP3D();
        }
        if (GUILayout.Button("Select All TMP (UI + 3D)"))
        {
            SelectAllTMP();
        }
    }

    private void SelectAllTMPUGUI()
    {
        TextMeshProUGUI[] uiTexts = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);
        SelectGameObjects(uiTexts);
    }

    private void SelectAllTMP3D()
    {
        TextMeshPro[] texts3D = GameObject.FindObjectsOfType<TextMeshPro>(true);
        SelectGameObjects(texts3D);
    }

    private void SelectAllTMP()
    {
        List<GameObject> all = new List<GameObject>();

        all.AddRange(GetGameObjectsFromComponents(GameObject.FindObjectsOfType<TextMeshProUGUI>(true)));
        all.AddRange(GetGameObjectsFromComponents(GameObject.FindObjectsOfType<TextMeshPro>(true)));

        Selection.objects = all.ToArray();
        Debug.Log($"Selected {all.Count} TMP objects in Hierarchy.");
    }

    private void SelectGameObjects(Component[] components)
    {
        List<GameObject> gos = GetGameObjectsFromComponents(components);
        Selection.objects = gos.ToArray();
        Debug.Log($"Selected {gos.Count} TMP objects in Hierarchy.");
    }

    private List<GameObject> GetGameObjectsFromComponents(Component[] components)
    {
        List<GameObject> gos = new List<GameObject>();
        foreach (var comp in components)
        {
            if (comp != null)
                gos.Add(comp.gameObject);
        }
        return gos;
    }
}
