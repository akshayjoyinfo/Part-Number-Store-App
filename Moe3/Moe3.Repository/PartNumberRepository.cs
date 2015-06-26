using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moe3.Domain;
using Moe3.Logging;

namespace Moe3.Repository
{
    public static class PartNumberRepository
    {
        public static Logger RepoLogger = null;

        static PartNumberRepository()
        {
            RepoLogger = new Logger();    
        }
        public static bool AddPartNumber(InventoryItem item)
        {
            bool status = false;
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();

                string commandStatement = "UPDATE  Inventory SET Quantity = Quantity + " + item.Quantity + ", " + item.Location + "= " + item.Location + " + " + item.Quantity + " WHERE PartNumber = '" + item.PartNumber + "' AND Version = " + item.Version;
                string dailyfactCommmanStatemetn =
                    "INSERT INTO InventoryDailyFacts (PartNumber,Version,Location,Quantity,TransactionDate,IsAdd) VALUES('" +
                    item.PartNumber + "'," + item.Version + ",'" + item.Location +
                    "'," + item.Quantity + ",'" + DateTime.Now.ToShortDateString() + "',1)"; 
                                                                           
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.INFO, " AddPartNumber SQL " + commandStatement);
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.INFO, " AddPartNumber SQL " + dailyfactCommmanStatemetn);
                RepoLogger.LogMsg(LogModes.REPO,LogLevel.INFO, "Plain Query SQL -UPDATE Inventory SET Quantity =  Quantity + 20 ,  A1 = A1 + 20  where PartNumber = 1038407748 AND Version =1" );
                OleDbCommand sqlCommand = new OleDbCommand();
                sqlCommand.CommandText = commandStatement;
                sqlCommand.Connection = repoConnection;
                int rows = sqlCommand.ExecuteNonQuery();
                
                sqlCommand.CommandText = dailyfactCommmanStatemetn;
                int drow = sqlCommand.ExecuteNonQuery();

