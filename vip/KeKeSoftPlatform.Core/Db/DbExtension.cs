using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeKeSoftPlatform.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Entity;
using System.Data.SqlClient;
using EntityFramework.Extensions;



namespace KeKeSoftPlatform.Core
{
    public static class DbExtension
    {
        public static Pager<TEntity> Page<TEntity>(this IQueryable<TEntity> query, int pageNum, int pageSize = Pager.DEFAULT_PAGE_SIZE,int firstPageLeftSize=-1) where TEntity : class
        {
            //首页的条数
            int shouYeSize = pageSize;
            if(firstPageLeftSize>=0)
            {
                shouYeSize = firstPageLeftSize;
            }
            //总条数
            var queryCount = query.FutureCount();
            var pager = new Pager<TEntity>
            {
                FirstPageSize = firstPageLeftSize,
                ItemCount = queryCount.Value,
                PageNum = pageNum,
                PageSize = pageSize
            };
            if (firstPageLeftSize<0 && pager.PageCount == 1)
            {
                pager.PageNum = 1;
                pageNum = 1;
            }
            var data = query.Skip((pageNum - 1) * pageSize).Take(shouYeSize).Future();
            pager.Data = data.ToList();
            return pager;
        }

        #region 备份还原
        public static void Backup(this Database database, string fullPath)
        {
            database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "use master;backup database @0 to disk=@1;",
                new SqlParameter
                {
                    ParameterName = "@0",
                    Value = database.Connection.Database
                }, new SqlParameter
                {
                    ParameterName = "@1",
                    Value = fullPath
                });
        }

        public static void Restore(this Database database, string fullPath)
        {
            database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, 
                                            @"USE [master]
                                                DECLARE @spid NVARCHAR(20)   
                                                DECLARE cDblogin CURSOR  
                                                FOR
                                                    SELECT CAST(spid AS VARCHAR(20)) AS spid
                                                    FROM   MASTER..sysprocesses
                                                    WHERE  dbid = DB_ID(@dbname)

                                                                                            OPEN   cDblogin  
                                                                                            FETCH   NEXT   FROM   cDblogin   
                                                INTO   @spid   
                                                WHILE @@fetch_status = 0
                                                BEGIN
                                                    IF @spid <> @@SPID
                                                        EXEC ('kill   ' + @spid)
    
                                                    FETCH NEXT FROM cDblogin INTO @spid
                                                END       
                                                CLOSE   cDblogin   
                                                DEALLOCATE   cDblogin  
                                                RESTORE DATABASE @dbname  FROM DISK 
                                                = @fullPath 
                                                WITH REPLACE",
                                                new SqlParameter { ParameterName = "dbname", Value = database.Connection.Database }, 
                                                new SqlParameter { ParameterName = "fullPath", Value = fullPath });
        }
        #endregion
    }
}
