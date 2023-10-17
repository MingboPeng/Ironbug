using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleFile : IB_Schedule
    {
        private string _filePath { get => this.Get(string.Empty); set => this.Set(value); }
        private string _fileContent { get => this.Get(string.Empty); set => this.Set(value); }
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleFile(this._filePath);

        private static ScheduleFile InitMethod(Model model, string path)
        {
            
            if (!File.Exists(path))
                throw new ArgumentException($"Invalid file path for ScheduleFile! \n{path}");

            var tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Ironbug", "files");
            Directory.CreateDirectory(tempFolder);

            //Copy to temp folder
            var targetFile = System.IO.Path.Combine(tempFolder, System.IO.Path.GetFileName(path));
            File.Copy(path, targetFile, true);

            var obj = new ScheduleFile(model, OpenStudioUtilitiesCore.toPath(targetFile));
            return obj;
        }
    

        private IB_ScheduleFile() : base(null) { }
        public IB_ScheduleFile(string filePath) : base(InitMethod(new Model(), filePath))
        {
            this._filePath = filePath;
        }

        
        public override ModelObject ToOS(Model model)
        {
            if (!System.IO.File.Exists(this._filePath))
            {
                this._filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{System.IO.Path.GetRandomFileName()}.csv");
                System.IO.File.WriteAllText(this._filePath, this._fileContent);
            }

            return base.OnNewOpsObj((m) => InitMethod(m, this._filePath), model);

        }

        public override string ToJson(bool indented = false)
        {
            if (System.IO.File.Exists(this._filePath))
            {
                this._fileContent = System.IO.File.ReadAllText(this._filePath);
            }

            return base.ToJson(indented); 
        }


        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (System.IO.File.Exists(this._filePath))
            {
                this._fileContent = System.IO.File.ReadAllText(this._filePath);
            }
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