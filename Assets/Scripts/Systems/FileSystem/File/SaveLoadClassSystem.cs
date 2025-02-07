using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SaveLoadClassSystem : MonoBehaviour
    {
        [SerializeField] List<ScriptableObject> saveScriptableObjects;
        List<IFile> filesList;


        private void Awake()
        {
            FindAndLoad();
            Debug.LogWarning("Pass scriptable objects to save/load!");
        }

        private void FindAndLoad()
        {
            FindObjects();
            Load();
        }
        
        private void FindObjects()
        {
            filesList = new List<IFile>(InterfaceService.FindOnAllScenes<IFile>(true).ToArray());
            filesList.AddRange(saveScriptableObjects);
        }

        private void Save()
        {
            foreach (ISavable file in filesList) { 
                file.Save();
            }
        }

        public void Load()
        {
            foreach (ILoadable file in filesList)
            {
                bool result = file.Load();
                if (result == false)
                {
                    Debug.Log("Loading is failed");
                }
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }
    }

    public static class InterfaceService
    {
        public static IEnumerable<T> FindOnActiveScene<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }

        public static IEnumerable<T> FindOnAllScenes<T>(bool includeInactive = false)
        {
            return Enumerable.Range(0, SceneManager.sceneCount).SelectMany(sceneIndex => SceneManager.GetSceneAt(sceneIndex).GetRootGameObjects().SelectMany(go => go.GetComponentsInChildren<T>(includeInactive)));
        }
    }
}