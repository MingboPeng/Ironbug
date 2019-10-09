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

        //private static Type _refOsType;

        //public static Type GetRefOsType()
        //{
        //    if (_refOsType != null) return _refOsType;

        //    var v0 = typeof(Model).Assembly.GetName().Version;
        //    var v1 = new System.Version("2.8.0");
        //    var isOldVersion = v0.CompareTo(v1) < 0;

        //    if (!isOldVersion)
        //    {
        //        _refOsType = typeof(Model).Assembly.GetType("OpenStudio.ScheduleFile");
        //        return _refOsType;
        //    }
        //    else
        //    {
        //        return typeof(Node);
        //        //throw new ArgumentException("OpenStudio.ScheduleFile is only supported in 2.8 or newer version!");
        //    }
            
        //}
        private static ScheduleFile InitMethod(Model model, string path)
        {
            var w = model.workflowJSON();
            if (w.oswDir().__str__().EndsWith(@"System"))
            {
                var tempPath = System.IO.Path.GetTempPath() + @"\Ladybug\HVAC";
                Directory.CreateDirectory(tempPath);
                w.setOswDir(OpenStudioUtilitiesCore.toPath(tempPath));
            }

            var extFile = ExternalFile.getExternalFile(model, path);
            if (extFile.is_initialized())
            {
                var obj = new ScheduleFile(extFile.get());
                return obj;
            }
           
            throw new ArgumentException($"Invalid file path for ScheduleFile! \n{path}\n{w.oswDir().__str__()}");
        }
        //private static ScheduleInterval InitMethod(Model model, string path)
        //    => (ScheduleInterval)Activator.CreateInstance(GetRefOsType(), new Object[] { ExternalFile.getExternalFile(model, path).get() }); 

        private string _FilePath;

        public IB_ScheduleFile(string FilePath) : base(InitMethod(new Model(), FilePath))
        {
            //if (_refOsType == typeof(Node))
            //{
            //    throw new ArgumentException("OpenStudio.ScheduleFile is only supported in 2.8 or newer version!");
            //}
            if (!File.Exists(FilePath))
            {
                throw new ArgumentException($"{this._FilePath} does not exit!");
            }
            this._FilePath = FilePath;
        }

        
        public override ModelObject ToOS(Model model)
        {
            return InitMethod(model, this._FilePath);
            //return base.OnNewOpsObj((m) => new ScheduleFile(ExternalFile.getExternalFile(m, this._FilePath).get()), model);

        }

        
    }
    public sealed class IB_ScheduleFile_FieldSet
    : IB_FieldSet<IB_ScheduleFile_FieldSet>
    {
        private IB_ScheduleFile_FieldSet() { }

        //public IB_Field ScheduleTypeLimits { get; }
        //    = new IB_BasicField("ScheduleTypeLimits", "ScheduleTypeLimits") { };
    }
}