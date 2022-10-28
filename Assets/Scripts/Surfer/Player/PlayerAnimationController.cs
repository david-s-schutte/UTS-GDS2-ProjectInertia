using System.Collections;
using System.Collections.Generic;
using Surfer.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Surfer.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        public delegate void PlayerIntroCompleted();
        public PlayerIntroCompleted OnPlayerIntroCompleted;

        /// <summary>
        /// Event function called when intro is completed
        /// </summary>
        public void OnIntroCompleted() => OnPlayerIntroCompleted?.Invoke();
        



    }
}