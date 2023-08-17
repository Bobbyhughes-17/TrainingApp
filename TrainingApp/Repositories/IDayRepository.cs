using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IDayRepository
    {
        List<Day> GetAll();
        Day GetById(int id);
        int Add(Day day);
        void Update(Day day);
        void Delete(int id);
        List<Day> GetDaysByTrainingProgramId(int trainingProgramId);
    }
}
