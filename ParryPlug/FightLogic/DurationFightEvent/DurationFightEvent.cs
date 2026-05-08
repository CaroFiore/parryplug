using FFXIVClientStructs.FFXIV.Client.Game.Character;
using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Dalamud.Game.Player;
using System.Diagnostics.Metrics;
using System.Linq;
namespace ParryPlug;

/* A duration event describes an event with a duration.
For example, a tether between players has a start time, when it appears, and a stop time, when it disappears.
If you want specific checks, use InstantFightEvent*/

public abstract class DurationFightEvent : IDisposable
{
    public uint ActivationTime {get; }
    public uint ResolveTime {get; }
    protected uint Seed {get; }

    public DurationFightEvent(uint _activationTime, uint _resolveTime, uint _seed)
    {
        ActivationTime = _activationTime;
        ResolveTime = _resolveTime;
        Seed = _seed;
    }

    public abstract void OnFrameWorkTick(IFramework framework);

    public void Dispose()
    {
        Plugin.Framework.Update -= this.OnFrameWorkTick;
    }
}

