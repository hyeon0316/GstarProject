using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DuloGames.UI
{
    [ExecuteInEditMode]
    public class BreakPrefabConnection : MonoBehaviour
    {
        void Start()
        {
            DestroyImmediate(this); // Remove this script
        }
    }
}