
using Language;

namespace DCParry;

class DCParryMod : ModBase , Modding.IGlobalSettings<Settings>, Modding.ICustomMenuMod
{
    public static AudioClip parryAudio;
    public static Texture2D shieldTex;
    public static Sprite shieldSprite;
    public override void Initialize()
    {
        parryAudio = Resources.Load<AudioClip>("hero_parry");
        /*Modding.ModHooks.HeroUpdateHook += () => {
            if(!HeroController.instance.GetComponent<HeroAttach>()) 
                HeroController.instance.gameObject.AddComponent<HeroAttach>();
        };*/
        On.HeroController.Start += (orig,self)=>{
            orig(self);
            self.gameObject.AddComponent<HeroAttach>();
        };
        using(var stream = typeof(DCParryMod).Assembly.GetManifestResourceStream("DCParry.Images.Shield"))
        {
            byte[] b = new byte[stream.Length];
            stream.Read(b, 0, b.Length);
            shieldTex = new Texture2D(1,1);
            shieldTex.LoadImage(b);

            shieldSprite = Sprite.Create(shieldTex, new Rect(0, 0, shieldTex.width
            , shieldTex.height), Vector2.zero, 44);
        }
    }
    protected override (LanguageCode, string)[] Languages => new (LanguageCode, string)[]{
        (LanguageCode.EN, "DCParry.Languages.en"),
        (LanguageCode.ZH, "DCParry.Languages.zh-cn")
    };
    protected override LanguageCode DefaultLanguageCode => LanguageCode.EN;
    public bool ToggleButtonInsideMenu => false;
    public static SettingsMenu menu;
    public MenuScreen GetMenuScreen(MenuScreen returnScreen, Modding.ModToggleDelegates? _)
    {
        menu = new(returnScreen);
        return menu;
    }
    public static Settings settings = new();
    public void OnLoadGlobal(Settings s)
    {
        settings = s;
    }
    public Settings OnSaveGlobal()
    {
        return settings;
    }
}
