using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.IO;
using System.Linq;

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
            var tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Ironbug", "files");
            if (File.Exists(path))
            { 
                //Copy to temp folder
                var targetFile =  System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileName(path));
                File.Copy(path, targetFile, true);
            }

            // Check if temp folder exist in current workflow paths
            var workflow = model.workflowJSON();
            var Paths = workflow.absoluteFilePaths().Select(_=>_.__str__());
            if (!Paths.Contains(tempFolder))
            {
                workflow.addFilePath(OpenStudioUtilitiesCore.toPath(tempFolder));
            }

            var extFile = ExternalFile.getExternalFile(model, path);

            var paths = model.workflowJSON().filePaths()[0].__str__();

            if (extFile.is_initialized())
            {
                var obj = new ScheduleFile(extFile.get());
                return obj;
            }
           
            throw new ArgumentException($"Invalid file path for ScheduleFile! \n{path}\n{workflow.oswDir().__str__()}");
        }
        //private static ScheduleInterval InitMethod(Model model, string path)
        //    => (ScheduleInterval)Activator.CreateInstance(GetRefOsType(), new Object[] { ExternalFile.getExternalFile(model, path).get() }); 

        private string _FilePath;

        private IB_ScheduleFile() : base(null) { }
        public IB_ScheduleFile(string FilePath) : base(InitMethod(new Model(), FilePath))
        {
            //if (_refOsType == typeof(Node))
            //{
            //    throw new ArgumentException("OpenStudio.ScheduleFile is only supported in 2.8 or newer version!");
            //}
         
            this._FilePath = FilePath;
        }

        
        public override ModelObject ToOS(Model model)
        {
            return base.OnNewOpsObj((m) => InitMethod(m, this._FilePath), model);

        }

        public override string ToJson(bool indented = false)
        {
            throw new ArgumentException($"{this.GetType().Name} is not supported to be exported to Json now");
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