using UnityEditor;
using UnityEngine;

namespace Code.Util.Editor
{
    public class PlayerPrefsResetWindow : MonoBehaviour
    {
        [MenuItem("Window/Reset PlayerPrefs")]
        private static void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
            print("PlayerPrefs has been reset.");
        }
    }
}