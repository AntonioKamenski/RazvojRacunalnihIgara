using UnityEngine;
using System.Collections;

public class CharacterAnimator : MonoBehaviour
{
    [Header("Idle / Walk")]
    [SerializeField] private Sprite[] idleWalkFrames;
    [SerializeField] private float idleFps = 8f;

    [Header("Attack")]
    [SerializeField] private Sprite[] attackFrames;
    [SerializeField] private float attackFps = 12f;

    private SpriteRenderer sr;
    private bool isPlayingAttack;
    private int idleFrame;
    private float idleTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (CharacterVisualDatabase.Instance != null && GameManager.Instance != null)
        {
            var v = CharacterVisualDatabase.Instance.GetVisuals(GameManager.Instance.SelectedClass);
            if (v != null) Setup(v.idleWalkFrames, v.idleFps, v.attackFrames, v.attackFps);
        }
    }

    private void Update()
    {
        if (isPlayingAttack) return;
        if (idleWalkFrames == null || idleWalkFrames.Length == 0) return;

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            idleTimer = 1f / Mathf.Max(0.1f, idleFps);
            idleFrame = (idleFrame + 1) % idleWalkFrames.Length;
            sr.sprite = idleWalkFrames[idleFrame];
        }
    }

    public void TriggerAttack()
    {
        if (attackFrames == null || attackFrames.Length == 0) return;
        if (isPlayingAttack) return;
        StartCoroutine(PlayAttackAnim());
    }

    public void Setup(Sprite[] idle, float iFps, Sprite[] attack, float aFps)
    {
        idleWalkFrames = idle;
        idleFps = iFps;
        attackFrames = attack;
        attackFps = aFps;
        idleFrame = 0;
        idleTimer = 0f;
        isPlayingAttack = false;
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (idle != null && idle.Length > 0) sr.sprite = idle[0];
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
