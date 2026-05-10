using System.Collections.Generic;

namespace ParryPlug;

public class PlayerPicker{
    List<Player> partyInfo;
    public delegate List<Player> PickPlayers();
    private Dictionary<string, PickPlayers> pickStrategies;

    public PlayerPicker(List<Player> _partyInfo)
    {
        this.partyInfo = _partyInfo;

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
    
    //Role Groups
    List<Player> AllSupports(){return new List<Player>();}
    List<Player> AllDPS(){return new List<Player>();}
    List<Player> AllTanks(){return new List<Player>();}
    List<Player> AllHealers(){return new List<Player>();}
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