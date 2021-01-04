using System;
using System.Data;
using MVC.Models.DBCless;

namespace MVC.Models.Service
{
    public class CategoryService
    {
        DataAccess objConn;
        int MaxID;
        public DataTable FindCategoryByName(string typekey, string key)
        {
            DataTable dt = new DataTable();
            string str;
            try
            {
                objConn = new DataAccess();
                str = "select * from Category where CategoryName like '%' + @keyword +'%'";
                objConn.ClearParameter();
                objConn.AddParameter(typekey, "@keyword", key);
                var dr = objConn.ExecuteReader(str);
                if (dr.HasRows)
                {
                    dt.Load(dr);
                }
                dr.Close();
                objConn.Close();
                objConn.CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FindCategoryByID(string typekey, int ID)
        {
            DataTable dt = new DataTable();
            string str;
            try
            {
                objConn = new DataAccess();
                str = "select * from Category where CategoryID = @ID";
                objConn.ClearParameter();
                objConn.AddParameter(typekey, "@ID", ID);
                var dr = objConn.ExecuteReader(str);
                if (dr.HasRows)
                {
                    dt.Load(dr);
                }
                dr.Close();
                objConn.Close();
                objConn.CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable FindAllCategory()
        {
            DataTable dt = new DataTable();
            string str;
            try
            {
                objConn = new DataAccess();
                str = "select * from Category ";
                objConn.ClearParameter();
                var dr = objConn.ExecuteReader(str);
                if (dr.HasRows)
                {
                    dt.Load(dr);
                }
                dr.Close();
                objConn.Close();
                objConn.CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateCategoryByID(int ID, string Name, string Description)
        {
            string str;
            try
            {
                objConn = new DataAccess();
                str = "Update Category set CategoryName = @Name , Description = @Description where CategoryID = @ID";
                objConn.BeginTrans();
                objConn.ClearParameter();
                objConn.AddParameter("Int", "@ID", ID);
                objConn.AddParameter("Str", "@Name", Name);
                objConn.AddParameter("Str", "@Description", Description);
                var result = objConn.ExecuteNonQuery(str);
                objConn.CommitTrans();
                objConn.Close();
                objConn.CloseConnection();
                return result;
            }
            catch (Exception ex)
            {
                objConn.RollbackTrans();
                throw ex;
            }
        }

        public int AddNewCategory(string Name, string Description)
        {
            string str;
            int ID;
            try
            {
                objConn = new DataAccess();
                ID = GetLastID() + 1;
                str = "insert into [Category] (CategoryID,CategoryName,Description) VALUES  (@ID,@Name,@Description)";
                objConn.BeginTrans();
                objConn.ClearParameter();
                objConn.AddParameter("Str", "@ID", ID);
                objConn.AddParameter("Str", "@Name", Name);
                objConn.AddParameter("Str", "@Description", Description);
                var result = objConn.ExecuteNonQuery(str);
                objConn.CommitTrans();
                objConn.Close();
                objConn.CloseConnection();
                return result;
                
            }
            catch (Exception ex)
            {
                objConn.RollbackTrans();
                throw ex;
            }
        }

        private Int32 GetLastID()
        {
            DataTable dt = new DataTable();
            string str;
            MaxID = 0;

            try
            {
                objConn = new DataAccess();
                str = "select Max(CategoryID) As ID from Category ";
                objConn.ClearParameter();
                var dr = objConn.ExecuteReader(str);
                if (dr.HasRows)
                {
                    dt.Load(dr);
                    if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[0].ToString()))
                    { MaxID = 0; }
                    else
                    { MaxID = Int32.Parse(dt.Rows[0].ItemArray[0].ToString()); }
                    
                }
                dr.Close();
                objConn.Close();
                //objConn.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MaxID;
        }

    }
}
