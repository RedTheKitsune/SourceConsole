using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace SourceConsole
{
    public sealed class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        private static readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        // Optional: track main thread id to short-circuit when already on main thread.
        private static int _mainThreadId;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _instance = null;
            while (_queue.TryDequeue(out _)) { }
            _mainThreadId = 0;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureExists()
        {
            if (_instance != null) return;

            var go = new GameObject("[MainThreadDispatcher]");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<MainThreadDispatcher>();
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>Enqueue work to run on Unity main thread.</summary>
        public static void Post(Action action)
        {
            if (action == null) return;

            // If we're already on main thread, run immediately (optional behavior).
            if (_mainThreadId != 0 && Thread.CurrentThread.ManagedThreadId == _mainThreadId)
            {
                action();
                return;
            }

            _queue.Enqueue(action);
        }

        private void Update()
        {
            // Execute everything queued since last frame.
            while (_queue.TryDequeue(out var action))
            {
                try { action(); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
        }
    }
}