namespace TrainingApp.Models
{
    public class UserProgram
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TrainingProgramId { get; set; }
        public DateTime StartDate { get; set; }
        public int CurrentDay { get; set; }
    }
}
