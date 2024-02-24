using Code.Infrastructure.GameStateMachine;
using Code.Infrastructure.GameStateMachine.State;
using Code.Infrastructure.Logic;
using Code.Services;
using UnityEngine;

namespace Code.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private const string InitialSceneKey = "Initial";
        public static bool IsStarted { get; private set; }

        public void StartGame()
        {
            IsStarted = true;
            DontDestroyOnLoad(this);

            SceneLoader sceneLoader = new(this);
            sceneLoader.Load(InitialSceneKey, () =>
             {
                 RegisterServices _ = new(sceneLoader, this);
                 AllServices.Container.Single<IGameStateMachine>().Enter<LoadMenuState>();
             });
        }
    }
}