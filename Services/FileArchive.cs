using Interface.FileMovement.Interfaces;
using System;
using System.IO;

namespace Interface.FileMovement.Services
{
    public class FileArchive : IFileArchive
    {
        public void ArchiveOnly(FileInfo sourceFilePath, string archivePath)
        {
            try
            {
                ArchiveFile(sourceFilePath, archivePath);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(ex.Message);
            }
        }

        public void CopyAndArchive(FileInfo sourceFile, string destinationPath, string archivePath)
        {
            try
            {
                if (File.Exists(sourceFile.FullName))
                {
                    var destinationFile = Path.Combine(destinationPath, sourceFile.Name);
                    if (!File.Exists(destinationFile))
                    {
                        File.Copy(sourceFile.FullName, destinationFile);
                    }
                    ArchiveFile(sourceFile, archivePath);
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(ex.Message);
            }
        }

        private static void ArchiveFile(FileInfo sourceFilePath, string archivePath)
        {
            var archiveFile = Path.Combine(archivePath, sourceFilePath.Name);
            if (File.Exists(sourceFilePath.FullName))
            {
                if (File.Exists(archiveFile))
                {
                    File.Delete(archiveFile);
                }
                File.Move(sourceFilePath.FullName, archiveFile);
            }
        }
    }
}