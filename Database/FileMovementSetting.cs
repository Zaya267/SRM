using System.Collections.Generic;

#nullable disable

namespace Interface.FileMovement.Database
{
    public partial class FileMovementSetting
    {
        public FileMovementSetting()
        {
            FileMovementErrors = new HashSet<FileMovementError>();
            FileMovementHistories = new HashSet<FileMovementHistory>();
        }

        public int Id { get; set; }
        public string SourcePath { get; set; }
        public string CopyPath { get; set; }
        public string ArchivePath { get; set; }
        public string FileMask { get; set; }
        public string AntiFileMask { get; set; }
        public string Description { get; set; }
        public string MailTo { get; set; }
        public string Mailcc { get; set; }
        public string MailBcc { get; set; }
        public string ModifiedBy { get; set; }
        public bool? Enabled { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<FileMovementError> FileMovementErrors { get; set; }
        public virtual ICollection<FileMovementHistory> FileMovementHistories { get; set; }
    }
}
