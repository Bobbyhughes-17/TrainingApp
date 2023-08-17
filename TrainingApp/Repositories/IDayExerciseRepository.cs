using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IDayExerciseRepository
    {
        List<DayExercise> GetAll();
        List<DayExercise> GetByDayId(int dayId);
        List<Exercise> GetExercisesByUserIdAndDayNumber(int userId, int dayNumber);
        int GetDayIdByDayNumber(int dayNumber);
        DayExercise GetById(int id);
        int Add(DayExercise dayExercise);
        void Update(DayExercise dayExercise);
        void Delete(int id);
    }
}
