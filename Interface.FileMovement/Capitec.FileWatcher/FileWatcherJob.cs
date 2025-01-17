using Interface.FileMovement.Database;
using Interface.FileMovement.Interfaces;
using Interface.FileMovement.Models;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Interface.FileMovement
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class FileWatcherJob : IJob
    {
        private readonly IEmail emailService;
        private readonly ISettingsConfig settingsConfig;
        private readonly IDatabase databaseService;
        private readonly IFileArchive fileArchive;
        private readonly IFileValidator fileValidator;

        public FileWatcherJob(IEmail emailService, ISettingsConfig settingsConfig, IDatabase databaseService, IFileArchive fileArchive, IFileValidator fileValidator)
        {
            this.emailService = emailService;
            this.settingsConfig = settingsConfig;
            this.databaseService = databaseService;
            this.fileArchive = fileArchive;
            this.fileValidator = fileValidator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var fileMovementSettings = await databaseService.GetFileMovementSettings();
                var mailOptions = new MailOptions();

                foreach (var settings in fileMovementSettings)
                {
                    try
                    {
                        mailOptions = settingsConfig.GetMailOptions(settings);
                        ProcessFiles(settings);
                    }
                    catch (Exception ex)
                    {
                        databaseService.LogError(settings.Id, ex.Message, ex.Source);
                        emailService.Error(mailOptions, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Service Error.");
            }
        }

        public void ProcessFiles(FileMovementSetting setting)
        {
            IEnumerable<FileInfo> localFiles = new DirectoryInfo(setting.SourcePath).EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);

            if (localFiles.Any())
            {
                foreach (var currentFile in localFiles)
                {
                    MoveFiles(setting, currentFile);
                }
            }
        }

        private void MoveFiles(FileMovementSetting setting, FileInfo currentFile)
        {
            if (File.Exists(currentFile.FullName) && fileValidator.IsFileValid(currentFile, setting.FileMask, setting.AntiFileMask))
            {
                var recordExist = databaseService.RecordExits(currentFile);
                if (recordExist)
                {
                    fileArchive.ArchiveOnly(currentFile, setting.ArchivePath);
                    databaseService.LogError(setting.Id, null, $"{currentFile.Name} is a duplicate.");
                }
                else
                {
                    if (string.IsNullOrEmpty(setting.CopyPath))
                    {
                        fileArchive.ArchiveOnly(currentFile, setting.ArchivePath);
                    }
                    else
                    {
                        fileArchive.CopyAndArchive(currentFile, setting.CopyPath, setting.ArchivePath);
                    }
                    databaseService.LogProcessed(setting.Id, currentFile.Name, currentFile.LastWriteTime);
                }
            }
        }
    }
}