                if (rows > 0 && drow > 0)
                    status = true;

            }
            catch (Exception exp)
            {

                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                    "Error while adding AddPartNumber- " + exp.Message + " StackTrace:- " + exp.StackTrace);
                status = false;
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return status;
        }

        public static bool RestorePartNumber(InventoryItem item)
        {
            bool status = false;
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();

                string commandStatement = "UPDATE  Inventory SET Quantity = Quantity - " + item.Quantity + ", " + item.Location + "= " + item.Location + " - " + item.Quantity + " WHERE PartNumber = '" + item.PartNumber + "' AND Version = " + item.Version;
                string dailyfactCommmanStatemetn =
                    "INSERT INTO InventoryDailyFacts (PartNumber,Version,Location,Quantity,TransactionDate,IsAdd) VALUES('" +
                    item.PartNumber + "'," + item.Version + ",'" + item.Location +
                    "'," + item.Quantity + ",'" + DateTime.Now.ToShortDateString() + "',0)";

                RepoLogger.LogMsg(LogModes.REPO, LogLevel.INFO, " AddPartNumber SQL " + commandStatement);
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.INFO, " AddPartNumber SQL " + dailyfactCommmanStatemetn);
                
                OleDbCommand sqlCommand = new OleDbCommand();
                sqlCommand.CommandText = commandStatement;
                sqlCommand.Connection = repoConnection;
                int rows = sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = dailyfactCommmanStatemetn;
                int drow = sqlCommand.ExecuteNonQuery();

                if (rows > 0 && drow > 0)
                    status = true;

            }
            catch (Exception exp)
            {

                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                    "Error while adding AddPartNumber- " + exp.Message + " StackTrace:- " + exp.StackTrace);
                status = false;
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return status;
        }

        public static List<string> GetStoreLocations()
        {
            List<string> listColumns = new List<string>();
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();
                using (var cmd = new OleDbCommand("SELECT * from Inventory", repoConnection))
                using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    var table = reader.GetSchemaTable();
                    var nameCol = table.Columns["ColumnName"];
                    foreach (DataRow row in table.Rows)
                    {
                        listColumns.Add(row[nameCol].ToString());
                    }
                }
                
                   

                string[] elems = new string[] { "Version", "Quantity","PartNumber" ,"ID"};
                listColumns.RemoveAll(x => elems.Contains(x));
            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                  "Error while Getting GetStoreLocations - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return listColumns;
        
        }

        public static List<string> GetPartNumbers()
        {
            List<string> listColumns = new List<string>();
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();
                OleDbCommand cmdExcel = new OleDbCommand();
                cmdExcel.Connection = repoConnection;
                cmdExcel.CommandText = "SELECT PartNumber From [Inventory] ";
                OleDbDataReader reader = cmdExcel.ExecuteReader();
                while (reader.Read())
                {
                    listColumns.Add(reader[0].ToString());
                }
                reader.Close();   
            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                  "Error while Getting GetStoreLocations - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return listColumns;

        }

        public static bool CheckPartNumberExist(InventoryItem item)
        {
            bool status = false;
            OleDbConnection repoConnection = null;
            int NoOfParts = 0;
            try
            {
                repoConnection = DbConnection.OpenConnection();
                
                string commandtext = "SELECT count(*) From [Inventory] WHERE  [PartNumber] = '"+item.PartNumber+"' AND [Version] = "+item.Version;
                RepoLogger.LogMsg(LogModes.REPO,LogLevel.INFO,"SQL :- " + commandtext);
                OleDbCommand sqlCommand = new OleDbCommand();
                sqlCommand.CommandText = commandtext;
                sqlCommand.Connection = repoConnection;
                NoOfParts = Convert.ToInt32(sqlCommand.ExecuteScalar());

                if (NoOfParts >= 1)
                    status = true;


            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                  "Error while Getting CheckPartNumberExist - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                status = false;
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return status;

        }

        public static bool CheckPartLocaionExist(InventoryItem item)
        {
            bool status = false;


            try
            {
                List<string> listLocation = GetStoreLocations();
                status = listLocation.Exists(x => x == item.Location);
            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                    "Error while Getting CheckPartLocaionExist - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                status = false;
                throw;
            }
            
           
            return status;
        }

        public static Queue<InventoryItem> GetAllNonZeroItems()
        {
            Queue<InventoryItem> listInventoryItems = new Queue<InventoryItem>();
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();
                List<string> columns = GetStoreLocations();
                string dynamicSQL = "SELECT  PartNumber, Version , Quantity FROM Inventory WHERE Quantity > 0 ORDER BY PartNumber ASC";


                OleDbCommand sqlCommand = new OleDbCommand();
                sqlCommand.CommandText = dynamicSQL;
                sqlCommand.Connection = repoConnection;
                OleDbDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    InventoryItem cls = new InventoryItem(reader["PartNumber"].ToString(), Convert.ToInt32(reader["Version"].ToString()),Convert.ToInt32(reader["Quantity"].ToString()));
                    listInventoryItems.Enqueue(cls);
                }
            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                  "Error while Getting GetAllNonZeroItems  - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return listInventoryItems;
        }

        public static List<InventoryDaiyFact> GetInventoryReport(string fromDate, string toDate)
        {
            List<InventoryDaiyFact> listInventoryItems = new List<InventoryDaiyFact>();
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();
          
                string dynamicSQL =
                    "SELECT TransactionDate,PartNumber, Version ,IsAdd, SUM(Quantity) AS Qty FROM InventoryDailyFacts WHERE TransactionDate   Between  Format(#" +
                    fromDate + "#, 'dd/mm/yyyy') And   Format(#" + toDate +
                    "#, 'dd/mm/yyyy')  GROUP BY TransactionDate, PartNumber, Version,IsAdd ORDER BY TransactionDate desc";


                OleDbCommand sqlCommand = new OleDbCommand();
                sqlCommand.CommandText = dynamicSQL;
                sqlCommand.Connection = repoConnection;
                OleDbDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(reader[0].ToString());
                    string Parttime = reader[1].ToString();
                    int version = Convert.ToInt32(reader[2].ToString());
                    string transType = Convert.ToBoolean(reader[3].ToString()) == true ? "Add" : "Restore";
                    int quantity = Convert.ToInt32(reader[4].ToString());
                    InventoryDaiyFact cls = new InventoryDaiyFact(Parttime,version,quantity,date,transType);
                    listInventoryItems.Add(cls);
                }
            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                  "Error while Getting GetAllNonZeroItems  - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return listInventoryItems;
        }
        public static List<LocationQty> GetPartNumbersByPartNumberVersion(InventoryItem item)
        {
            List<LocationQty> listInventoryItems = new List<LocationQty>();
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();
                List<string> columns = GetStoreLocations();
                string dynamicSQL = "";
                int colPos = 1;
                int chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["LocationChunkSize"]);
                

                while (columns.Any())
                {
                    List<string> listGroupColumns = new List<string>();
                    listGroupColumns = columns.Take(chunkSize).ToList();
                    columns = columns.Skip(chunkSize).ToList();
                    dynamicSQL = "";
                    colPos = 1;
                    foreach (var column in listGroupColumns)
                    {
                        string unionQuery = colPos == listGroupColumns.Count ? "" : " UNION ALL \n";
                        dynamicSQL = dynamicSQL + " SELECT '" + column + "' AS Location , " + column +
                                     " AS Qty FROM Inventory where PartNumber='" + item.PartNumber + "' AND Version =" +
                                     item.Version
                                     + unionQuery;
                        colPos++;
                       


                    }
                    OleDbCommand sqlCommand = new OleDbCommand();
                    sqlCommand.CommandText = dynamicSQL;
                    sqlCommand.Connection = repoConnection;
                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        LocationQty cls = new LocationQty(reader[0].ToString(), Convert.ToInt32(reader[1].ToString()));
                        listInventoryItems.Add(cls);
                    }

                }


              

            }
            catch (Exception exp)
            {
                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                 "Error while Getting GetPartNumbersByPartNumberVersion - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            List<LocationQty> errorLocations = listInventoryItems.Where(x => x.Quantity == 0).ToList();
            List<LocationQty> finalList = listInventoryItems.Except(errorLocations).ToList();
            return finalList;
        }

        public static bool AddLocationColumn(string location)
        {
            bool status = false;
            OleDbConnection repoConnection = null;
            try
            {
                repoConnection = DbConnection.OpenConnection();

                DataTable dtExcelSchema;
                //Get the Schema of the WorkBook
                dtExcelSchema = repoConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);



                OleDbCommand cmdExcel = new OleDbCommand();
                cmdExcel.Connection = repoConnection;
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                cmdExcel.CommandText = "ALTER TABLE [" + SheetName + "] ADD COLUMN [" + location + "] VARCHAR(45)";
                cmdExcel.ExecuteNonQuery();
                status = true;
            }
            catch (Exception exp)
            {

                RepoLogger.LogMsg(LogModes.REPO, LogLevel.ERROR,
                    "Error while Getting AddLocactionColumn - " + exp.Message + " StackTrace:- " + exp.StackTrace);
                status = false;
                throw;
            }
            finally
            {
                DbConnection.CloseConnection(repoConnection);
            }
            return status;
        }

    }
}
