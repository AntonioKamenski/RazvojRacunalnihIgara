using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField] private float collectRadius = 0.6f;
    [SerializeField] private float magnetSpeed = 8f;

    private int xpValue;
    private Transform player;

    public void Init(int value) => xpValue = value;

    private void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (XPManager.Instance != null && XPManager.Instance.MagnetismRadius > 0f
            && dist <= XPManager.Instance.MagnetismRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position,
                                                     magnetSpeed * Time.deltaTime);
            dist = Vector2.Distance(transform.position, player.position);
        }

        if (dist <= collectRadius)
        {
            XPManager.Instance?.AddXP(xpValue);
            AudioManager.Instance?.PlayXPPickup();
            Destroy(gameObject);
        }
    }
}
