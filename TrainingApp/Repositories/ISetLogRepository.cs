using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface ISetLogRepository
    {
        List<SetLog> GetAll();
        List<SetLog> GetByUserIdAndDate(int userId, DateTime? date);
        SetLog GetById(int id);
        int Add(SetLog setLog);
        void Update(SetLog setLog);
        void Delete(int id);
    }
}
