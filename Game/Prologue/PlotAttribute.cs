using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    public class PlotAttribute : ReflectionAttribute
    {
        public PlotAttribute(EPlotType plotType) : base(typeof(IPlotHandler), plotType, 100)
        {
        }
    }
}