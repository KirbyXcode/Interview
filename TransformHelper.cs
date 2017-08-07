using UnityEngine;
using System.Collections;

/// <summary>
/// 变换组件助手类
/// </summary>
public class TransformHelper
{
    public static Transform GetChild(Transform parentTF, string childName)
    {
        //在子物体中根据名称查找
        Transform childTF = parentTF.FindChild(childName);
        if (childTF != null) return childTF;
        for (int i = 0; i < parentTF.childCount; i++)
        {//将问题转移给子物体
            childTF = GetChild(parentTF.GetChild(i), childName);
            if (childTF != null)
                return childTF;
        }
        return null;
    }
}
