using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Game.ClientState.Party;
using Lumina.Excel.Sheets;

public class Player
{
    public enum Roles
    {
        Tank,Healer,DPS
    }

public enum Jobs 
{
    PLD, GLA, WAR, MRD, DRK, GNB, WHM, CNJ, SCH, ACN, AST, SGE, MNK, PGL, DRG, LNC, NIN, ROG, SAM, RPR, VPR, BRD, ARC, MCH, DNC, BLM, THM, SMN, RDM, PCT, BLU
}

static readonly Dictionary<Jobs, Roles> JobRoles = new()
{
    { Jobs.PLD, Roles.Tank }, { Jobs.GLA, Roles.Tank },
    { Jobs.WAR, Roles.Tank }, { Jobs.MRD, Roles.Tank },
    { Jobs.DRK, Roles.Tank },
    { Jobs.GNB, Roles.Tank },

    { Jobs.WHM, Roles.Healer }, { Jobs.CNJ, Roles.Healer },
    { Jobs.SCH, Roles.Healer }, { Jobs.ACN, Roles.DPS },
    { Jobs.AST, Roles.Healer },
    { Jobs.SGE, Roles.Healer },

    { Jobs.MNK, Roles.DPS }, { Jobs.PGL, Roles.DPS },
    { Jobs.DRG, Roles.DPS }, { Jobs.LNC, Roles.DPS },
    { Jobs.NIN, Roles.DPS }, { Jobs.ROG, Roles.DPS },
    { Jobs.SAM, Roles.DPS },
    { Jobs.RPR, Roles.DPS },
    { Jobs.VPR, Roles.DPS },

    { Jobs.BRD, Roles.DPS }, { Jobs.ARC, Roles.DPS },
    { Jobs.MCH, Roles.DPS },
    { Jobs.DNC, Roles.DPS },

    { Jobs.BLM, Roles.DPS }, { Jobs.THM, Roles.DPS },
    { Jobs.SMN, Roles.DPS },
    { Jobs.RDM, Roles.DPS },
    { Jobs.PCT, Roles.DPS },
    { Jobs.BLU, Roles.DPS }
};


    public string Name {get; private set;}
    public Jobs Job {get; private set;}
    public Roles Role {get; private set;}
    public Vector3 Position {get; set;}
    public uint CurrentHP {get; set;} 
    public uint MaxHP {get; set;} // even max health needs to be updateable.
    
    public uint EntityID {get; set;}

    public Player(IPartyMember member)
    {       
        var classJob = member.ClassJob.Value;

        this.Name = member.Name.ToString();
        this.Job = Enum.Parse<Jobs>(classJob.Abbreviation.ToString());
        this.Role = JobRoles.GetValueOrDefault(this.Job);

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

    public string PrintPlayerInfo()
    {
        return $"{EntityID} {Name} {Role} {Job} {CurrentHP}/{MaxHP} {Position}";
    }
}