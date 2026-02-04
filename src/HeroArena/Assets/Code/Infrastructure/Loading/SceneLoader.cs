using Code.Infrastructure.Helpers;
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Loading
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void NetworkLoad(string nextScene)
        {
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += SceneManager_OnSynchronizeComplete;

            var waitNextScene = NetworkManager.Singleton.SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }

        private void SceneManager_OnSynchronizeComplete(ulong clientId)
        {
            int totalClients = NetworkManager.Singleton.ConnectedClientsIds.Count;

            foreach (var kvp in NetworkManager.Singleton.SpawnManager.SpawnedObjects)
            {
                if (kvp.Value.IsPlayerObject)
                    Debug.Log($"Игрок → ClientId: {kvp.Value.OwnerClientId},  NetId: {kvp.Value.NetworkObjectId}");
            }

            Debug.Log($"totalClients {totalClients}");
        }

        public void LocalLoad(string name, Action onLoaded = null) =>
          _coroutineRunner.StartCoroutine(Load(name, onLoaded));

        private IEnumerator Load(string nextScene, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();

                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}