using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Arrays;
using Microsoft.VisualBasic;

namespace ParryPlug;

public class PartyInfo : IDisposable
{
    public List<Player> partyInfo = new();
    public PartyInfo()
    {
        this.Build();
        Plugin.Framework.Update += Update;
    }

    public List<Player> Get()
    {
        return partyInfo;
    }

    public void Build()
    {
        partyInfo = Plugin.PartyList.Select(member => new Player(member)).ToList();
    }

    public void Update(IFramework framework)
    {
        foreach (Player player in partyInfo)
        {
            var member = Plugin.PartyList.FirstOrDefault(member => member.EntityId == player.EntityID);
            if (member == null) continue;
            player.UpdateInfo(member);
        }
    }

    public virtual void Dispose()
    {
        Plugin.Framework.Update -= Update;
    }
}