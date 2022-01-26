
namespace DCParry;

class HeroAttach : MonoBehaviour
{
    private GameObject shield;
    private HeroController hc;
    private GameManager gm;
    private tk2dSpriteAnimator anim;
    private Collider2D col;
    public static PlayerAction ParryAction => DCParryMod.settings.keySettings.parry;
    private AudioSource audio;
    public float lastParry = -1;
    public float parryCT = 0;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        if (audio == null) audio = gameObject.AddComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        hc = HeroController.instance;
        gm = GameManager.instance;
        anim = GetComponent<tk2dSpriteAnimator>();

        shield = new GameObject("Shield");
        shield.layer = (int)PhysLayers.HERO_ATTACK;
        shield.transform.parent = transform;

        var r = shield.AddComponent<SpriteRenderer>();
        r.sprite = DCParryMod.shieldSprite;
        r.drawMode = SpriteDrawMode.Sliced;
        r.size = new Vector2(0.5f, 1.5f);
        shield.transform.localPosition = new Vector3(-0.15f, -1.25f, -0.01f);
        shield.transform.localScale = new Vector3(-1, 1, 1);

        var collider = shield.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.5f, 1.5f);
        shield.AddComponent<Shield>().ha = this;

        shield.SetActive(false);
    }
    private void ParryFirstFrame()
    {
        shield.SetActive(true);
        lastParry = Time.time;
        hc.RelinquishControl();
        hc.StopAnimationControl();
        anim.Play("idle");
    }
    private IEnumerator ETriggerParry()
    {
        yield return new WaitForSeconds(0.15f);
        parryCT = 0.75f;
        lastParry = 1;
    }
    public void TriggerParry()
    {
        StartCoroutine(ETriggerParry());
        lastParry = -1;
        parryCT = 0.75f;
        DCParryMod.parryAudio?.PlayOnSource(audio);
        gm.FreezeMoment(0);
        hc.parryInvulnTimer = hc.INVUL_TIME_PARRY;
    }
    private void ParryTime()
    {
        hc.parryInvulnTimer = 0.1f;;
    }
    private void ParryEnd()
    {
        shield.SetActive(false);
        lastParry = -1;
        parryCT = parryCT < 0.25f ? 0.25f : parryCT;
        hc.RegainControl();
        hc.StartAnimationControl();
    }
    private void Update()
    {
        if (parryCT > 0) parryCT -= Time.deltaTime;
        if (ParryAction.IsPressed && parryCT <= 0 && !hc.cState.dead)
        {
            if (lastParry <= 0) ParryFirstFrame();
            float s = Time.time - lastParry;
            if (s < 0.75f)
            {
                ParryTime();
            }
        }
        else
        {
            if (lastParry >= 0) ParryEnd();
        }
    }
}
