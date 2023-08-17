using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetUserById(int id);
        User GetByEmail(string email);
        int AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        string GenerateToken(User user);
        bool ValidateUser(string email, string plainPassword);

    }
}
