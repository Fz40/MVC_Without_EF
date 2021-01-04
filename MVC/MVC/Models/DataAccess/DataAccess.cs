using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace MVC.Models.DBCless
{

    public class DataAccess
    {

        //protected string Conn = "Data Source=SSO\\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True ;User ID=sa; Password=123456;";
        //protected string Conn = "Server=LENOVO;Database=Northwind;uid=sa; password=1234";
        protected string Conn = ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString;


        protected SqlConnection databaseConnection;

        private SqlTransaction _dbTrans;
        private SqlCommand _dbCommand = new SqlCommand();
        
        private Boolean _CheckBeginTrans = false;

        private void ConnectDB()
        {
            if (databaseConnection == null)
                databaseConnection = new SqlConnection(Conn);

            if (databaseConnection.State != ConnectionState.Open)
                databaseConnection.Open();
        }

        public void AddParameter(string paraType, string paraName, object paraValue)
        {
            try
            {
                //_dbCommand.Parameters.Add(new SqlParameter(paraName, paraValue));
                switch (paraType)
                {
                    case "Str":
                        _dbCommand.Parameters.Add(paraName, SqlDbType.VarChar).Value = paraValue;
                        break;

                    case "Int":
                        _dbCommand.Parameters.Add(paraName, SqlDbType.Int).Value = paraValue;
                        break;
                }

            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void ClearParameter()
        {
            try
            {
                _dbCommand.Parameters.Clear();
            }
            catch (SqlException ex)
            {


            }
            catch (Exception ex)
            {

            }
        }

        public SqlDataReader ExecuteReader(string sqlcommand)
        {
            ConnectDB();
            _dbCommand.Connection = databaseConnection;
            _dbCommand.CommandText = sqlcommand;
            if (_CheckBeginTrans == true)
            {
                _dbCommand.Transaction = _dbTrans;
            }
            if (_dbCommand.Connection.State != ConnectionState.Open)
            {
                _dbCommand.Connection.Open();
            }
            var dataexec = _dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            return dataexec;
        }

        public int ExecuteNonQuery(string sqlCommand)
        {
            ConnectDB();
            _dbCommand.Connection = databaseConnection;
            _dbCommand.CommandText = sqlCommand;
            var rowAffect = 0;
            rowAffect = _dbCommand.ExecuteNonQuery();

            return rowAffect;
        }

        public void BeginTrans()
        {

            try
            {
                if (databaseConnection.State != ConnectionState.Open)
                {
                    _dbTrans = databaseConnection.BeginTransaction();
                    _CheckBeginTrans = true;
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void CommitTrans()
        {
            try
            {
                if (_dbTrans != null)
                {
                    _dbTrans.Commit();
                    _dbTrans = null;
                    Close();
                    Dispose();
                    _CheckBeginTrans = false;
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void RollbackTrans()
        {
            try
            {
                if (_dbTrans != null)
                {
                    _dbTrans.Rollback();
                    _dbTrans = null;
                    Close();
                    Dispose();
                    _CheckBeginTrans = false;
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void Close()
        {
            try
            {
                if (_dbCommand.Connection.State != ConnectionState.Open)
                {
                    _dbCommand.Connection.Close();
                }
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        public void Dispose()
        {
            try
            {
                _dbCommand.Dispose();
                databaseConnection.Dispose();
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
        public void CloseConnection()
        {
            try
            {
                databaseConnection.Close();
                databaseConnection.Dispose();
            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {

            }

        }

    }
}
