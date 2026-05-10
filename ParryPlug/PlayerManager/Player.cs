using System.Numerics;
using Dalamud.Game.ClientState.Party;

public class Player
{
    public string Name {get; private set;}
    public string Job {get; private set;}
    public string Role {get; private set;}
    public Vector3 Position {get; set;}
    public uint CurrentHP {get; set;} 
    public uint MaxHP {get; set;} // even max health needs to be updateable.
    
    public uint EntityID {get; set;}

    public Player(IPartyMember member)
    {       
        var classJob = member.ClassJob.Value;

        this.Name = member.Name.ToString();
        this.Job = classJob.Abbreviation.ToString() ?? "Error";
        this.Role = classJob.Role switch
        {
            1 => "Tank",
            2 => "Melee DPS",
            3 => "Ranged DPS",
            4 => "Healer",
            _ => "Unknown"
        };

        this.Position = member.Position;
        this.CurrentHP = member.CurrentHP;
        this.MaxHP = member.MaxHP;

        this.EntityID = member.EntityId;
    }

    public void UpdateInfo(IPartyMember member)
    {
        this.Position = member.Position;
        this.CurrentHP = member.CurrentHP;
        this.MaxHP = member.MaxHP;
    }
}