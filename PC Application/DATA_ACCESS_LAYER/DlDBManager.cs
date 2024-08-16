using System;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Collections.Generic;

public enum DataProvider
{
    Oracle, SqlServer, OleDb, Odbc
}

namespace DATA_ACCESS_LAYER
{
    public interface IDBManager
    {
        DataProvider ProviderType
        {
            get;
            set;
        }

        string ConnectionString
        {
            get;
            set;
        }
        IDbConnection Connection
        {
            get;
        }
        IDbTransaction Transaction
        {
            get;
        }

        IDataReader DataReader
        {
            get;
        }
        IDbCommand Command
        {
            get;
        }

        IDbDataParameter[] Parameters
        {
            get;
        }

        List<IDbDataParameter> ParametersList
        {
            get;
        }

        void Open();
        void BeginTransaction();
        void BeginTransaction(IDbConnection Connection);
        void CommitTransaction();
        void RollBackTransaction();
        void CreateParameters(int paramsCount);
        void AddParameters(int index, string paramName, object objValue);
        void AddOutParameters(int index, string paramName, int size, int iNoOfOutParams);
        string GetParameterValue(int index);
        string GetParameterValue(string parameterName);
        IDataReader ExecuteReader(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        object ExecuteScalar(CommandType commandType, string commandText);
        int ExecuteNonQuery(CommandType commandType, string commandText);
        void CloseReader();
        void Close();
        void Dispose();
    }
    public sealed class DBManagerFactory
    {
        private DBManagerFactory() { } //constructor for dbmanager factory

        public static IDbConnection GetConnection(DataProvider providerType)
        {
            IDbConnection iDbConnection = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDbConnection = new SqlConnection();
                    break;
                case DataProvider.OleDb:
                    iDbConnection = new OleDbConnection();
                    break;
                case DataProvider.Odbc:
                    iDbConnection = new OdbcConnection();
                    break;
                case DataProvider.Oracle:
                    iDbConnection = new OracleConnection();
                    break;
                default:
                    return null;
            }
            return iDbConnection;
        }

        public static IDbCommand GetCommand(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlCommand();
                case DataProvider.OleDb:
                    return new OleDbCommand();
                case DataProvider.Odbc:
                    return new OdbcCommand();
                case DataProvider.Oracle:
                    return new OracleCommand();
                default:
                    return null;
            }
        }

