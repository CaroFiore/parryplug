using System.Collections.Generic;
using System.Linq;
using Dalamud.Bindings.ImGui;
using FFXIVClientStructs.FFXIV.Client.UI.Arrays;
using Microsoft.VisualBasic;

namespace ParryPlug;

public class PartyInfo
{
    public List<Player> partyInfo = new();
    public PartyInfo()
    {
        this.Build();
    }

    public List<Player> Get()
    {
        return partyInfo;
    }

    public void Build()
    {
        partyInfo = Plugin.PartyList.Select(member => new Player(member)).ToList();
    }

    public void Update()
    {
        foreach (Player player in partyInfo)
        {
            var member = Plugin.PartyList.FirstOrDefault(member => member.EntityId == player.EntityID);
            if (member == null) continue;
            player.UpdateInfo(member);
        }
    }
}