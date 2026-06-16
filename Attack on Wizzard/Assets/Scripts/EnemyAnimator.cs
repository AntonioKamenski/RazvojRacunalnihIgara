using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Idle / Walk")]
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private float idleFps = 8f;

    [Header("Attack (optional)")]
    [SerializeField] private Sprite[] attackFrames;
    [SerializeField] private float attackFps = 12f;

    private SpriteRenderer sr;
    private bool isPlayingAttack;
    private int idleFrame;
    private float idleTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (idleFrames != null && idleFrames.Length > 0)
            sr.sprite = idleFrames[0];
    }

    private void Update()
    {
        if (isPlayingAttack) return;
        if (idleFrames == null || idleFrames.Length == 0) return;

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            idleTimer = 1f / Mathf.Max(0.1f, idleFps);
            idleFrame = (idleFrame + 1) % idleFrames.Length;
            sr.sprite = idleFrames[idleFrame];
        }
    }

    public void TriggerAttack()
    {
        if (attackFrames == null || attackFrames.Length == 0) return;
        if (isPlayingAttack) return;
        StartCoroutine(PlayAttackAnim());
    }

    private IEnumerator PlayAttackAnim()
    {
        isPlayingAttack = true;
        float delay = 1f / Mathf.Max(0.1f, attackFps);
        for (int i = 0; i < attackFrames.Length; i++)
        {
            sr.sprite = attackFrames[i];
            yield return new WaitForSeconds(delay);
        }
        isPlayingAttack = false;
        idleFrame = 0;
        idleTimer = 0f;
    }
}
