using Ironbug.Core;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    /// <summary>
    /// Cache OpenStudio object Handle IDs along with Ironbug tracking IDs. 
    /// Only the following types of objects are cached:
    ///     - IIB_DualLoopObj
    ///     - IB_ScheduleRuleset
    ///     _ IB_NodeProbe (OpenStudio.Node)
    /// </summary>
    internal static class OpsIDMapper
    {
        private static Dictionary<string, OpenStudio.UUID> _mapper;
        private static bool _isRecording;

        public static void StartRecording()
        {
            _mapper = new Dictionary<string, OpenStudio.UUID>();
            _isRecording = true;
        }

        public static void TryAdd(string trackingID, OpenStudio.UUID uid)
        {
            if (!_isRecording) return;
            _mapper.TryAdd(trackingID, uid);
        }

        public static bool TryGetValue(string trackingID, out OpenStudio.UUID uid)
        {
            uid = null;
            if (!_isRecording) return false;

            return _mapper.TryGetValue(trackingID, out uid);

        }
      



    }
}
