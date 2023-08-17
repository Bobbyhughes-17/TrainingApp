using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface ITrainingProgramRepository
    {
        List<TrainingProgram> GetAll();
        TrainingProgram GetById(int id);
        void Add(TrainingProgram trainingProgram);
        void Update(TrainingProgram trainingProgram);
        void Delete(int id);
    }
}
