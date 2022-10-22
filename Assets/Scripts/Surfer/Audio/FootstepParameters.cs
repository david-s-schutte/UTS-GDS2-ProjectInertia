#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;

namespace Surfer.Audio
{
    [CreateAssetMenu(fileName = "Footstep Parameter", menuName = "Surfer/Audio Parameters", order = 0)][Serializable]
    public class FootstepParameters : Parameters
    {
        [HideInInspector] public float StepDistance;
        [HideInInspector] public string[] MaterialTypes;
        [HideInInspector] public bool overrideMaterial;
        


#if UNITY_EDITOR
        
        public void UpdateMaterials(string[] Materials) => MaterialTypes = Materials;
        
        #endif
    }
}