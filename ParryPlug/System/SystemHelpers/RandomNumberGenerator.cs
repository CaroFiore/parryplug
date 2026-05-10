using System;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace ParryPlug;

public class RandomNumberGenerator
{
    private uint state;

    public RandomNumberGenerator(uint _seed = 0)
    {
        Plugin.Log.Information($"Constructor: RandomNumberGenerator with seed: {_seed}");
        state = _seed;
    }


    public int Next(uint max)
    {
        //Mulberry32 PRNG
        state += 0x6D2B79F5;
        uint t = (state ^ (state >> 15)) * (1 | state);
        t = (t + (t ^ (t >> 7)) * (61 | t)) ^ t;
        t = t ^ (t >> 14);
        return (int)(t % max); // 0–7
    }
}