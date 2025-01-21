using System;
using System.IO;

namespace Interface.FileMovement.Interfaces
{
    public interface IFileValidator
    {
        bool IsLocked(string filePath);

        bool WriteComplete(DateTime lastWriteTime);

        bool FileMaskMatch(string filename, string fileMask, string antiFileMask);

        bool ValidateMask(string filename, string fileMask);

        bool IsFileValid(FileInfo fileInfo, string fileMask, string antiFileMask);
    }
}