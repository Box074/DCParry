
namespace DCParry;

class Shield : MonoBehaviour
{
    private Rigidbody2D rig;
    private HeroController hc;
    public HeroAttach ha;
    private bool tlf = false;
    private void OnTriggerEnter2D(Collider2D other) {
        var dh = other.GetComponent<DamageHero>();
        if(dh == null) return;
        if(!tlf)
        {
            ha.TriggerParry();
            tlf = true;
            if(hc.cState.facingRight)
            {
                rig.velocity = new Vector2(-hc.RECOIL_HOR_VELOCITY, 0);
            }
            else
            {
                rig.velocity = new Vector2(hc.RECOIL_HOR_VELOCITY, 0);
            }
        }
        
        var hm = other.GetComponent<HealthManager>() ??
             other.GetComponentInParent<HealthManager>();
        if (hm != null)
        {
            FSMUtility.SendEventToGameObject(hm.gameObject, "STUN");
            FSMUtility.SendEventToGameObject(hm.gameObject,"PARRIED");
            HitTaker.Hit(hm.gameObject, new()
            {
                Multiplier = 1,
                AttackType = AttackTypes.Generic,
                MagnitudeMultiplier = 1,
                CircleDirection = true,
                SpecialType = SpecialTypes.None,
                DamageDealt = dh.damageDealt * hc.playerData.nailDamage,
                Direction = hc.cState.facingRight ? 0 : 180
            });
        }
        else
        {
            var go = dh.gameObject;
            if (go.name.StartsWith("Needle"))
            {
                go.LocateMyFSM("Control").SetState("Return");
                return;
            }
            if (go.GetComponent<Rigidbody2D>() != null)
            {
                go.AddComponent<OtherDirection>();
            }
            dh.gameObject.AddComponent<DamageC>();
        }
    }
    private void Update() {
        tlf = false;
    }
    private void Awake() {
        hc = HeroController.instance;
        rig = hc.GetComponent<Rigidbody2D>();
    }
}
