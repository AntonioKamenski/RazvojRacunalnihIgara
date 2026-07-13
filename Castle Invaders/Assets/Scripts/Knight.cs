using UnityEngine;

public class Knight : MonoBehaviour
{
    Level level;
    [SerializeField] int hitPoints;
    [SerializeField] int timesHit;
    private void Start()
    {
        level = Object.FindAnyObjectByType<Level>();
        level.CountBreakableBlocks();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        timesHit++;
        if (timesHit >= hitPoints){

            Destroy(gameObject);
            level.BlockDestroyed();
        }
    }
}
