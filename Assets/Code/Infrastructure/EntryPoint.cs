using UnityEngine;

namespace Code.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Bootstrapper _bootstrapper;

        private void Awake()
        {
            if (Bootstrapper.IsStarted == false)
                Instantiate(_bootstrapper)
                    .StartGame();
        }
    }
}