using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] int countBlocks = 0;
    SceneLoader sceneloader;
    void Start()
    {
        sceneloader = Object.FindAnyObjectByType<SceneLoader>();
    }
    public void CountBreakableBlocks()
    {
        countBlocks++;
    }
    public void BlockDestroyed()
    {
        countBlocks--;
        if (countBlocks <= 0)
        {
            sceneloader.LoadNextScene();
        }
    }
}
