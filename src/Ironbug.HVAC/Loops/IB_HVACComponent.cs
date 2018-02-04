﻿using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public interface IIB_HVACComponent
    {
        HVACComponent GetHVACComponent();
        bool AddToNode(ref Model model, Node node);
    }

    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
