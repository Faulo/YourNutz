using UnityEngine;
using UnityEngine.SceneManagement;

namespace NuttinToLose {
    [CreateAssetMenu(fileName = "LoadScene", menuName = "ScriptableObjects/LoadScene", order = 1)]
    public class LoadScene : ScriptableObject {
        public string sceneToLoad;

        public void Load() {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        }
    }
}