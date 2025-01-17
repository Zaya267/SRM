using Interface.FileMovement.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Interface.FileMovement.Interfaces
{
    public interface IDatabase
    {
        void LogProcessed(int id, string fileName, DateTime dateCreated);

        void LogError(int id, string errorSource, string errorDescription);

        Task<List<FileMovementSetting>> GetFileMovementSettings();

        bool RecordExits(FileInfo fileInfo);
    }
}
