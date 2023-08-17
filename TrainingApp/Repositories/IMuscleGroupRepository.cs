using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IMuscleGroupRepository
    {
        List<MuscleGroup> GetAll();
        MuscleGroup GetById(int id);
        void Add(MuscleGroup muscleGroup);
        void Update(MuscleGroup muscleGroup);
        void Delete(int id);
    }
}