        public static IDbDataAdapter GetDataAdapter(DataProvider providerType)
        {
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    return new SqlDataAdapter();
                case DataProvider.OleDb:
                    return new OleDbDataAdapter();
                case DataProvider.Odbc:
                    return new OdbcDataAdapter();
                case DataProvider.Oracle:
                    return new OracleDataAdapter();
                default:
                    return null;
            }
        }

        public static IDbTransaction GetTransaction(DataProvider providerType)
        {
            IDbConnection iDbConnection = GetConnection(providerType);
            IDbTransaction iDbTransaction = iDbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            return iDbTransaction;
        }

        public static IDataParameter GetParameter(DataProvider providerType)
        {
            IDataParameter iDataParameter = null;
            switch (providerType)
            {
                case DataProvider.SqlServer:
                    iDataParameter = new SqlParameter();
                    break;
                case DataProvider.OleDb:
                    iDataParameter = new OleDbParameter();
                    break;
                case DataProvider.Odbc:
                    iDataParameter = new OdbcParameter();
                    break;
                case DataProvider.Oracle:
                    iDataParameter = new OracleParameter();
                    break;

            }
            return iDataParameter;
        }

        public static IDbDataParameter[] GetParameters(DataProvider providerType, int paramsCount)
        {
            IDbDataParameter[] idbParams = new IDbDataParameter[paramsCount];

            switch (providerType)
            {
                case DataProvider.SqlServer:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new SqlParameter();
                    }
                    break;
                case DataProvider.OleDb:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OleDbParameter();
                    }
                    break;
                case DataProvider.Odbc:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OdbcParameter();
                    }
                    break;
                case DataProvider.Oracle:
                    for (int i = 0; i < paramsCount; ++i)
                    {
                        idbParams[i] = new OracleParameter();
                    }
                    break;
                default:
                    idbParams = null;
                    break;
            }
            return idbParams;
        }

        internal static IDbTransaction GetTransaction(DataProvider dataProvider, IDbConnection Connection)
        {
            IDbConnection iDbConnection = Connection;
            IDbTransaction iDbTransaction = iDbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            return iDbTransaction;
        }

        //public static IDbDataParameter GetParameterType(DataProvider providerType)
        //{
        //    IDbDataParameter idbParam;
        //    switch (providerType)
        //    {
        //        case DataProvider.SqlServer:
        //            idbParam = new SqlParameter();
        //            break;
        //        case DataProvider.OleDb:
        //            idbParam = new OleDbParameter();
        //            break;
        //        case DataProvider.Odbc:
        //            idbParam = new OdbcParameter();
        //            break;
        //        case DataProvider.Oracle:
        //            idbParam = new OracleParameter();
        //            break;
        //        default:
        //            idbParam = null;
        //            break;
        //    }
        //    return idbParam;
        //}
    }
    public sealed class DBManager : IDBManager, IDisposable
    {
        private IDbConnection idbConnection;
        private IDataReader idataReader;
        private IDbCommand idbCommand;
        private DataProvider providerType;
        private IDbTransaction idbTransaction = null;
        private IDbDataParameter[] idbParameters = null;
        private List<IDbDataParameter> idbParametersList = new List<IDbDataParameter>();
        private string strConnection;

        public DBManager()
        {

        }

        public DBManager(DataProvider providerType)
        {
            this.providerType = providerType;
        }

        public DBManager(DataProvider providerType, string connectionString)
        {
            this.providerType = providerType;
            this.strConnection = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return idbConnection;
            }
        }

        public IDataReader DataReader
        {
            get
            {
                return idataReader;
            }
            set
            {
                idataReader = value;
            }
        }

        public DataProvider ProviderType
        {
            get
            {
                return providerType;
            }
            set
            {
                providerType = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return strConnection;
            }
            set
            {
                strConnection = value;
            }
        }

        public IDbCommand Command
        {
            get
            {
                return idbCommand;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return idbTransaction;
            }
        }

        public IDbDataParameter[] Parameters
        {
            get
            {
                return idbParameters;
            }
        }

        public List<IDbDataParameter> ParametersList
        {
            get
            {
                return idbParametersList;
            }
        }

        public void Open()
        {
            idbConnection = DBManagerFactory.GetConnection(this.providerType);
            idbConnection.ConnectionString = this.ConnectionString;
            if (idbConnection.State != ConnectionState.Open)
                idbConnection.Open();
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
        }

        public void Close()
        {
            if (idbConnection.State != ConnectionState.Closed)
                idbConnection.Close();
        }

        public void Dispose()
        {
            this.Close();
            this.idbCommand = null;
            this.idbTransaction = null;
            this.idbConnection = null;
        }

        public void CreateParameters(int paramsCount)
        {
            idbParameters = new IDbDataParameter[paramsCount];
            idbParameters = DBManagerFactory.GetParameters(this.ProviderType,
              paramsCount);
        }

        public void AddParameters(int index, string paramName, object objValue)
        {
            if (index < idbParameters.Length)
            {
                idbParameters[index].ParameterName = paramName;
                idbParameters[index].Value = objValue;
            }
        }

        public void AddOutParameters(int index, string paramName, int iSize, int iNoOfOutParams)
        {
            IDbDataParameter[] oIDbDataParameter = DBManagerFactory.GetParameters(this.ProviderType, iNoOfOutParams);
            oIDbDataParameter[0].ParameterName = paramName;
            oIDbDataParameter[0].Value = "0";
            oIDbDataParameter[0].Direction = ParameterDirection.Output;
            oIDbDataParameter[0].Size = iSize;
            idbParameters[index] = oIDbDataParameter[0];
        }

        public string GetParameterValue(string parameterName)
        {
            foreach (IDbDataParameter feIDbDataParameter in idbParameters)
            {
                if (feIDbDataParameter.ParameterName == parameterName)
                { return feIDbDataParameter.Value.ToString(); }
            }
            return "";
        }

        public string GetParameterValue(int index)
        {
            if (index < idbParameters.Length)
            { return idbParameters[index].Value.ToString(); }
            return "";
        }

        public void BeginTransaction()
        {
            if (this.idbTransaction == null)
                idbTransaction =
                DBManagerFactory.GetTransaction(this.ProviderType);
            this.idbCommand.Transaction = idbTransaction;
        }
        
        public void BeginTransaction(IDbConnection Connection)
        {
            if (this.idbTransaction == null)
                idbTransaction =
                DBManagerFactory.GetTransaction(this.ProviderType, Connection);
            this.idbCommand.Transaction = idbTransaction;
        }

        public void CommitTransaction()
        {
            if (this.idbTransaction != null)
                this.idbTransaction.Commit();
            idbTransaction = null;
        }

        public void RollBackTransaction()
        {
            if (this.idbTransaction != null)
                this.idbTransaction.Rollback();
            idbTransaction = null;
        }

        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            idbCommand.Connection = this.Connection;
            PrepareCommand(idbCommand, this.Connection, this.Transaction,
             commandType,
              commandText, this.Parameters);
            this.DataReader = idbCommand.ExecuteReader();
            idbCommand.Parameters.Clear();
            return this.DataReader;
        }

        public void CloseReader()
        {
            if (this.DataReader != null)
                this.DataReader.Close();
        }

        private void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
        {
            foreach (IDbDataParameter idbParameter in commandParameters)
            {
                if ((idbParameter.Direction == ParameterDirection.InputOutput) && (idbParameter.Value == null))
                {
                    idbParameter.Value = DBNull.Value;
                }
                command.Parameters.Add(idbParameter);
            }
        }

        private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            PrepareCommand(idbCommand, this.Connection, this.Transaction,
            commandType, commandText, this.Parameters);
            int returnValue = idbCommand.ExecuteNonQuery();
            idbCommand.Parameters.Clear();
            return returnValue;
        }

        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            PrepareCommand(idbCommand, this.Connection, this.Transaction,
            commandType,
              commandText, this.Parameters);
            object returnValue = idbCommand.ExecuteScalar();
            idbCommand.Parameters.Clear();
            return returnValue;
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            this.idbCommand = DBManagerFactory.GetCommand(this.ProviderType);
            PrepareCommand(idbCommand, this.Connection, this.Transaction, commandType, commandText, this.Parameters);
            IDbDataAdapter dataAdapter = DBManagerFactory.GetDataAdapter(this.ProviderType);
            dataAdapter.SelectCommand = idbCommand;
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            idbCommand.Parameters.Clear();
            return dataSet;
        }

    }
}
