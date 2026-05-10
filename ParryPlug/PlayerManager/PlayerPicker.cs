using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private List<Player> eligiblePlayers;
    public PlayerPicker(PartyInfo _partyInfo, RandomNumberGenerator _rng)
    {
        this.partyInfo = _partyInfo;
        this.rng = _rng;
        this.eligiblePlayers = new();

        this.pickStrategies = new Dictionary<string, PickPlayers>
        {
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
        
        //If fails, returns empty list
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

    //Role Groups
    List<Player> AllSupports(){
        eligiblePlayers = partyInfo.Get().Where(p => p.Role == Player.Roles.Healer || p.Role == Player.Roles.Tank).ToList();
        return ShufflePlayers(eligiblePlayers);
    }
    List<Player> AllDPS(){ 
        eligiblePlayers = partyInfo.Get().Where(p => p.Role == Player.Roles.DPS).ToList();
        return ShufflePlayers(eligiblePlayers);
    }
    List<Player> AllTanks()
    {
        eligiblePlayers = partyInfo.Get().Where(p => p.Role == Player.Roles.Tank).ToList();
        return ShufflePlayers(eligiblePlayers);
    }
    List<Player> AllHealers()
    {
        eligiblePlayers = partyInfo.Get().Where(p => p.Role == Player.Roles.Healer).ToList();
        return ShufflePlayers(eligiblePlayers);
    }
    //Pairs
    List<Player> SupportPairs(){return new List<Player>();}
    List<Player> DPSPairs(){return new List<Player>();}
    List<Player> AllPairs(){return new List<Player>();}
    // Role Pairs
    List<Player> XTankDPSPairs(){return new List<Player>();}
    List<Player> XHealerDPSPairs(){return new List<Player>();}
    List<Player> XTankHealerPairs(){return new List<Player>();}
    List<Player> XRolePairs(){return new List<Player>();}
}