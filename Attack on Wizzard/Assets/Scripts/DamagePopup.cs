using UnityEngine;
using TMPro;
using System.Collections;

// Prefab requires a TextMeshPro (3D, NOT TextMeshProUGUI) component.
// Place the prefab anywhere — it auto-positions in world space.
public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float randomXRange = 0.4f;

    private TMP_Text tmp;

    public static void Spawn(Vector2 worldPos, float amount, ElementType element)
    {
        var prefab = GameStatsTracker.DamagePopupPrefab;
        if (prefab == null) return;
        var go = Instantiate(prefab, (Vector3)worldPos + Vector3.back, Quaternion.identity);
        go.GetComponent<DamagePopup>()?.Setup(amount, element);
    }

    private void Awake() => tmp = GetComponentInChildren<TMP_Text>();

    private void Setup(float amount, ElementType element)
    {
        if (tmp == null) return;
        tmp.text  = Mathf.RoundToInt(amount).ToString();
        tmp.color = GetElementColor(element);
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float xDrift = Random.Range(-randomXRange, randomXRange);
        float elapsed = 0f;
        Color startColor = tmp.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            transform.position += new Vector3(xDrift, floatSpeed, 0f) * Time.deltaTime;
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            yield return null;
        }
        Destroy(gameObject);
    }

    private static Color GetElementColor(ElementType element) => element switch
    {
        ElementType.Fire      => new Color(1f,   0.4f, 0f),
        ElementType.Ice       => new Color(0.4f, 0.9f, 1f),
        ElementType.Lightning => new Color(1f,   0.95f, 0f),
        ElementType.Shadow    => new Color(0.6f, 0.2f, 0.9f),
        ElementType.Bleed     => new Color(0.8f, 0.1f, 0.1f),
        _                     => Color.white
    };
}
