using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;

public class SQLiteHelper
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    private static SQLiteConnection conn;

    private static object _osqlLock = new object();
    /// <summary>
    /// 线程中操作使用的锁
    /// </summary>
    public static object oSqlLock
    {
        get { return _osqlLock; }
        set { _osqlLock = value; }
    }

    /// <summary>
    /// 初始化数据库链接
    /// </summary>
    /// <param name="dbPath">数据库路径</param>
    public static void Initial(string dbPath)
    {
        _osqlLock = new object();
        conn = new SQLiteConnection("Data Source=" + dbPath + ";Version=3;");
    }

    ~SQLiteHelper()
    {
        conn.Dispose();
    }

    /// <summary>
    /// 执行单条SQL语句(增删改)
    /// </summary>
    /// <param name="sql">单条SQL语句</param>
    /// <returns>受影响的总行数</returns>
    public static int ExecuteNonQuerySingleSql(string sql, params SQLiteParameter[] ps)
    {
        int count = 0;
        lock (_osqlLock)
        {
            try
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                if (ps != null)
                {
                    cmd.Parameters.AddRange(ps);
                }
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 执行多条SQL语句(增删改)使用了事务
    /// </summary>
    /// <param name="sqlList">SQL语句集合</param>
    /// <returns>受影响的总行数</returns>
    public static int ExecuteMultiSql(string[] sqlList)
    {
        int count = 0;
        lock (_osqlLock)
        {
            try
            {
                conn.Open();
                SQLiteTransaction sqltran = conn.BeginTransaction();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = conn;
                command.Transaction = sqltran;
                for (int i = 0; i < sqlList.Length; i++)
                {
                    try
                    {
                        command.CommandText = sqlList[i];
                        count += command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                sqltran.Commit();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 执行多条SQL语句(增删改)使用了事务
    /// </summary>
    /// <param name="sqlList">SQL语句集合</param>
    /// <returns>受影响的总行数</returns>
    public static int ExecuteMultiSql(List<string> sqlList)
    {
        return ExecuteMultiSql(sqlList.ToArray());
    }

    /// <summary>
    /// 数据库查询的方法
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <returns>数据表DataTable</returns>
    public static DataTable Select(string sql)
    {
        DataTable dt = new DataTable();
        lock (_osqlLock)
        {
            //try
            //{
                DataSet ds = new DataSet();
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                adapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            //}
        }
        return dt;
    }

    /// <summary>
    /// 数据库查询的方法
    /// </summary>
    /// <param name="sql">sql语句</param>
    /// <returns>数据表集DataSet</returns>
    public static DataSet SelectDataSet(string sql, List<SQLiteParameter> psList)  // 动态添加SQLiteParameter[]不方便,所以用list
    {
        DataSet dsr = new DataSet();
        lock (_osqlLock)
        {
            //try
            //{
                DataSet ds = new DataSet();
                conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                if (psList.Count>0)
                {
                    //params SQLiteParameter[] ps = psList.ToArray();
                    SQLiteParameter[] ps = psList.ToArray();
                    adapter.SelectCommand.Parameters.AddRange(ps);
                }
                adapter.Fill(ds);
                dsr = ds;
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //}
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
    }
        return dsr;
    }

    /// <summary>
    /// 查询数据条数
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int SelectDataRowCount(string sql)
    {
        try
        {
            return Select(sql).Rows.Count;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// 判断数据库中是否存在某表
    /// </summary>
    /// <param name="TableName">表名</param>
    /// <returns>是否存在</returns>
    public static bool ExistTable(string TableName)
    {
        bool IsExist = false;
        try
        {
            string sql = "select count(*) from MSysObjects WHERE MSysObjects.Name Like '" + TableName + "'";
            DataTable dt = Select(sql);
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0].ItemArray[0].ToString().IndexOf('1') > -1)
                {
                    IsExist = true;
                }
            }
        }
        catch (Exception ex)
        {
            IsExist = false;
        }
        return IsExist;
    }
}
