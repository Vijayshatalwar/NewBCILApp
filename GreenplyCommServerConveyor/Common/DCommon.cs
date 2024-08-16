using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using GreenplyCommServer.Common;
using GreenplyCommServer;

namespace GreenplyCommServer.Common
{
    public class DCommon
    {

        #region PrivateProperties&Methods

        DBManager oManager;
        public string MODE
        {
            get;
            set;
        }

        public IDbConnection _Con;
        public IDbCommand _Com;
        public IDbTransaction _Transaction;
        public SqlDataAdapter _adp;
        public DCommon()
        {
            oManager = SqlDBProvider();



        }

        #region Start with Transaction

        public void TranStartTransaction()
        {
            DisConnect();
            Connect();
            _Transaction = _Com.Connection.BeginTransaction();
        }
        public void TranCommitTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Commit();
                _Transaction.Dispose();
            }
            DisConnect();
        }
        public void TranRollBackTransaction()
        {
            try
            {
                if (_Transaction != null)
                {
                    _Transaction.Rollback();
                    _Transaction.Dispose();
                }
                DisConnect();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void TranStopTransaction()
        {
            DisConnect();
            _Transaction = null;
        }
        #endregion
        #region Transaction Methods
        public bool Connect()
        {
            try
            {
                oManager = new DBManager(DataProvider.SqlServer, GlobalVariable.mSqlConString);
                if (oManager.Connection != null)
                {
                    if (oManager.isOpen())
                    {
                        if (_Com == null)
                        {
                            oManager.Open();
                            _Com = oManager.Command;
                            _Com.Connection = oManager.Connection;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    oManager.Open();
                    _Com = oManager.Command;
                    _Com.Connection = oManager.Connection;
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public bool DisConnect()
        {
            try
            {
                oManager = new DBManager(DataProvider.SqlServer, GlobalVariable.mSqlConString);
                if (oManager.Connection != null)
                {
                    if (oManager.isOpen())
                    {
                        oManager.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    oManager.Close();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public int TrnsExecuteSQL(string Squery)
        {
            int result = 0;
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            _Com.Transaction = _Transaction;
            result = _Com.ExecuteNonQuery();
            return result;
        }

        public int TrnsExecuteProcedure(string ProcName, SqlParameter[] Arrp)
        {
            int result = 0;
            _Com.CommandType = CommandType.StoredProcedure;
            _Com.CommandText = ProcName;
            _Com.Transaction = _Transaction;
            _Com.CommandTimeout = 0;

            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }
            result = _Com.ExecuteNonQuery();

            _Com.Parameters.Clear();

            return result;
        }



        public int TrnsExecuteProcedure(string ProcName)
        {
            int result = 0;
            _Com.CommandType = CommandType.StoredProcedure;
            _Com.CommandText = ProcName;
            _Com.Transaction = _Transaction;
            _Com.CommandTimeout = 0;

            result = _Com.ExecuteNonQuery();
            return result;
        }

        public DataTable TrnsGetDataUsingProcedure(string ProcName)
        {

            DataTable DT = new DataTable();
            _Com.CommandText = ProcName;
            _Com.Transaction = _Transaction;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(_Com as SqlCommand);
            da.Fill(DT);
            return DT;
        }

        public DataTable TrnsGetDataUsingProcedure(string ProcName, SqlParameter[] Arrp)
        {

            DataTable DT = new DataTable();
            _Com.CommandText = ProcName;
            _Com.Transaction = _Transaction;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.StoredProcedure;

            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(_Com as SqlCommand);
            da.Fill(DT);
            _Com.Parameters.Clear();
            return DT;
        }

        public DataSet TrnsExecuteDataSet(string Squery)
        {
            //DisConnect();
            //Connect();
            DataSet _ds = new DataSet();
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            _Com.Transaction = _Transaction;
            SqlDataAdapter adp = new SqlDataAdapter(_Com as SqlCommand);
            adp.Fill(_ds);

            //DisConnect();
            return _ds;

        }
        public object TrnsExecuteScalar(string Squery)
        {
            object Objresult;
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            _Com.Transaction = _Transaction;
            Objresult = _Com.ExecuteScalar();
            return Objresult;
        }
        public object TrnsExecuteScalar(string SqlProc, SqlParameter[] Arrp)
        {
            object Objresult;
            _Com.CommandType = CommandType.StoredProcedure;
            _Com.Transaction = _Transaction;
            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }

            Objresult = _Com.ExecuteScalar();
            _Com.Parameters.Clear();
            return Objresult;
        }

        public IDataReader TrnsGetDataReader(string Squery)
        {
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            _Com.Transaction = _Transaction;
            IDataReader dr = _Com.ExecuteReader();
            return dr;
        }
        #endregion
        #region   Not In Transaction
        public int ExecuteSQL(string Squery)
        {
            int result = 0;
            DisConnect();
            Connect();
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            result = _Com.ExecuteNonQuery();

            DisConnect();
            return result;
        }

        public int ExecuteProcedure(string ProcName, SqlParameter[] Arrp)
        {
            int result = 0;
            DisConnect();
            Connect();
            _Com.CommandType = CommandType.StoredProcedure;
            _Com.CommandText = ProcName;
            _Com.CommandTimeout = 0;

            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }
            result = _Com.ExecuteNonQuery();

            _Com.Parameters.Clear();
            DisConnect();

            return result;
        }



        public int ExecuteProcedure(string ProcName)
        {
            int result = 0;
            DisConnect();
            Connect();
            _Com.CommandType = CommandType.StoredProcedure;
            _Com.CommandText = ProcName;
            _Com.CommandTimeout = 0;

            result = _Com.ExecuteNonQuery();

            DisConnect();

            return result;
        }

        public DataTable GetDataUsingProcedure(string ProcName)
        {
            DisConnect();
            Connect();

            DataTable DT = new DataTable();


            _Com.CommandText = ProcName;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(_Com as SqlCommand);
            da.Fill(DT);
            DisConnect();

            return DT;
        }

        public DataTable GetDataUsingProcedure(string ProcName, SqlParameter[] Arrp)
        {
            DisConnect();
            Connect();

            DataTable DT = new DataTable();
            _Com.CommandText = ProcName;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.StoredProcedure;

            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }
           
            SqlDataAdapter da = new SqlDataAdapter(_Com as SqlCommand);
            da.Fill(DT);
            _Com.Parameters.Clear();
            DisConnect();

            return DT;
        }


        public object ExecuteScalar(string Squery)
        {
            object Objresult;
            DisConnect();
            Connect();
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            Objresult = _Com.ExecuteScalar();

            DisConnect();
            return Objresult;
        }
        public object ExecuteScalar(string SqlProc, SqlParameter[] Arrp)
        {
            object Objresult;
            DisConnect();
            Connect();
            _Com.CommandType = CommandType.StoredProcedure;
            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }

            Objresult = _Com.ExecuteScalar();
            _Com.Parameters.Clear();
            DisConnect();

            return Objresult;
        }


        public IDataReader GetDataUsingProcedurebyDataReader(string ProcName, SqlParameter[] Arrp)
        {
            DisConnect();
            Connect();

            IDataReader oDr;

            _Com.CommandText = ProcName;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.StoredProcedure;

            if (Arrp != null)
            {
                _Com.Parameters.Clear();
                foreach (SqlParameter pr in Arrp)
                {
                    _Com.Parameters.Add(pr);
                }
            }

            //  SqlDataAdapter da = new SqlDataAdapter(_Com as SqlCommand);
            oDr = _Com.ExecuteReader();

            _Com.Parameters.Clear();
            DisConnect();

            return oDr;
        }

        public IDataReader GetDataReader(string Squery)
        {
            DisConnect();
            Connect();
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            IDataReader dr = _Com.ExecuteReader();

            DisConnect();
            return dr;
        }

        public DataSet ExecuteDataSet(string Squery)
        {
            DisConnect();
            Connect();
            DataSet _ds = new DataSet();
            _Com.CommandText = Squery;
            _Com.CommandTimeout = 0;
            _Com.CommandType = CommandType.Text;
            SqlDataAdapter adp = new SqlDataAdapter(_Com as SqlCommand);
            adp.Fill(_ds);

            DisConnect();
            return _ds;

        }

        #endregion
        #region DataProvider
        public DBManager OracleDBProvider()
        {
            try
            {

                DBManager oManager = new DBManager(DataProvider.Oracle, GlobalVariable.mSqlConString);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DBManager SqlDBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, GlobalVariable.mSqlConString);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion


        #endregion
    }
}
