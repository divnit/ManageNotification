namespace ManageNotification.Models
{
    public class ProjectReferenceViewModel
    {
        public int MainProjectId { get; set; }
        public int SubProjectId { get; set; }
        public string SubProjectName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
