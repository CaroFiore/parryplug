using System;
using System.Collections.Generic;
using Dalamud.Interface.Internal.UiDebug.Browsing;
using Dalamud.Plugin.Services;
using ParryPlug;

public class EventScheduler : IDisposable
{
    public EventScheduler()
    {
        Plugin.Framework.Update += this.OnFrameWorkTick;
    }

    public void OnFrameWorkTick(IFramework framework)
    {
       

    }
    
    public void Dispose()
    {

        
    }
}