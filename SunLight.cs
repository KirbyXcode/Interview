using UnityEngine;
using System.Collections;

public class SunLight : MonoBehaviour 
{
	IEnumerator Start()
    {
        float day = 24.0f;
        float now = 0.0f;
        float nightDegree = 210.0f;
        while(true)
        {
            now = (now + Time.deltaTime) % day;
            float degree = nightDegree + now / day * 360.0f;
            transform.rotation = Quaternion.Euler(degree, 0, 0);
            yield return new WaitForFixedUpdate();
        }
    }
}
