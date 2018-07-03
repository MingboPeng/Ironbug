using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class IB_SettingComponentAttributes : GH_ComponentAttributes
    {
        public IB_SettingComponentAttributes(GH_Component component):base(component)
        {
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (this.Owner is Ironbug_ObjParams objParams)
            {
                objParams.RespondToMouseDoubleClick();
            }
            else if (this.Owner is Ironbug_OutputParams outputParams)
            {
                outputParams.RespondToMouseDoubleClick();
            }
            return base.RespondToMouseDoubleClick(sender, e);   
        }

    }
    
}
