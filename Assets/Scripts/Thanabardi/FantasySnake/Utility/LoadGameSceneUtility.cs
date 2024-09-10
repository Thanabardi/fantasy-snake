using System;
using System.Collections;
using Thanabardi.FantasySnake.Core.System;
using Thanabardi.FantasySnake.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thanabardi.FantasySnake.Utility
{
    public class SceneLoader : MonoBehaviour
    {
        private Coroutine _loadSceneCoroutine;
        private SceneLoadPanel _sceneLoadPanel;

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

            _sceneLoadPanel = (SceneLoadPanel)UIManager.Instance.SetPanelActive(UIManager.UIKey.SceneLoadPanel, true);

            while (!operation.isDone)
            {
                // float progress = Mathf.Clamp01(operation.progress / 0.9f);

                // _sceneLoadPanel.SetSliderValue(progress);
                yield return null;
            }

            _loadSceneCoroutine = null;
            UIManager.Instance.SetPanelActive(UIManager.UIKey.SceneLoadPanel, false);

            callback?.Invoke();
        }
    }
}