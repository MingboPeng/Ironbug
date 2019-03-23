using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class IB_SettingComponentAttributes : IB_ComponentAttributes
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
            return base.RespondToMouseDoubleClick(sender, e);   
        }

    }
    
}
