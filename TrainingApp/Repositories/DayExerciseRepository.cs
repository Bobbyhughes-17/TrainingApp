using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public class DayExerciseRepository : IDayExerciseRepository
    {
        private readonly IConfiguration _config;

        public DayExerciseRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public int GetDayIdByDayNumber(int dayNumber)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id FROM Day WHERE dayNumber = @dayNumber";  
                    cmd.Parameters.AddWithValue("@dayNumber", dayNumber);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                    reader.Close();
                    return 0; 
                }
            }
        }
        public List<Exercise> GetExercisesByUserIdAndDayNumber(int userId, int dayNumber)
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT e.*
                FROM DayExercise de
                JOIN Day d ON d.id = de.dayID
                JOIN UserProgram up ON up.trainingProgramID = d.trainingProgramID
                JOIN Exercise e ON e.id = de.exerciseID
                WHERE up.userID = @userId AND d.dayNumber = @dayNumber
            ";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@dayNumber", dayNumber);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MuscleGroupId = reader.GetInt32(reader.GetOrdinal("MuscleGroupID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        });
                    }

                    reader.Close();
                }
            }
            return exercises;
        }


        public List<DayExercise> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, DayId, ExerciseId FROM DayExercise";
                    var reader = cmd.ExecuteReader();
                    var dayExercises = new List<DayExercise>();
                    while (reader.Read())
                    {
                        dayExercises.Add(new DayExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            DayId = reader.GetInt32(reader.GetOrdinal("DayId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"))
                        });
                    }
                    reader.Close();
                    return dayExercises;
                }
            }
        }
        public List<DayExercise> GetByDayId(int dayId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, DayId, ExerciseId FROM DayExercise WHERE DayId = @dayId";
                    cmd.Parameters.AddWithValue("@dayId", dayId);
                    var reader = cmd.ExecuteReader();

                    var dayExercises = new List<DayExercise>();

                    while (reader.Read())
                    {
                        dayExercises.Add(new DayExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            DayId = reader.GetInt32(reader.GetOrdinal("DayId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"))
                        });
                    }
                    reader.Close();
                    return dayExercises;
                }
            }
        }

        public DayExercise GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT DayId, ExerciseId FROM DayExercise WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new DayExercise
                        {
                            Id = id,
                            DayId = reader.GetInt32(reader.GetOrdinal("DayId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"))
                        };
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public int Add(DayExercise dayExercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO DayExercise (DayId, ExerciseId) 
                                OUTPUT INSERTED.ID 
                                VALUES (@dayId, @exerciseId)";
                    cmd.Parameters.AddWithValue("@dayId", dayExercise.DayId);
                    cmd.Parameters.AddWithValue("@exerciseId", dayExercise.ExerciseId);

                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
            }
        }

        public void Update(DayExercise dayExercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE DayExercise 
                                SET DayId = @dayId, 
                                    ExerciseId = @exerciseId
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", dayExercise.Id);
                    cmd.Parameters.AddWithValue("@dayId", dayExercise.DayId);
                    cmd.Parameters.AddWithValue("@exerciseId", dayExercise.ExerciseId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM DayExercise WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
