namespace MainApp.Models
{
    public class Project
    {
        public Project(string projectName, string projectUrl, int? registrationNo)
        {
            this.ProjectName = projectName;
            this.ProjectUrl = projectUrl;
            this.RegistrationNo = registrationNo;
        }

        public string ProjectName { get; set; }
        public string ProjectUrl { get; set; }
        public int? RegistrationNo { get; set; }
    }

}
