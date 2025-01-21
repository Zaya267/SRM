using Interface.FileMovement.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace Interface.FileMovement.Services
{
    public class FileValidator : IFileValidator
    {
        public bool IsLocked(string filePath)
        {
            try
            {
                using FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                file.Close();
                return false;
            }
            catch
            {
                return true;
            }
        }

        public bool WriteComplete(DateTime lastWriteTime)
        {
            var lastWriteMinutes = 1;
            var currentTime = DateTime.Now.Subtract(lastWriteTime).TotalMinutes;

            // sftp.sanlam.co.za adds extra two hours on LastWriteTime
            if (currentTime < 0)
                currentTime = DateTime.Now.AddHours(2).Subtract(lastWriteTime).TotalMinutes;

            return currentTime > lastWriteMinutes;
        }

        public bool FileMaskMatch(string filename, string fileMask, string antiFileMask)
        {
            return ValidateMask(filename, fileMask) && !ValidateMask(filename, antiFileMask);
        }

        public bool ValidateMask(string filename, string fileMask)
        {
            if (string.IsNullOrEmpty(fileMask))
                return false;

            return fileMask.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Any(x => filename.Contains(x));
        }

        public bool IsFileValid(FileInfo fileInfo, string fileMask, string antiFileMask)
        {
            return !IsLocked(fileInfo.FullName) && WriteComplete(fileInfo.LastWriteTime) && FileMaskMatch(fileInfo.Name, fileMask, antiFileMask);
        }
    }
}