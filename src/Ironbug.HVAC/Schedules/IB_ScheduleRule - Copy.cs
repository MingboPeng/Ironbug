using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.IO;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleFile : IB_Schedule
    {
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleFile(this._FilePath);

        private static ScheduleFile InitMethod(Model model, string path)
            => new ScheduleFile(ExternalFile.getExternalFile(model, path).get());

        private string _FilePath;

        public IB_ScheduleFile(string FilePath) : base(InitMethod(new Model(), FilePath))
        {
            this._FilePath = FilePath;
        }

        
        public override ModelObject ToOS(Model model)
        {
            if (!File.Exists(this._FilePath))
            {
                throw new ArgumentException($"{this._FilePath} does not exit!");
            }

            return base.OnNewOpsObj((m)=>new ScheduleFile(ExternalFile.getExternalFile(m, this._FilePath).get()), model);



        }

        //public ScheduleFile ToOS(ScheduleFileset Ruleset)
        //{
        //    var model = Ruleset.model();
        //    this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
        //    var name = $"ScheduleFile - {trackingId.ToString().Substring(12)}";

           
        //    //There is a bug in ScheduleFile when it is initialized with ScheduleDay, it recreates a new ScheduleDay
        //    var day = this.ScheduleDay.ToOS(new Model()) as ScheduleDay;
        //    var obj = new ScheduleFile(Ruleset, day);
        //    obj.setName(name);
        //    obj.SetCustomAttributes(this.CustomAttributes);
        //    return obj;
        //}
        
    }
    public sealed class IB_ScheduleFile_FieldSet
    : IB_FieldSet<IB_ScheduleFile_FieldSet, ScheduleFile>
    {
        private IB_ScheduleFile_FieldSet() { }

    }
}