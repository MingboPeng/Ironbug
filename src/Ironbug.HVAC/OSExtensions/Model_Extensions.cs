using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public static class Model_Extensions
    {
        public static bool Save(this Model model, string filePath)
        {
            return model.save(new Path(filePath), true);

        }

        public static void IDD(this IddObject idd)
        {
            
        }
    }
}
