namespace SiteFuel.Exchange.ViewModels.Job
{
    public class OnsiteJobUserViewModel
    {
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name
        {
            get { return FirstName + " " + LastName; }
        }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int OnboardedTypeId { get; set; }
    }
}
