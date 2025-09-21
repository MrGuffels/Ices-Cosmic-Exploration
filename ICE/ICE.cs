using ECommons.Automation.NeoTaskManager;
using ECommons.Configuration;
using ICE.IPC;
using ICE.Ui;
using ICE.Config;
using Pictomancy;
using System.Collections.Generic;
using static ICE.Utilities.CosmicHelper;

namespace ICE;

public sealed partial class ICE : IDalamudPlugin
{
    public static string Name => "ICE";

    internal static ICE P = null!;
    private readonly Configuration Config;
    private static WaypointInfo? waypointInfo;
    private static MissionConfigs missionConfigs;

    public static Configuration OldConfig => P.Config;
    public static MissionConfigs C => missionConfigs ??= LoadConfig<MissionConfigs>();
    public static WaypointInfo D => waypointInfo ??= LoadConfig<WaypointInfo>();

    // Yaml Config Loaders. For both loading a yaml in the config folder, and for embedded
    private static T LoadConfig<T>() where T : IYamlConfig, new()
    {
        var path = typeof(T).GetProperty("ConfigPath")!.GetValue(null)!.ToString()!;
        var config = YamlConfig.Load<T>(path);

        if (config == null)
        {
            PluginLog.Warning($"[{typeof(T).Name}] Config was null. Creating new default.");
            config = new T();
            YamlConfig.Save(config, path);
        }

        PluginLog.Information($"[{typeof(T).Name}] Loaded from {path}");
        return config;
    }

    private static T LoadEmbeddedConfig<T>(string resourceName) where T : IYamlConfig, new()
    {
        var config = YamlConfig.LoadFromResource<T>(resourceName);

        if (config == null)
        {
            PluginLog.Warning($"[{typeof(T).Name}] Embedded config was null. Returning new default.");
            config = new T();
        }

        PluginLog.Information($"[{typeof(T).Name}] Loaded from embedded resource: {resourceName}");
        return config;
    }

    // Window's that I use, base window to the settings... need these to actually show shit 
    internal WindowSystem windowSystem;
    internal MainWindowV2 mainWindow2;
    internal SettingsWindowV2 settingsWindowV2;
    internal OverlayWindow overlayWindow;
    internal DebugWindow debugWindow;

    // Taskmanager from Ecommons
    internal TaskManager TaskManager;

    // Internal IPC's that I use for... well plugins. 
    internal LifestreamIPC Lifestream;
    internal NavmeshIPC Navmesh;
    internal PandoraIPC Pandora;
    internal ArtisanIPC Artisan;
    internal VislandIPC Visland;
    internal AutoHookIPC AutoHook;
    internal IceCosmicExplorationIPC IceIpc;

    public ICE(IDalamudPluginInterface pi)
    {
        P = this;
        ECommonsMain.Init(pi, P, Module.DalamudReflector, ECommons.Module.ObjectFunctions);
        PictoService.Initialize(pi);

        EzConfig.Migrate<Configuration>();
        Config = EzConfig.Init<Configuration>();

        //IPC's that are used
        Lifestream = new();
        Navmesh = new();
        Pandora = new();
        Artisan = new();
        Visland = new();
        AutoHook = new();
        IceIpc = new();

        // all the windows
        windowSystem = new();
        mainWindow2 = new();
        settingsWindowV2 = new();
        overlayWindow = new();
        debugWindow = new();

        EzCmd.Add("/icecosmic", OnCommand, """
            Open plugin interface
            /ice help - shows all commands
            /ice clear - removes all missions
            /ice stop - stops ICE
            /ice start - Starts ICE
            /ice add | remove | toggle | only 
            /ice flag [id] - Opens the map and marks where the area of gathering is.
            """);
        EzCmd.Add("/ice", OnCommand);
        EzCmd.Add("/IceCosmic", OnCommand);
        Init();
        Svc.Framework.Update += Tick;

        TaskManager = new(new(showDebug: true));
        Svc.PluginInterface.UiBuilder.Draw += windowSystem.Draw;
        Svc.PluginInterface.UiBuilder.OpenMainUi += () =>
        {
            mainWindow2.IsOpen = true;
        };
        Svc.PluginInterface.UiBuilder.OpenConfigUi += () =>
        {
            settingsWindowV2.IsOpen = true;
        };
        DictionaryCreation();
        Task_Gamba.EnsureGambaWeightsInitialized();
        ConfigMigrator.UpdateConfigMissionList();
        ConfigMigrator.MigrateConfigv1();
        ConfigMigrator.CheckMissions();
    }

    private static void Init()
    {
        ExcelHelper.Init();
        ConsumableInfo.Init();
    }

    private void Tick(object _)
    {
        if (Svc.ClientState.LocalPlayer != null)
        {
            PlayerHandlers.Tick();
            if (SchedulerMain.State != IceState.Idle)
                SchedulerMain.Tick();
            WeatherForecastHandler.Tick();
        }
        else
        {
            PlayerHandlers.DisablePlugin();
        }
        GenericManager.Tick();
        TextAdvancedManager.Tick();
        YesAlreadyManager.Tick();
    }

