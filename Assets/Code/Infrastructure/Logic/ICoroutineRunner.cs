using System.Collections;
using UnityEngine;

namespace Code.Infrastructure.Logic
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(IEnumerator sendHeartbeatCoroutine);
    }
}