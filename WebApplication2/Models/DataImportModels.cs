using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DataImportModels
    {
        public HttpPostedFileBase file { get; set; }
        public bool ImportUsers { get; set; }
        public bool ImportProject { get; set; }
        public bool ImportTask { get; set; }

    }

    public class ImportStatus
    {
        public List<ImportResult> Users { get; private set; } = new List<ImportResult>();
        public List<ImportResult> Tasks { get; private set; } = new List<ImportResult>();
        public List<ImportResult> Projects { get; private set; } = new List<ImportResult>();
    }

    public class ImportResult
    {
        public bool Succeeded;
        public string Name;
        public string FailReason;
    }

    public class UserXML
    {
        public string Email;
        public string UserName;
        public string FirstName;
        public string LastName;
        public string BirthDate;
        public string Role;
    }

    public class ProjectXML
    {
        public string Owner;
        public string Name;
        public string Description;
        public DateTime StartDate;
        public DateTime DueDate;
        public bool Completed;
        public DateTime? CompletedOn;
        public bool Archived;
        public int Budget;
        public int Spent;
    }

    public class TaskXML
    {
        public string ProjectName;
        public string Name;
        public string Description;
        public DateTime Started;
        public DateTime Due;
        public bool Completed;
        public DateTime CompletedOn;
    }
}