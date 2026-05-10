using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Dalamud.Game.Player;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs;
using Lumina.Excel.Sheets;
namespace ParryPlug;
public class PlayerPicker{
    RandomNumberGenerator rng;
    PartyInfo partyInfo;
    public delegate List<Player> PickPlayers();
    private Dictionary<string, PickPlayers> pickStrategies;
    public PlayerPicker(PartyInfo _partyInfo, RandomNumberGenerator _rng)
    {
        this.partyInfo = _partyInfo;
        this.rng = _rng;
        this.pickStrategies = new Dictionary<string, PickPlayers>
        {
            {"All", All},
            {"AllSupports", AllSupports},
            {"AllDPS", AllDPS},
            {"AllTanks", AllTanks},
            {"AllHealers", AllHealers},
            {"SupportPairs", SupportPairs},
            {"DPSPairs", DPSPairs},
            {"AllPairs", AllPairs},
            {"XTankDPSPairs", XTankDPSPairs},
            {"XHealerDPSPairs", XHealerDPSPairs},
            {"XTankHealerPairs", XTankHealerPairs},
            {"XRolePairs", XRolePairs}
        };
    }
    public List<Player> Pick(string chosenStrategy)
    {
        if (this.pickStrategies.TryGetValue(chosenStrategy, out var chosenFunction))
            return chosenFunction();
        
        return new List<Player>();
    }
    
    // Shuffle algorithm based on Fisher-Yates shuffle https://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619
    private List<Player> ShufflePlayers(IList<Player> players)
    {
        int n = players.Count;
        while (n > 1) {
            n--;
            int k = rng.Next((uint)n + 1);
            Player value = players[k];
            players[k] = players[n];
            players[n] = value;
        }
        return players.ToList();
    }

    private List<Player> All()
    {
        return ShufflePlayers(partyInfo.Get().ToList());
    }
    // Role Groups
    private List<Player> AllSupports()
    {
        return ShufflePlayers(partyInfo.Get().Where(p => p.Role == Player.Roles.Healer || p.Role == Player.Roles.Tank).ToList());
    }
    private List<Player> AllDPS()
    {
        return ShufflePlayers(partyInfo.Get().Where(p => p.Role == Player.Roles.DPS).ToList());
    }
    private List<Player> AllTanks()
    {
        return ShufflePlayers(partyInfo.Get().Where(p => p.Role == Player.Roles.Tank).ToList());
    }
    private List<Player> AllHealers()
    {
        return ShufflePlayers(partyInfo.Get().Where(p => p.Role == Player.Roles.Healer).ToList());
    }
    // Pairs
    private List<Player> SupportPairs()
    {
        return AllSupports();
    }
    private List<Player> DPSPairs()
    {
        return AllDPS();
    }
    private List<Player> AllPairs()
    {
        return All();
    }
    // Role Pairs
    private List<Player> XTankDPSPairs()
    {
        var result = new List<Player>();
        List<Player> groupTanks = AllTanks();
        List<Player> groupDPS = AllDPS();
        for (int i = 0; i < Math.Min(groupTanks.Count, groupDPS.Count); i++)
        {
            result.Add(groupTanks[i]);
            result.Add(groupDPS[i]);
        }
        return result;
    }
    private List<Player> XHealerDPSPairs()
    {
        var result = new List<Player>();
        List<Player> groupHealers = AllHealers();
        List<Player> groupDPS = AllDPS();
        for (int i = 0; i < Math.Min(groupHealers.Count, groupDPS.Count); i++)
        {
            result.Add(groupHealers[i]);
            result.Add(groupDPS[i]);
        }
        return result;
    }
    private List<Player> XTankHealerPairs()
    {
        var result = new List<Player>();
        List<Player> groupTanks = AllTanks();
        List<Player> groupHealers = AllHealers();
        for (int i = 0; i < Math.Min(groupTanks.Count, groupHealers.Count); i++)
        {
            result.Add(groupTanks[i]);
            result.Add(groupHealers[i]);
        }
        return result;
    }
    
    private List<Player> XRolePairs()
    {
        var result = new List<Player>();
        List<Player> groupSupports = AllSupports();
        List<Player> groupDPS = AllDPS();
        for (int i = 0; i < Math.Min(groupSupports.Count, groupDPS.Count); i++)
        {
            result.Add(groupSupports[i]);
            result.Add(groupDPS[i]);
        }
        return result;
    }
}