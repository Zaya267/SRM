using System.IO;

namespace Interface.FileMovement.Interfaces
{
    public interface IFileArchive
    {
        void ArchiveOnly(FileInfo sourceFilePath, string archivePath);

        void CopyAndArchive(FileInfo sourceFile, string destinationPath, string archivePath);
    }
}