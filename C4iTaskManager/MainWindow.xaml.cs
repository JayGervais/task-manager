using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C4iTaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FormRefresh();
            lblDate.Content = DateTime.Now; 

            TaskDB currentTasks = new TaskDB();
            currentTasks.GetTasks(lstTasks, false);
            currentTasks.GetTasks(lstComplete, true);

            lstComplete.ItemContainerGenerator.StatusChanged += new EventHandler(ContainerStatusChanged);
        }

        private void ContainerStatusChanged(object sender, EventArgs e)
        {
            if (lstComplete.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                lstComplete.Foreground = Brushes.Green;
            }
        }

        protected void LstTasks_SelectionChanged(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                if (DateTime.Parse(e.Row.Cells[0].Text) < DateTime.Now)
                {
                    lstTasks.ItemContainerGenerator.StatusChanged += new EventHandler(ContainerStatusChanged);
                }
            }
        }

        private void LstTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListSelections(lstTasks);
            btnClear.IsEnabled = true;
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void LstComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListSelections(lstComplete);
            btnClear.IsEnabled = true;
            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void ListSelections(ListView list)
        {
            list.SelectedValuePath = "TaskId";

            if (list.SelectedValue != null)
            {
                int id = Convert.ToInt32(list.SelectedValue);
                Task task = TaskDB.GetTaskDetails(id);
                txtTask.Text = task.TaskName;
                txtDescription.Text = task.TaskDetail;
                dateDue.SelectedDate = task.TaskDueDate;
                cbComplete.IsChecked = task.TaskComplete;
            }
           
        }

        private int ListChange()
        {
            if (lstComplete.SelectedValue != null)
            {
                lstTasks.SelectedValue = null;
                lstComplete.SelectedValuePath = "TaskId";
                int id = Convert.ToInt32(lstComplete.SelectedValue);
                return id;
            }
            else
            {
                lstComplete.SelectedValue = null;
                lstTasks.SelectedValuePath = "TaskId";
                int id = Convert.ToInt32(lstTasks.SelectedValue);
                return id;
            }
        }

        public void ListRefresh()
        {
            TaskDB refresh = new TaskDB();
            refresh.GetTasks(lstComplete, true);
            refresh.GetTasks(lstTasks, false);
        }

        private void FormRefresh()
        {
            txtTask.Text = "";
            txtDescription.Text = "";
            dateDue.SelectedDate = null;
            cbComplete.IsChecked = false;

            btnClear.IsEnabled = false;
            btnAdd.IsEnabled = true;
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();
            task.TaskName = txtTask.Text;
            task.TaskDetail = txtDescription.Text;
            task.TaskDueDate = dateDue.SelectedDate;
            task.TaskComplete = cbComplete.IsChecked.Value;
            TaskDB.AddTask(task);

            FormRefresh();
            ListRefresh();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Task newTask = new Task();
            newTask.TaskId = ListChange();

            newTask.TaskName = txtTask.Text;
            newTask.TaskDetail = txtDescription.Text;
            if (dateDue.SelectedDate.HasValue)
            {
                newTask.TaskDueDate = dateDue.SelectedDate;
            }
            else
            {
                newTask.TaskDueDate = null;
            }

            if (cbComplete.IsChecked == true)
            {
                newTask.TaskComplete = true;
            }
            else
            {
                newTask.TaskComplete = false;
            }

            TaskDB.UpdateTask(newTask);
            FormRefresh();
            ListRefresh();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int id = ListChange();
            TaskDB.DeleteTask(id);
            FormRefresh();
            ListRefresh();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            FormRefresh();
        }


    }
}
