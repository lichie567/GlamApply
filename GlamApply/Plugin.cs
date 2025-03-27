using System;
using System.Reflection;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;

namespace GlamApply
{
    public class Plugin : IDalamudPlugin
    {
        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.1.0.1";

        public string Name => "GlamApply";
        private readonly ICommandManager _commandManager;
        private readonly IPluginLog _logger;

        public Plugin(ICommandManager commandManager, IPluginLog logger)
        {
            _commandManager = commandManager;
            _logger = logger;

            _commandManager.AddHandler(
                "/gp",
                new CommandInfo(PluginCommand)
                {
                    HelpMessage = "Apply Glamour Plate",
                    ShowInHelp = true
                }
            );
        }

        private void PluginCommand(string command, string arguments)
        {
            arguments = arguments.Trim();
            if (string.IsNullOrEmpty(arguments))
            {
                this.ResetGlamour();
            }
            else if (int.TryParse(arguments, out int plate))
            {
                this.ApplyGlamourPlate(plate);
            }
        }

        private unsafe void ApplyGlamourPlate(int plate)
        {
            var instance = RaptureGearsetModule.Instance();
            instance->EquipGearset(instance->CurrentGearsetIndex, (byte)plate);
        }

        private unsafe void ResetGlamour()
        {
            var instance = RaptureGearsetModule.Instance();
            instance->EquipGearset(instance->CurrentGearsetIndex, 0);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _commandManager.RemoveHandler("/gp");
            }
        }
    }
}
