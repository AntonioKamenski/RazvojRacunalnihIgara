using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] public int damage = 100;
    public void Hit() { 
        Destroy(gameObject); 
    }
    public int GetDamage() { 
        return damage; 
    }
}
