using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Helpers
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly ConcurrentQueue<Action> executeOnMainThreadQueue = new ConcurrentQueue<Action>();
        private static MainThreadDispatcher instance;

        public static MainThreadDispatcher Instance
        {
            get
            {
                Initialize();
                return instance;
            }
        }

        public static void Initialize()
        {
            if (instance == null)
            {
                Console.WriteLine("MainThreadDispatcher Initialize called.");
                GameObject obj = new GameObject("MainThreadDispatcher");
                instance = obj.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(obj);
            }
        }

        public static void Enqueue(Action action)
        {
            Console.WriteLine("Enqueuing Action");
            executeOnMainThreadQueue.Enqueue(action);
        }

        private void Awake()
        {
            Console.WriteLine("MainThreadDispatcher Awake called.");
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            Console.WriteLine("MainThreadDispatcher Start called.");
        }

        private void Update()
        {
            while (executeOnMainThreadQueue.TryDequeue(out var action))
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while executing action on main thread: {ex}");
                    // Handle any exceptions thrown by the action
                }
            }
        }
    }

}
