
namespace DCParry;

class DamageC : MonoBehaviour
{
    private int origDamage;
    private DamageEnemies de;
    private void Awake() {
        var dh = GetComponent<DamageHero>();
        if(dh!= null)
        {
            origDamage = dh.damageDealt;
            de = gameObject.AddComponent<DamageEnemies>();
            de.attackType = AttackTypes.Generic;
            de.damageDealt = origDamage;
            de.magnitudeMult = 1;
            de.ignoreInvuln = false;
            de.specialType = SpecialTypes.None;
        }
    }
    private void Update() {
        var dh = GetComponent<DamageHero>();
        if(dh!=null)
        {
            if(dh.damageDealt != -1)
            {
                origDamage = dh.damageDealt;
                dh.damageDealt = -1;
            }
        }
    }
    private void OnDisable() {
        var dh = GetComponent<DamageHero>();
        if(dh!=null)
        {
            dh.damageDealt = origDamage;
        }
        if(de != null)
        {
            Destroy(de);
        }
        Destroy(this);
    }
}
class OtherDirection : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool x;
    private bool y;
    private void Awake() {
        rig = GetComponent<Rigidbody2D>();
        if(rig == null) 
        {
            Destroy(this);
            return;
        }
        x = rig.velocity.x < 0;
        y = rig.velocity.y < 0;
    }
    private void Start() {
        
    }
    private void Update() {
        Vector2 vel = rig.velocity;
        if((rig.velocity.x > 0 && !x) || (rig.velocity.x < 0 && x))
        {
            vel.x = -vel.x;
        }
        if((rig.velocity.y > 0 && !y) || (rig.velocity.y < 0 && y))
        {
            vel.y = -vel.y;
        }
        rig.velocity = vel;
    }
    private void OnDisable() {
        Destroy(this);
    }
}
