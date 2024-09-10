using UnityEngine;
using Thanabardi.Generic.Utility;
using Thanabardi.FantasySnake.Utility;
using System;

namespace Thanabardi.FantasySnake.Core.GameScene
{
    public class GameSceneManager : MonoSingleton<GameSceneManager>
    {
        [SerializeField]
        private SceneLoader _sceneLoader;

        public void GoToScene(SceneKey sceneKey, Action callback = null)
        {
            switch (sceneKey)
            {
                case SceneKey.MenuScene:
                    _sceneLoader.LoadSceneByName(sceneKey.ToString(), callback);
                    break;
                case SceneKey.GameScene:
                    _sceneLoader.LoadSceneByName(sceneKey.ToString(), callback);
                    break;
                default:
                    Debug.LogError("Scene not found");
                    break;
            }
        }

        public enum SceneKey
        {
            MenuScene,
            GameScene,
        }
    }
}