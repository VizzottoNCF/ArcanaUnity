using UnityEngine;

[CreateAssetMenu(menuName = "Spells/SwordSlash")]
public class SlashSpell : Spell
{
    private Transform _firePoint;
    [Header("Magic Slash Config")]
    public int damage;
    public re_DamageType damageType;
    public GameObject sword;
    public LayerMask targetLayer;
    public override void rf_Activate(GameObject Parent)
    {
        // gets starting point for spell
        _firePoint = GameObject.Find("FirePoint").transform;

        // spawn sword gameobject and give attributes
                    
        GameObject spell = Instantiate(sword, _firePoint.position, Quaternion.identity, _firePoint);
        
        spell.GetComponent<DealDamage>().rf_ReceiveAttributes(damage, damageType, targetLayer);

        // make swing/thrust with animator
    }
}
