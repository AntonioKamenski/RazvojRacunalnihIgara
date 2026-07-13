using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip[] breakSound;
    Level level;
    [SerializeField] GameObject blockBreakVFX;
    [SerializeField] Sprite[] hitSprites;
    [SerializeField] int hitPoints;
    [SerializeField] int timesHit;
    private void Start()
    {
        level = Object.FindAnyObjectByType<Level>();
        if (tag == "Breakable")
        {

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if (tag == "Breakable")
        {
            timesHit++;
            if (timesHit >= hitPoints){
                AudioSource.PlayClipAtPoint(breakSound[0], Camera.main.transform.position);

                Destroy(gameObject);

                triggerVFX();
            }
            else
            {
                AudioSource.PlayClipAtPoint(breakSound[0], Camera.main.transform.position);
                showNextHitSprite();
            }
        }
        
    }
    private void triggerVFX()
    {
        GameObject breakVFX = Instantiate(blockBreakVFX, transform.position, Quaternion.identity);
        Destroy(breakVFX, 1f);
    }
    private void showNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
    }
}
