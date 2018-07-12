using UnityEngine;

public abstract class Global
{
    public static GameObject FindChild(Transform trans, string childName)
    {
        Transform child = trans.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }
        int count = trans.childCount;
        GameObject go = null;
        for (int i = 0; i < count; ++i)
        {
            child = trans.GetChild(i);
            go = FindChild(child, childName);
            if (go != null)
                return go;
        }
        return null;
    }

    public static T FindChild<T>(Transform trans, string childName) where T : Component
    {
        GameObject go = FindChild(trans, childName);
        if (go == null)
            return null;
        return go.GetComponent<T>();
    }
}
