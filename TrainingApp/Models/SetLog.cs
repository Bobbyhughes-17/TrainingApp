namespace TrainingApp.Models
{
    public class SetLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public float Weight { get; set; } 
        public int Repetitions { get; set; }
        public DateTime Date { get; set; }
    }
}
