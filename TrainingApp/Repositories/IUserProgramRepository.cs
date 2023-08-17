using System.Collections.Generic;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IUserProgramRepository
    {
        List<UserProgram> GetAll();
        UserProgram GetById(int id);
        UserProgram GetByUserId(int userId);
        int Add(UserProgram userProgram);
        void Update(UserProgram userProgram);
        void Delete(int id);

    }
}