    public void Dispose()
    {
        GenericHelpers.Safe(() => Svc.Framework.Update -= Tick);
        GenericHelpers.Safe(() => Svc.PluginInterface.UiBuilder.Draw -= windowSystem.Draw);
        GenericHelpers.Safe(TextAdvancedManager.UnlockTA);
        GenericHelpers.Safe(YesAlreadyManager.Unlock);
        ECommonsMain.Dispose();
        PictoService.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        var subcommands = args.Split(' ');

        if (subcommands.Length == 0 || args == "")
        {
            mainWindow2.IsOpen = !mainWindow2.IsOpen;
            return;
        }

        var firstArg = subcommands[0];

        if (firstArg.ToLower() == "d" || firstArg.ToLower() == "debug")
        {
            debugWindow.IsOpen = true;
            return;
        }
        else if (firstArg.ToLower() == "s" || firstArg.ToLower() == "settings")
        {
            settingsWindowV2.IsOpen = !settingsWindowV2.IsOpen;
            return;
        }
        else if (firstArg.ToLower() == "clear")
        {
            foreach (var mission in C.MissionConfig)
            {
                mission.Value.Enabled = false;
            }
            C.Save();
        }
        else if (firstArg.ToLower() == "stop")
        {
            SchedulerMain.DisablePlugin();
        }
        else if (firstArg.ToLower() == "start")
        {
            SchedulerMain.EnablePlugin();
        }
        else if (firstArg.ToLower() == "add")
        {
            uint[] ids = [.. subcommands.Skip(1).Select(uint.Parse)];
            var idSet = new HashSet<uint>(ids);
            if (ids.Length == 0) return;

            foreach (var id in idSet)
            {
                if (C.MissionConfig.TryGetValue(id, out var mission))
                {
                    mission.Enabled = true;
                }
            }
            C.Save();
        }
        else if (firstArg.ToLower() == "remove")
        {
            uint[] ids = [.. subcommands.Skip(1).Select(uint.Parse)];
            var idSet = new HashSet<uint>(ids);
            if (ids.Length == 0) return;

            foreach (var id in idSet)
            {
                if (C.MissionConfig.TryGetValue(id, out var mission))
                {
                    mission.Enabled = false;
                }
            }
            C.Save();
        }
        else if (firstArg.ToLower() == "toggle")
        {
            uint[] ids = [.. subcommands.Skip(1).Select(uint.Parse)];
            var idSet = new HashSet<uint>(ids);
            if (ids.Length == 0) return;

            foreach (var id in idSet)
            {
                if (C.MissionConfig.TryGetValue(id, out var mission))
                {
                    mission.Enabled = !mission.Enabled;
                }
            }
            C.Save();
        }
        else if (firstArg.ToLower() == "only")
        {
            uint[] ids = [.. subcommands.Skip(1).Select(uint.Parse)];
            var idSet = new HashSet<uint>(ids);
            if (ids.Length == 0) return;

            foreach (var mission in C.MissionConfig.Where(x => x.Value.Enabled))
            {
                mission.Value.Enabled = false;
            }
            foreach (var id in idSet)
            {
                if (C.MissionConfig.TryGetValue (id, out var mission))
                {
                    mission.Enabled = true;
                }
            }
        }
        else if (firstArg.ToLower() == "flag")
        {
            if (subcommands.Length != 2) return;
            if (!PlayerHelper.IsInCosmicZone()) return;

            int missionId = int.Parse(subcommands[1]);
            var info = SheetMissionDict.FirstOrDefault(mission => mission.Key == missionId);
            if (info.Value == default) return;
            if (info.Value.MarkerId == 0) return;

            Utils.SetGatheringRing(info.Value.TerritoryId, (int)info.Value.MapPosition.X, (int)info.Value.MapPosition.Y, info.Value.Radius, info.Value.Name);
        }
        else if (firstArg.ToLower() == "help")
        {
            string helpMessage = $"- - ICE Commands Help - - \n" +
                                 $"/ice help - show all available commands\n" +
                                 $"/ice -> opens the main settings\n" +
                                 $"/ice s -> opens the settings menu\n" +
                                 $" - - - Mission specific - - - \n" +
                                 $"/ice stop - Stops ICE\n" +
                                 $"/ice start - starts ICE \n" +
                                 $"The rest of the commands work by doing a single id/multiple in a row \n" +
                                 $"EX. /ice add 10 155 185\n" +
                                 $"/ice add (ids) - enables select missions\n" +
                                 $"/ice remove (ids) - removes/disables select missions\n" +
                                 $"/ice toggle (ids) - toggles select mission ids" +
                                 $"/ice only (ids) - makes only select missions enabled" +
                                 $"/ice flag (id) - opens the map and flags the mission (if it has one).\n";
            Svc.Chat.Print(helpMessage);
        }
    }
}
