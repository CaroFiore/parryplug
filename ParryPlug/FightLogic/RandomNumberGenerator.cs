using System.Runtime.CompilerServices;
using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;

public class RandomNumberGenerator(uint _seed = 0, int _max = 1)
{
    private uint state = _seed;
    private int max = _max;

    public int Next()
    {
        //Mulberry32 PRNG
        state += 0x6D2B79F5;
        uint t = (state ^ (state >> 15)) * (1 | state);
        t = (t + (t ^ (t >> 7)) * (61 | t)) ^ t;
        t = t ^ (t >> 14);
        return (int)(t % (uint)this.max); // 0–7
    }

    public void NextSeed()
    {
        this.state++;
    }

    public void UpdateMax(int _max)
    {
        this.max = _max;
    }
}