using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float timeScale = 1f;

    void Update()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * timeScale;
    }
}
