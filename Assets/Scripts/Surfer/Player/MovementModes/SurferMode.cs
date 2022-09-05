using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Surfer.Player.MovementModes;
using UnityEngine;


[CreateAssetMenu(fileName = "SurferMode",menuName = "Surfer/SurferMode")]
public class SurferMode : MovementMode
{
   [SerializeField] private float rotationRate;
   [SerializeField] private float brakeForce;
   [SerializeField] private float ollieHeight;
   
   [Header("Physics Variables")]
   [SerializeField] private LayerMask playerLayer;
   [SerializeField] private LayerMask groundLayer;

   private bool m_HitDetect;
   RaycastHit m_Hit;
   public float m_MaxDistance;

   
   protected override async UniTask OnModeChanged()
   {
      await base.OnModeChanged();
   }
}
