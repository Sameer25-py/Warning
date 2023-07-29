using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public BallController BallController;


        private void Start()
        {
            BallController.IsGameStarted = true;
        }
    }
}