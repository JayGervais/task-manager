using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C4iTaskManager
{
    public class TaskDB
    {
        public void GetTasks(ListBox listbox, bool complete)
        {
            SqlConnection con = Connection.GetConnection();
            try
            {
                string taskQuery = @"SELECT TaskId, TaskName, TaskDetail, TaskDueDate, TaskComplete FROM Tasks WHERE TaskComplete=@complete";
                SqlCommand sqlCommand = new SqlCommand(taskQuery, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@complete", complete);
                    DataTable currentTaskTable = new DataTable();
                    sqlDataAdapter.Fill(currentTaskTable);
                    listbox.SelectedValuePath = "TaskId";
                    listbox.ItemsSource = currentTaskTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static Task GetTaskDetails(int taskId)
        {
            Task task = null;

            SqlConnection con = Connection.GetConnection();
            string taskQuery = @"SELECT TaskName, TaskDetail, TaskDueDate, TaskComplete FROM Tasks WHERE TaskId=@taskId";
            SqlCommand sqlCommand = new SqlCommand(taskQuery, con);
            sqlCommand.Parameters.AddWithValue("@TaskId", taskId);

            try
            {
                con.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    task = new Task();
                    task.TaskName = reader["TaskName"].ToString();
                    task.TaskDetail = reader["TaskDetail"].ToString();
                    if (reader["TaskDueDate"] == DBNull.Value)
                    {
                        task.TaskDueDate = null;
                    }
                    else
                    {
                        task.TaskDueDate = Convert.ToDateTime(reader["TaskDueDate"]);
                    }
                    
                    task.TaskComplete = (bool)reader["TaskComplete"];
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return task;
        }

        public static int AddTask(Task task)
        {
            SqlConnection con = Connection.GetConnection();
            string insertStatement = "INSERT INTO Tasks (TaskName, TaskDetail, TaskDueDate, TaskComplete) " +
                                     "VALUES(@TaskName, @TaskDetail, @TaskDueDate, @TaskComplete)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            cmd.Parameters.AddWithValue("@TaskName", task.TaskName);
            cmd.Parameters.AddWithValue("@TaskDetail", task.TaskDetail);
            if (task.TaskDueDate == null)
            {
                cmd.Parameters.AddWithValue("@TaskDueDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TaskDueDate", task.TaskDueDate);
            }
            
            cmd.Parameters.AddWithValue("@TaskComplete", task.TaskComplete);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string selectQuery = "SELECT IDENT_CURRENT('Tasks') FROM Tasks";
                SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                int TaskId = Convert.ToInt32(selectCmd.ExecuteScalar()); 

                return TaskId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static bool UpdateTask(Task newTask)
        {
            SqlConnection con = Connection.GetConnection();
            string updateStatement = "UPDATE Tasks " +
                                     "SET TaskName = @NewTaskName, " +
                                     "TaskDetail = @NewTaskDetail, " +
                                     "TaskDueDate = @NewTaskDueDate, " +
                                     "TaskComplete = @NewTaskComplete " +
                                     "WHERE TaskId = @TaskId";
            SqlCommand cmd = new SqlCommand(updateStatement, con);
            cmd.Parameters.AddWithValue("@TaskId", newTask.TaskId);
            cmd.Parameters.AddWithValue("@NewTaskName", newTask.TaskName);
            cmd.Parameters.AddWithValue("@NewTaskDetail", newTask.TaskDetail);
            if (newTask.TaskDueDate == null)
            {
                cmd.Parameters.AddWithValue("@NewTaskDueDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@NewTaskDueDate", newTask.TaskDueDate);
            }
            cmd.Parameters.AddWithValue("@NewTaskComplete", newTask.TaskComplete);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0) return true;
                else return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static bool DeleteTask(int id)
        {
            SqlConnection con = Connection.GetConnection();
            string deleteStatement = "DELETE FROM Tasks " +
                                     "WHERE TaskId = @TaskId";
            SqlCommand cmd = new SqlCommand(deleteStatement, con);
            cmd.Parameters.AddWithValue("@TaskId", id);
            try
            {
                con.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0) return true;
                else return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
