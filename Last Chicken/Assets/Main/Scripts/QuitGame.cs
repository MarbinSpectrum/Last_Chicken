using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }
}