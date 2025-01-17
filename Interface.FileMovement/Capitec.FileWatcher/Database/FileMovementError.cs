using System;

#nullable disable

namespace Interface.FileMovement.Database
{
    public partial class FileMovementError
    {
        public int Id { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime Date { get; set; }
        public int FileMovementSettingsId { get; set; }

        public virtual FileMovementSetting FileMovementSettings { get; set; }
    }
}
