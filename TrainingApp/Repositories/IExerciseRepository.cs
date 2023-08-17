using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IExerciseRepository
    {
        List<Exercise> GetAll();
        List<Exercise> GetByMuscleGroupId(int muscleGroupId);
        Exercise GetById(int id);
        void Add(Exercise exercise);
        void Update(Exercise exercise);
        void Delete(int id);
    }
}
