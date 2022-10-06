using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.PricingSources.Processors;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.DataAccess
{
    public class FileFilter
    {
        public void UpdateDownloadStatus(List<PendingFile> files)
        {
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var file in files.Where(t => t.IsSaved && t.IsDownloaded))
                        {
                            var sqlQuery = $"UPDATE [dbo].[ExternalPricingDataFiles] SET [StatusId] = 1, DownloadedOn = @DownloadDate WHERE [FileName] = @FileName";

                            var sqlCommand = sqlConnection.CreateCommand();
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.CommandText = sqlQuery;
                            sqlCommand.CommandType = System.Data.CommandType.Text;

                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@FileName", file.Name));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@DownloadDate", DateTimeOffset.Now));
                            sqlCommand.ExecuteNonQuery();
                        }
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        LogManager.Logger.WriteException("FileFilter", "UpdateFilesStatus", ex.Message, ex);
                    }
                }
            }
        }
        public void UpdateStatusToProcessed(List<int> files)
        {
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var id in files)
                        {
                            var sqlQuery = $"UPDATE [dbo].[ExternalPricingDataFiles] SET [StatusId] = 2, ProcessedOn = @ProcessedDate WHERE Id = @Id";

                            var sqlCommand = sqlConnection.CreateCommand();
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.CommandText = sqlQuery;
                            sqlCommand.CommandType = System.Data.CommandType.Text;

                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Id", id));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProcessedDate", DateTimeOffset.Now));
                            sqlCommand.ExecuteNonQuery();
                        }
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        LogManager.Logger.WriteException("FileFilter", "UpdateFilesStatus", ex.Message, ex);
                    }
                }
            }
        }
        private List<string> CheckAndGetFiles(string fileNames)
        {
            var result = new List<string>();
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "usp_GetPendingFiles";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@Files", fileNames));
                sqlConnection.Open();
                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fileName = reader["FileName"].ToString();
                        result.Add(fileName);
                    }
                }
            }
            return result;
        }
        public List<PendingFile> GetPendingFiles(List<string> files)
        {
            var result = new List<PendingFile>();
            try
            {
                var pendingFiles = new List<string>();
                var fileNames = files.Select(t => new { Name = Path.GetFileName(t), Path = Path.GetDirectoryName(t) }).ToList();
                if (fileNames.Any())
                {
                    var fileNameString = string.Join(",", fileNames.Select(t => t.Name));
                    pendingFiles = CheckAndGetFiles(fileNameString);
                }
                result = fileNames.Where(t => pendingFiles.Contains(t.Name))
                                    .Select(t => new PendingFile
                                    {
                                        Name = t.Name,
                                        Path = t.Path
                                    }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FileFilter", "GetPendingFiles", "files: => " + string.Join(",", files) + ex.Message, ex);
                LogManager.Logger.WriteDebug("FileFilter", "GetPendingFiles", "Service Type: Pricing Sources");
            }
            return result;
        }
        public List<PendingFile> SavePendingFiles(List<PendingFile> pendingFiles)
        {
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var file in pendingFiles)
                        {
                            var sqlQuery = $"INSERT INTO [dbo].[ExternalPricingDataFiles] (FileName,RemotePath,CreatedOn,DownloadedOn,StatusId) "
                                            + "VALUES (@FileName,@RemotePath,@CreatedOn,@DownloadedOn,@StatusId);SELECT SCOPE_IDENTITY();";

                            var sqlCommand = sqlConnection.CreateCommand();
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.CommandText = sqlQuery;
                            sqlCommand.CommandType = System.Data.CommandType.Text;

                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@FileName", file.Name));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@RemotePath", file.Path));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@CreatedOn", DateTimeOffset.Now));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@DownloadedOn", file.DownloadedOn));
                            sqlCommand.Parameters.Add(SqlParam.GetParameter("@StatusId", 1));
                            var res = sqlCommand.ExecuteScalar();
                            file.ID = Convert.ToInt32(res);
                        }
                        sqlTransaction.Commit();
                        pendingFiles.ForEach(t => t.IsSaved = true);
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        LogManager.Logger.WriteException("FileFilter", "SavePendingFiles", ex.Message, ex);
                    }
                }
            }
            return pendingFiles;
        }

        /// <summary>
        /// Gets the opis last processed Uid.
        /// </summary>
        /// <returns></returns>
        public int GetOPISLastProcessedId()
        {
            int lastProcessedUid = 0;
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = $"select MAX(Uid) from [dbo].[SourceFiles];";
                sqlCommand.CommandType = CommandType.Text;
                sqlConnection.Open();
                var result = sqlCommand.ExecuteScalar().ToString();
                lastProcessedUid = !string.IsNullOrEmpty(result) ? Convert.ToInt32(result) : lastProcessedUid;
            }

            return lastProcessedUid;
        }

        /// <summary>
        /// Saves the attachment details in source files table.
        /// </summary>
        /// <param name="pendingFiles">The pending files.</param>
        public void SaveOPISSourceFiles(PendingFile pendingFiles)
        {
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = $"INSERT INTO [dbo].[SourceFiles] (FileName,CreationDate,Uid) "
                                        + "VALUES (@FileName,@CreationDate,@Uid);SELECT SCOPE_IDENTITY();";

                        var sqlCommand = sqlConnection.CreateCommand();
                        sqlCommand.Transaction = sqlTransaction;
                        sqlCommand.CommandText = sqlQuery;
                        sqlCommand.CommandType = System.Data.CommandType.Text;

                        sqlCommand.Parameters.Add(SqlParam.GetParameter("@FileName", pendingFiles.Name));
                        sqlCommand.Parameters.Add(SqlParam.GetParameter("@CreationDate", DateTimeOffset.Now));
                        sqlCommand.Parameters.Add(SqlParam.GetParameter("@Uid", pendingFiles.Uid));
                        var res = sqlCommand.ExecuteNonQuery();
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        LogManager.Logger.WriteException("FileFilter", "SavePendingFiles", ex.Message, ex);
                    }
                }
            }
        }
    }
}
