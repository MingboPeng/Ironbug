using OpenStudio;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public static class IddObject_Extensions
    {
        public static IEnumerable<IddField> GetIddFields(this IddObject iddObject)
        {
            return iddObject
                .nonextensibleFields()
                .Where(_ => _.IsWorkableField());

        }

    }
    public static class IddField_Extensions
    {
        public static bool IsWorkableField(this IddField iddField)
        {
            //https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-004.html
            //!Type of data for the field -
            //!integer
            //!real
            //!alpha    (arbitrary string),
            //!choice   (alpha with specific list of choices, see
            //!                                 \key)
            //!object-list  (link to a list of objects defined elsewhere,
            //!             see \object - list and \ reference)
            //!external-list    (uses a special list from an external source,
            //!             see \external - list)
            //!node         (name used in connecting HVAC components)

            var dataType = iddField.properties().type.valueDescription();
            var name = iddField.name().ToLower();
            var result = dataType.ToLower() != "object-list" ? true : !name.Contains("node");
            result &= dataType != "node";
            result &= dataType != "external-list";
            result &= dataType != "handle";
            return result;
            
        }

        public static bool IsRealType(this IddField iddField)
        {
            return iddField.properties().type.valueDescription() == "real";
        }

    }
}
