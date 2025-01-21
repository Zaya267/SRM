using System;

#nullable disable

namespace Interface.FileMovement.Database
{
    public partial class FileMovementHistory
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateMoved { get; set; }
        public int FileMovementSettingsId { get; set; }

        public virtual FileMovementSetting FileMovementSettings { get; set; }
    }
}
