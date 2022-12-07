using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    public virtual void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
