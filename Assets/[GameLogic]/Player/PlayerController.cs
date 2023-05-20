using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        //TODO: move it to config
        //temp
        public float speed = 5f;
        
        private FixedJoystick _joystick;
        private PlayerMovement _movement;
        //Player stats
        //Player combat
        
        [Inject]
        public void Constructor(FixedJoystick fixedJoystick, PlayerMovement movement)
        {
            _joystick = fixedJoystick;
            _movement = movement;
        }

        private void Update()
        {
            transform.Translate(new Vector3(-(_joystick.Direction.x * (speed * Time.deltaTime)), 0, -(_joystick.Direction.y * (speed * Time.deltaTime))));
        }
    }
}