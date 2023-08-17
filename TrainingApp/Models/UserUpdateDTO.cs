namespace TrainingApp.Models
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int MaxSquat { get; set; }
        public int MaxBench { get; set; }
        public int MaxDeadlift { get; set; }
    }
}
