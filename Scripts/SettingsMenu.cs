
namespace DCParry;

class SettingsMenu : CustomMenu
{
    public SettingsMenu(MenuScreen rs) : base(rs, "Dead Cells Parry")
    {

    }
    protected override void Build(ContentArea contentArea)
    {
        contentArea.AddKeybind("parryKeybind", DCParryMod.settings.keySettings.parry,
            new()
            {
                Label = "DCParry.ParryKeybind".Get(),
                CancelAction = Back,
                Style = KeybindStyle.VanillaStyle
            });
    }
}
