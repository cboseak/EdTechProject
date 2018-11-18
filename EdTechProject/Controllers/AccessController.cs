using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EdTechProject.Controllers
{

    /*TODO clean up all the SQL injection issues in this controller*/
    public class AccessController : Controller
    {

        public ActionResult GetMessagesByToUserId(int userId)
        {
            var query = $@"SELECT  a.[ID]
                ,[FromUserId]
                ,[ToUserId]
                ,[Subject]
                ,[Body]
                ,[CreatedOn]
                ,b.FirstName as senderFirstName
                ,b.LastName as senderLastName
                ,c.FirstName as toFirstName
                ,c.LastName as toLastName
	            ,c.Type
            FROM [DB_9FEBFD_cboseak].[dbo].[EdTechMessage] a
			inner join [Users] b on a.FromUserId = b.ID
			inner join [Users] c on a.ToUserId = c.ID
            Where ToUserId = {userId}";


            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult GetMessagesByFromUserId(int userId)
        {
            var query = $@"SELECT  a.[ID]
              ,[FromUserId]
              ,[ToUserId]
              ,[Subject]
              ,[Body]
              ,[CreatedOn]
			  ,b.FirstName as senderFirstName
			  ,b.LastName as senderLastName
			  ,c.FirstName as toFirstName
			  ,c.LastName as toLastName
            FROM [DB_9FEBFD_cboseak].[dbo].[EdTechMessage] a
			inner join [Users] b on a.FromUserId = b.ID
			inner join [Users] c on a.ToUserId = c.ID
            Where FromUserId = '{userId}'";


            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult GetOtherUsers(int myUserId)
        {
            var query = $@"SELECT [ID]
      ,[LastName]
      ,[FirstName]
      ,[Type]
  FROM [DB_9FEBFD_cboseak].[dbo].[Users]
where ID <> {myUserId}";

            return DataTableToJson(ReadFromDataBase(query));
        }
  
        public ActionResult SendMessage(int fromUserId, int toUserId, string subject, string body)
        {
            var createdOn = DateTime.Now;
            var query = $@"insert into [dbo].[EdTechMessage] values ({fromUserId},{toUserId},'{subject}','{body}','{createdOn}')";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }


        public ActionResult MarkTaskAsCompleted(int taskId) {
            var query = $@"update [DB_9FEBFD_cboseak].[dbo].[EdTechTasks]  set Completed = 1 where ID = {taskId}";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTasksByToUserId(int userId)
        {
            var query = $@"SELECT a.[ID]
      ,a.[FromUserId]
      ,a.[ToUserId]
      ,a.[Name]
      ,a.[FileId]
	  ,b.FirstName as senderFirstName
	  ,b.LastName as senderLastName
	  ,c.FileName
,c.Type
,a.Completed
  FROM [DB_9FEBFD_cboseak].[dbo].[EdTechTasks] a
  inner join Users b on a.FromUserId = b.ID
  inner join EdTechFile c on a.FileId = c.ID
  where ToUserId = {userId}";

            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult GetTasksByFromUserId(int userId)
        {
            var query = $@"SELECT a.[ID]
      ,a.[FromUserId]
      ,a.[ToUserId]
      ,a.[Name]
      ,a.[FileId]
	  ,b.FirstName as senderFirstName
	  ,b.LastName as senderLastName
	  ,c.FileName
,c.Type
,a.Completed
  FROM [DB_9FEBFD_cboseak].[dbo].[EdTechTasks] a
  inner join Users b on a.ToUserId = b.ID
  inner join EdTechFile c on a.FileId = c.ID
  where a.FromUserId = {userId} and a.Completed = 1";

            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult DownloadFile(int fileId)
        {
            var query = $@" select FileData from [DB_9FEBFD_cboseak].[dbo].[EdTechFile] where ID = {fileId}";

            return DataTableToJson(ReadFromDataBase(query));
        }

  

        public FileResult Download(int fileId)
        {
            var query = $@" select FileData, FileName from [DB_9FEBFD_cboseak].[dbo].[EdTechFile] where ID = {fileId}";
            var dt = ReadFromDataBase(query);
            if(dt.Rows.Count > 0)
            {
                var base64 = dt.Rows[0]["FileData"].ToString();
                var fileName = dt.Rows[0]["FileName"].ToString(); ;
                byte[] byteArray = Convert.FromBase64String(base64);
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(new byte[0], System.Net.Mime.MediaTypeNames.Application.Octet, "error");

        }
        public ActionResult GetTasksByTaskId(int taskId)
        {
            var query = $@"SELECT a.[ID]
      ,a.[FromUserId]
      ,a.[ToUserId]
      ,a.[Name]
      ,a.[FileId]
	  ,b.FirstName as senderFirstName
	  ,b.LastName as senderLastName
	  ,c.FileName
	  ,c.Type
  FROM [DB_9FEBFD_cboseak].[dbo].[EdTechTasks] a
  inner join Users b on a.FromUserId = b.ID
  inner join EdTechFile c on a.FileId = c.ID
  where a.ID = {taskId}";

            return DataTableToJson(ReadFromDataBase(query));
        }

        public ActionResult GetUserInformationByUserId(int userId)
        {
            var query = $@"SELECT [ID]
      ,[LastName]
      ,[FirstName]
      ,[Type]
  FROM [DB_9FEBFD_cboseak].[dbo].[Users]
where ID = {userId}";

            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult SaveTask(int feedbackId, int rating, string comments)
        {
            var query = $@"insert into [dbo].[Rating] values ({feedbackId},{rating},'{comments}')";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFileById(int fileId)
        {
            var query = $@"SELECT [ID]
                          ,[FileData]
                          ,[FileName]
                          ,[Archived]
                      FROM [DB_9FEBFD_cboseak].[dbo].[EdTechFile]
                      where id = '{fileId}'";
            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult GetFeedbackByFromUserId(int userId)
        {
            var query = $@"SELECT *
                          FROM [dbo].[Feedback] a
                          inner join [dbo].[Rating] b on a.ID = b.FeedbackId
                          where a.FromUserId = '{userId}'";

            return DataTableToJson(ReadFromDataBase(query));
        }

        public ActionResult MessageByMessageId(int messageId)
        {

            var query = $@"SELECT  a.[ID]
              ,[FromUserId]
              ,[ToUserId]
              ,[Subject]
              ,[Body]
              ,[CreatedOn]
			  ,b.FirstName as senderFirstName
			  ,b.LastName as senderLastName
			  ,c.FirstName as toFirstName
			  ,c.LastName as toLastName
            FROM [DB_9FEBFD_cboseak].[dbo].[EdTechMessage] a
			inner join [Users] b on a.FromUserId = b.ID
			inner join [Users] c on a.ToUserId = c.ID
  where a.ID = {messageId}";

            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult GetFeedbackByToUserId(int userId)
        {
            var query = $@"SELECT *
                              FROM [dbo].[Feedback] a
                              inner join [dbo].[Rating] b on a.ID = b.FeedbackId
                              where a.ToUserId = '{userId}'";

            return DataTableToJson(ReadFromDataBase(query));
        }
        public ActionResult SaveRating(int fromUserId, int toUserId, string name, int fileId)
        {
            var query = $@"insert into [dbo].[EdTechTasks] values ({fromUserId},{toUserId},'{name}',{fileId})";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFeedback(int lessonPlanId, int fromUserId,int toUserId, string comments)
        {
            var query = $@"insert into [dbo].[EdTechTasks] values ({lessonPlanId},{fromUserId},{toUserId},'{comments}')";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string name, int type)
        {
            var base64 = "";
            using (BinaryReader theReader = new BinaryReader(file.InputStream))
            {
                 base64 = Convert.ToBase64String(theReader.ReadBytes(file.ContentLength));

            }

            var query = $@"insert into [dbo].[EdTechFile] values ('{base64}','{name}',0,{type});  SELECT max(ID) FROM [DB_9FEBFD_cboseak].[dbo].[EdTechFile];";
            return Json(WriteToDbGetBackId(query));
        }
        public ActionResult AddTask(int fromUserId, int toUserId,string name, int fileId)
        {
            var query = $@" insert into [DB_9FEBFD_cboseak].[dbo].[EdTechTasks] values ({fromUserId},{toUserId},'{name}',{fileId},0)";
            return Json(WriteToDb(query), JsonRequestBehavior.AllowGet);
        }

        #region helper methods




        static string GetConnectionString()
        {
            return "Data Source=SQL5026.site4now.net;Initial Catalog=DB_9FEBFD_cboseak;User Id=DB_9FEBFD_cboseak_admin;Password=Fairway100!;";
        }



        int WriteToDb(string query)
        {
            try
            {
                string connectionString = GetConnectionString();
                List<string> ret = new List<string>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 500;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return 1;
            }
            catch
            {
                return 0;
            }




        }

        int WriteToDbGetBackId(string query)
        {
            try
            {
                string connectionString = GetConnectionString();
                List<string> ret = new List<string>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 500;
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch
            {
                return -1;
            }




        }

        private ActionResult DataTableToJson(DataTable dt)
        {

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }
            catch
            {

            }
            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(rows, Formatting.Indented, jsonSerializerSettings);
            return Json(json, "application/json", JsonRequestBehavior.AllowGet);

        }
        DataTable ReadFromDataBase(string queryString)
        {
            var connectionString = GetConnectionString();
            List<string> ret = new List<string>();
            var dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandTimeout = 500;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {

                    dataTable.Load(reader);
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
            return dataTable;
        }

        #endregion
    }
}