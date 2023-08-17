using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrainingApp.Models;
using TrainingApp.Utils;
using System.IO;


namespace TrainingApp.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, Username, Email, PasswordHash, MaxBench, MaxSquat, MaxDeadlift
                FROM [User]"; 

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User newUser = new User()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Username = DbUtils.GetString(reader, "Username"),
                                Email = DbUtils.GetString(reader, "Email"),
                                PasswordHash = DbUtils.GetString(reader, "PasswordHash"),
                                MaxBench = DbUtils.GetInt(reader, "MaxBench"),
                                MaxSquat = DbUtils.GetInt(reader, "MaxSquat"),
                                MaxDeadlift = DbUtils.GetInt(reader, "MaxDeadlift")
                            };

                            users.Add(newUser);
                        }
                    }
                }
            }

            return users;
        }

        public bool ValidateUser(string email, string plainPassword)
        {
            User user = GetByEmail(email);
            if (user == null)
            {
                return false;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash);

            return isValidPassword;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            string secretFromFile = File.ReadAllText("secrets.txt").Trim();
            var key = Encoding.ASCII.GetBytes(secretFromFile);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public int AddUser(User user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO [User] (username, email, passwordHash, maxSquat, maxBench, maxDeadlift) 
                                        OUTPUT INSERTED.ID 
                                        VALUES (@username, @email, @passwordHash, @maxSquat, @maxBench, @maxDeadlift)";
                    DbUtils.AddParameter(cmd, "@username", user.Username);
                    DbUtils.AddParameter(cmd, "@email", user.Email);
                    DbUtils.AddParameter(cmd, "@passwordHash", user.PasswordHash);
                    DbUtils.AddParameter(cmd, "@maxSquat", user.MaxSquat);
                    DbUtils.AddParameter(cmd, "@maxBench", user.MaxBench);
                    DbUtils.AddParameter(cmd, "@maxDeadlift", user.MaxDeadlift);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }


        public User GetByEmail(string email)
        {
            User user = null;

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, Username, Email, PasswordHash, MaxBench, MaxSquat, MaxDeadlift
                FROM [User]
                WHERE Email = @Email";

                    DbUtils.AddParameter(cmd, "@Email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Username = DbUtils.GetString(reader, "Username"),
                                Email = DbUtils.GetString(reader, "Email"),
                                PasswordHash = DbUtils.GetString(reader, "PasswordHash"),
                                MaxBench = DbUtils.GetInt(reader, "MaxBench"),
                                MaxSquat = DbUtils.GetInt(reader, "MaxSquat"),
                                MaxDeadlift = DbUtils.GetInt(reader, "MaxDeadlift")
                            };
                        }
                    }
                }
            }

            return user;
        }



        public User GetUserById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, username, email, passwordHash, maxSquat, maxBench, maxDeadlift FROM [User] WHERE id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);

                    User user = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                Id = DbUtils.GetInt(reader, "id"),
                                Username = DbUtils.GetString(reader, "username"),
                                Email = DbUtils.GetString(reader, "email"),
                                PasswordHash = DbUtils.GetString(reader, "passwordHash"),
                                MaxSquat = DbUtils.GetInt(reader, "maxSquat"),
                                MaxBench = DbUtils.GetInt(reader, "maxBench"),
                                MaxDeadlift = DbUtils.GetInt(reader, "maxDeadlift")
                            };
                        }
                    }

                    return user;
                }
            }
        }


        public void UpdateUser(User user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
        UPDATE [User]
        SET Username = @Username, 
            Email = @Email, 
            MaxBench = @MaxBench, 
            MaxSquat = @MaxSquat, 
            MaxDeadlift = @MaxDeadlift
        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Username", user.Username);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@MaxBench", user.MaxBench);
                    DbUtils.AddParameter(cmd, "@MaxSquat", user.MaxSquat);
                    DbUtils.AddParameter(cmd, "@MaxDeadlift", user.MaxDeadlift);
                    DbUtils.AddParameter(cmd, "@Id", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void DeleteUser(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [User] WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
