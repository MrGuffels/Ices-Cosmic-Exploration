using System.Threading.Tasks;

namespace ICE.Config;

public interface IYamlConfig
{
    Task SaveAsync();
    void Save();
    static abstract string ConfigPath { get; }
}
