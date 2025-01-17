using Interface.FileMovement.Database;
using Interface.FileMovement.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Interface.FileMovement.Services
{
    public class Database : IDatabase
    {
        private readonly FileWatcherContext watcherContext;

        public Database()
        {
            this.watcherContext = new FileWatcherContext();
        }

        public async Task<List<FileMovementSetting>> GetFileMovementSettings()
        {
            var storeProcedure = $"EXEC sps_FileMovement_Settings";
            var results = await watcherContext.FileMovementSettings.FromSqlRaw(storeProcedure).ToListAsync();
            return results;
        }

        public void LogError(int id, string errorSource, string errorDescription)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                 new SqlParameter("@ErrorSource", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(errorSource) ? string.Empty : errorSource },
                 new SqlParameter("@ErrorDescription", SqlDbType.VarChar) { Value = errorDescription },
                 new SqlParameter("@Date", SqlDbType.DateTime) { Value = DateTime.Now },
                 new SqlParameter("@FileMovement_Settings_ID", SqlDbType.Int) { Value = id },
            };

            var storeProcedure = $"EXEC spi_FileMovement_Error @ErrorSource, @ErrorDescription, @Date, @FileMovement_Settings_ID";

            watcherContext.Database.ExecuteSqlRaw(storeProcedure, sqlParameters);
        }

        public void LogProcessed(int id, string fileName, DateTime dateCreated)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                 new SqlParameter("@FileName", SqlDbType.VarChar) { Value = fileName },
                 new SqlParameter("@DateCreated", SqlDbType.DateTime) { Value = dateCreated},
                 new SqlParameter("@DateMoved", SqlDbType.DateTime) { Value = DateTime.Now},
                 new SqlParameter("@FileMovement_Settings_ID", SqlDbType.Int) { Value = id },
            };

            var storeProcedure = $"EXEC spi_FileMovement_History @FileName, @DateCreated, @DateMoved, @FileMovement_Settings_ID";

            watcherContext.Database.ExecuteSqlRaw(storeProcedure, sqlParameters);
        }

        public bool RecordExits(FileInfo fileInfo)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                 new SqlParameter("@FileName", SqlDbType.VarChar) { Value = fileInfo.Name },
                 new SqlParameter("@DateCreated", SqlDbType.DateTime) { Value = fileInfo.LastWriteTime },
                 new SqlParameter("@Count", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            var storeProcedure = $"EXEC sps_FileMovement_CheckProcessed @FileName, @DateCreated, @Count OUTPUT";

            watcherContext.Database.ExecuteSqlRaw(storeProcedure, sqlParameters);

            var databaseRecords = int.Parse(sqlParameters[2].Value.ToString());
            const int emptyRecord = 0;

            return databaseRecords != emptyRecord;
        }
    }
}