using UnityEngine;

namespace Code.Tutorials
{
    [CreateAssetMenu(fileName = "TutorialPart", menuName = "SO/Tutorial/Part", order = 0)]
    public class TutorialPartDataSO : ScriptableObject
    {
        [TextArea] public string explanation;
    }
}