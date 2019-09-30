using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C4iTaskManager
{
    public class Task
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDetail { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public bool TaskComplete { get; set; }

        public Task()
        {
            this.TaskId = TaskId;
            this.TaskName = TaskName;
            this.TaskDetail = TaskDetail;
            this.TaskDueDate = TaskDueDate;
            this.TaskComplete = TaskComplete;
        }

        public override string ToString()
        {
            return this.TaskId.ToString() + "\n" +
                this.TaskName.ToString() + "\n" +
                this.TaskDetail.ToString() + "\n" +
                this.TaskDueDate.ToString() + "\n" +
                this.TaskComplete.ToString();
        }
    }
}
