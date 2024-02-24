using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Logic
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string sceneName, Action onLoaded = null) => 
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, onLoaded));

        private static IEnumerator LoadScene(string name, Action onLoaded)
        {
            if (name == SceneManager.GetActiveScene().name)
            {
                onLoaded?.Invoke();
                yield break;
            }

            var waitScene = SceneManager.LoadSceneAsync(name);
            do
            {
                yield return null;
            } while (waitScene.isDone == false);

            onLoaded?.Invoke();
        }
    }
}