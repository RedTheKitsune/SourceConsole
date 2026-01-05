using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SourceConsole.UI
{
    public static class SourceConsoleBoot
    {
        public static GameObject Console;

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void EditorInit()
        {
            EditorApplication.playModeStateChanged += OnStateChanged;
        }

        private static void OnStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                // Avoid stale static reference when Domain Reload is disabled.
                Console = null;
            }
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            if (Console != null) return;

            var prefab = Resources.Load<GameObject>("SourceConsole");
            if (!prefab)
            {
                Debug.LogError("[SourceConsole] Could not load Resources/SourceConsole prefab.");
                return;
            }

            Console = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            Object.DontDestroyOnLoad(Console);
        }
    }
}