using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thanabardi.FantasySnake.Utility
{
    public class SceneLoader : MonoBehaviour
    {
        private Coroutine _loadSceneCoroutine;

        public void LoadSceneByName(string sceneName, Action callback = null)
        {
            if (_loadSceneCoroutine != null)
            {
                StopCoroutine(_loadSceneCoroutine);
            }
            _loadSceneCoroutine = StartCoroutine(LoadAsynchronously(sceneName, callback));
        }

        private IEnumerator LoadAsynchronously(string sceneName, Action callback)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            _loadSceneCoroutine = null;

            callback?.Invoke();
        }
    }
}