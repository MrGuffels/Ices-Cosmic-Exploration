using ECommons.Configuration;

namespace ICE.ConfigFiles;

public partial class Config
{
    public int ConfigVersion = 1;
    public bool OldConfigMigrateV1 = false;

    public void Save()
    {
        EzConfig.Save();
    }

    public void SaveDebounced()
    {
        EzConfigExtensions.SaveDebounced();
    }
}
