using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsWindow : EditorWindow 
{
    private List<MacorItem> list = new List<MacorItem>();
    private Dictionary<string, bool> dict = new Dictionary<string, bool>();
    private string macor = null;

    public SettingsWindow()
    {
        list.Clear();
        list.Add(new MacorItem("DEBUG_MODEL", "调试模式", true, false));
        list.Add(new MacorItem("DEBUG_LOG", "打印日志", true, false));
        list.Add(new MacorItem("STAT_TD", "开启统计", false, true));
    }

    private void OnEnable()
    {
        macor = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

        for (int i = 0; i < list.Count; i++)
        {
            if (!string.IsNullOrEmpty(macor) && macor.IndexOf(list[i].model) != -1)
            {
                dict[list[i].model] = true;
            }
            else
            {
                dict[list[i].model] = false;
            }
        }
    }

    private void OnGUI()
    {
        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");

            dict[list[i].model] = GUILayout.Toggle(dict[list[i].model], list[i].displayName);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("保存",GUILayout.Width(100)))
        {
            SaveMacor();
        }

        if (GUILayout.Button("调试模式", GUILayout.Width(100)))
        {
            for (int i = 0; i < list.Count; i++)
            {
                dict[list[i].model] = list[i].isDebug;
            }

            SaveMacor();
        }

        if (GUILayout.Button("发布模式", GUILayout.Width(100)))
        {
            for (int i = 0; i < list.Count; i++)
            {
                dict[list[i].model] = list[i].isRelease;
            }

            SaveMacor();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SaveMacor()
    {
        macor = string.Empty;

        foreach (var item in dict)
        {
            if(item.Value)
            {
                macor += string.Format("{0};", item.Key);
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, macor);
    }

    public class MacorItem
    {
        public MacorItem(string model, string displayName, bool isDebug, bool isRelease)
        {
            this.model = model;
            this.displayName = displayName;
            this.isDebug = isDebug;
            this.isRelease = isRelease;
        }

        public string model;
        public string displayName;
        public bool isDebug;
        public bool isRelease;
    }
}
