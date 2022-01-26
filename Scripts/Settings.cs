
namespace DCParry;

[Serializable]
class Settings 
{
    [JsonConverter(typeof(PlayerActionSetConverter))]
    public KeySettings keySettings = new();
}

class KeySettings : PlayerActionSet
{
    public KeySettings()
    {
        parry = CreatePlayerAction("ParryKey");
    }
    public PlayerAction parry;
}
