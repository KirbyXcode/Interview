using UnityEngine;
using UnityEditor;

public class Menu 
{
    [MenuItem("Tools/Settings")]
	public static void Settings()
    {
        SettingsWindow win = EditorWindow.GetWindow(typeof(SettingsWindow)) as SettingsWindow;
        win.titleContent = new GUIContent("全局设置");
        win.Show();
    }
}
