namespace ManageNotification.Models
{
    public class SaveSubProjectsModel
    {
        public int MainProjectId { get; set; }

        public List<SubProject> SubProjects { get; set; }

        public bool IsAssigned { get; set; }
    }
}
