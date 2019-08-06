using Dapper;
using OfficeOnline.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using UploadFile.Models;

namespace UploadFile.Repository
{
    public class FileRepository
    {
        public int AddFile(File file)
        {
            try
            {
                var param = new DynamicParameters();
                if (file.FileID != Guid.Empty)
                    param.Add("@FileID", file.FileID, DbType.Guid);
                else
                    param.Add("@FileID", Guid.NewGuid(), DbType.Guid);

                param.Add("@NameFile", file.FileName, DbType.String);
                param.Add("@UploadDate", file.UploadDate, DbType.Date);
                param.Add("@UploadPerson", file.UploadPerson, DbType.String);                               
                var enumResult = DapperHelper.Execute("uploadfile_AddFile", "admin", param);
                return enumResult;
            }
            catch (Exception ex)
            {
                // ghi log
                throw ex;
            }
        }
    }
}