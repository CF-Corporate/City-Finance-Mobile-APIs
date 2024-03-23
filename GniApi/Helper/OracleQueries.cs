using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace GniApi.Helper
{
    public interface IOracleQueries
    {

    
        public void ExecuteDbProcedure(string pProcName, object[] values = null, string[] parameters = null);
        public DataTable GetDataSetFromDBProcedure(string p_ProcName, object[] values = null, string[] parameters = null);
        public (DataTable, int) GetDataSetFromDBProcedureWithCount(string p_ProcName, object[] values = null, string[] parameters = null);
        public string GetDataSetFromDBFunction(string p_ProcName, object[] values = null, string[] parameters = null);

        public DataTable RunQuery(string query, object[] values = null, string[] parameters = null);


        //public void Execute_SET_KOLLEKTOR_ALL_DATA(string pProcName, T_XLS_COLL xlsColl);
        //public void Execute_SET_SECTION_KOLLEKTOR_FOR_CONTRACT(string pProcName, T_XLS_COLL xlsColl);
    }

    public class OracleQueries : IOracleQueries
    {
        private readonly IConfiguration _configuration;

        public OracleQueries(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetDataSetFromDBFunction(string p_ProcName, object[] values = null, string[] parameters = null)
        {
            OracleConnection orclCon = null;
            try
            {
                // Open a connection
                var constr = _configuration.GetConnectionString("OracleDBConnection");
                orclCon = new OracleConnection(constr);
                orclCon.Open();


                OracleCommand cmd = new OracleCommand(p_ProcName, orclCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.FetchSize = 100000;
                //cmd.BindByName = true;
                OracleParameter p_refcursor = new OracleParameter();

                // this is vital to set when using ref cursors
                p_refcursor.OracleDbType = OracleDbType.Clob;

                p_refcursor.ParameterName = "return_value";
                // this is a function return value so we must indicate that fact
                p_refcursor.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(p_refcursor);



                //OracleParameter parameter = new OracleParameter();
                //// this is vital to set when using ref cursors
                //parameter.OracleDbType = OracleDbType.Int32;

                //parameter.ParameterName = "p_count";

                //// this is a function return value so we must indicate that fact
                //parameter.Direction = ParameterDirection.ReturnValue;

                ////parameter.Value = 133;

                //// add the parameter to the collection


                //cmd.Parameters.Add(parameter);

                // change point


                //OracleParameter parameter2 = new OracleParameter();
                //// this is vital to set when using ref cursors
                //parameter2.OracleDbType = OracleDbType.Int32;

                //parameter2.ParameterName = "p_count";

                //// this is a function return value so we must indicate that fact
                //parameter2.Direction = ParameterDirection.Output;


                ////parameter2.Value = 5;
                //// add the parameter to the collection

                ////cmd.Parameters.Add(p_refcursor);
                ////parameter2.ParameterName = "p_count";

                //cmd.Parameters.Add(parameter2);


                // object[] values = new object[1] { "fdsf" };
                // string[] parameters = new string[1] { "p_TG_SAZISH_SENED_NOMRE" };


                if (values != null)
                {

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i != values.Count() -1)
                        {
                            cmd.Parameters.Add(parameters[i], values[i]);
                            cmd.Parameters[i+1].Direction = ParameterDirection.Input;
                        }
                        else
                        {
                            cmd.Parameters.Add(parameters[i], OracleDbType.Clob, values[i], ParameterDirection.Input);
                            //cmd.Parameters[i].Direction = ParameterDirection.Input;
                        }
                    }
                }


                // create a data adapter to use with the data set
                //OracleDataAdapter da = new OracleDataAdapter(cmd);


                cmd.ExecuteNonQuery();

                OracleClob clobData = (OracleClob)p_refcursor.Value;
                string result = clobData.Value;
                


                //var p_count = int.Parse(cmd.Parameters["p_count"].Value.ToString());
                // create the data set
                //DataSet ds = new DataSet();
                // fill the data set


                //da.Fill(ds);


                // Decimal intValue = OracleDbType.Parse(cmd.Parameters[1].Value);


                // Console.WriteLine(cmd.Parameters[0].ParameterName.ToString());                //Console.WriteLine(cmd.Parameters[0].ParameterName.ToString());

                //Console.WriteLine(cmd.Parameters[].ParameterName.ToString());
                //Console.WriteLine(cmd.Parameters[3].Value);
                // clean up our objects release resourcesint

                //int allRecordCount = 133_000;


                //da.Dispose();
                //p_refcursor.Dispose();
                cmd.Dispose();
                return  result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                // Close the connection
                if (null != orclCon)
                    orclCon.Close();
            }


        }
        public DataTable GetDataSetFromDBProcedure(string p_ProcName, object[] values = null, string[] parameters = null)
        {
            OracleConnection orclCon = null;
            try
            {
                // Open a connection
                var constr = _configuration.GetConnectionString("OracleDBConnection");
                orclCon = new OracleConnection(constr);
                orclCon.Open();


                OracleCommand cmd = new OracleCommand(p_ProcName, orclCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.FetchSize = 100000;
                //cmd.BindByName = true;
                OracleParameter p_refcursor = new OracleParameter();
                // this is vital to set when using ref cursors
                p_refcursor.OracleDbType = OracleDbType.RefCursor;
                // this is a function return value so we must indicate that fact
                p_refcursor.Direction = ParameterDirection.Output;
                // add the parameter to the collection
                cmd.Parameters.Add(p_refcursor);
                //object[] values = new object[1] { "fdsf" };
                //string[] parameters = new string[1] { "p_TG_SAZISH_SENED_NOMRE" };
                if (values != null)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i], values[i]);
                        cmd.Parameters[i + 1].Direction = ParameterDirection.Input;
                    }
                }


                // create a data adapter to use with the data set
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                // create the data set
                DataSet ds = new DataSet();
                // fill the data set
                da.Fill(ds);
                // clean up our objects release resources
                da.Dispose();
                p_refcursor.Dispose();
                cmd.Dispose();

                return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                // Close the connection
                if (null != orclCon)
                    orclCon.Close();
            }



        }
        public (DataTable, int) GetDataSetFromDBProcedureWithCount(string p_ProcName, object[] values = null, string[] parameters = null)
        {
            OracleConnection orclCon = null;
            try
            {
                // Open a connection
                var constr = _configuration.GetConnectionString("OracleDBConnection");
                orclCon = new OracleConnection(constr);
                orclCon.Open();


                OracleCommand cmd = new OracleCommand(p_ProcName, orclCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.FetchSize = 100000;
                //cmd.BindByName = true;
                OracleParameter p_refcursor = new OracleParameter();
                // this is vital to set when using ref cursors
                p_refcursor.OracleDbType = OracleDbType.RefCursor;
                // this is a function return value so we must indicate that fact
                p_refcursor.Direction = ParameterDirection.Output;
                // add the parameter to the collection

                OracleParameter parameter = new OracleParameter();
                // this is vital to set when using ref cursors
                parameter.OracleDbType = OracleDbType.Int32;

                parameter.ParameterName = "p_count";

                // this is a function return value so we must indicate that fact
                parameter.Direction = ParameterDirection.Output;

                //parameter.Value = 133;

                // add the parameter to the collection
                cmd.Parameters.Add(p_refcursor);
                cmd.Parameters.Add(parameter);




                //object[] values = new object[1] { "fdsf" };
                //string[] parameters = new string[1] { "p_TG_SAZISH_SENED_NOMRE" };
                if (values != null)
                {
                    for (int i = 1; i <= values.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i - 1], values[i - 1]);
                        cmd.Parameters[i + 1].Direction = ParameterDirection.Input;
                    }
                }

                cmd.ExecuteNonQuery();

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                var p_count = int.Parse(cmd.Parameters["p_count"].Value?.ToString());
                // create a data adapter to use with the data set

                // create the data set
                DataSet ds = new DataSet();
                // fill the data set
                da.Fill(ds);
                // clean up our objects release resources
                da.Dispose();
                p_refcursor.Dispose();
                cmd.Dispose();

                return ds.Tables.Count > 0 ? (ds.Tables[0], p_count) : (new DataTable(), p_count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                // Close the connection
                if (null != orclCon)
                    orclCon.Close();
            }



        }
        public DataTable RunQuery(string query, object[] values = null, string[] parameters = null)
        {
            OracleConnection orclCon = null;
            try
            {
                // Open a connection
                var constr = _configuration.GetConnectionString("OracleDBConnection");
                orclCon = new OracleConnection(constr);
                orclCon.Open();


                OracleCommand cmd = new OracleCommand(query, orclCon);
                cmd.BindByName = true;
                //object[] values = new object[1] { "fdsf" };
                //string[] parameters = new string[1] { "p_TG_SAZISH_SENED_NOMRE" };
                if (values != null)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        cmd.Parameters.Add(parameters[i], values[i]);
                        cmd.Parameters[i].Direction = ParameterDirection.Input;
                    }
                }


                // create a data adapter to use with the data set
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                // create the data set
                DataSet ds = new DataSet();
                // fill the data set
                da.Fill(ds);
                // clean up our objects release resources
                da.Dispose();
                cmd.Dispose();

                return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                // Close the connection
                if (null != orclCon)
                    orclCon.Close();
            }



        }
        public void ExecuteDbProcedure(string pProcName, object[] values = null, string[] parameters = null)
        {
            OracleConnection conn = null;
            var constr = _configuration.GetConnectionString("OracleDBConnection");
            conn = new OracleConnection(constr);
            conn.Open();
            OracleCommand cmd = new OracleCommand(pProcName, conn);
            cmd.FetchSize = 1000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.FetchSize = 1000;

            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    cmd.Parameters.Add(parameters[i], values[i]);
                    cmd.Parameters[i].Direction = ParameterDirection.Input;
                }
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                if (null != conn)
                    conn.Close();
            }


        }


    }
}
