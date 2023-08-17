using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly IConfiguration _config;
        public ExerciseRepository(IConfiguration config)
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
        public List<Exercise> GetAll()
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, MuscleGroupId, Name, Description 
                                        FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MuscleGroupId = reader.GetInt32(reader.GetOrdinal("MuscleGroupId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };

                        exercises.Add(exercise);
                    }
                }
            }

            return exercises;
        }

        public List<Exercise> GetByMuscleGroupId(int muscleGroupId)
        {
            List<Exercise> exercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, MuscleGroupId, Name, Description 
                                FROM Exercise WHERE MuscleGroupId = @muscleGroupId";
                    cmd.Parameters.AddWithValue("@muscleGroupId", muscleGroupId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MuscleGroupId = reader.GetInt32(reader.GetOrdinal("MuscleGroupId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };

                        exercises.Add(exercise);
                    }
                }
            }

            return exercises;
        }

        public Exercise GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT MuscleGroupId, Name, Description 
                                        FROM Exercise WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Exercise exercise = new Exercise
                        {
                            Id = id,
                            MuscleGroupId = reader.GetInt32(reader.GetOrdinal("MuscleGroupId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };

                        return exercise;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void Add(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Exercise (MuscleGroupId, Name, Description) 
                                        VALUES (@muscleGroupId, @name, @description)";
                    cmd.Parameters.AddWithValue("@muscleGroupId", exercise.MuscleGroupId);
                    cmd.Parameters.AddWithValue("@name", exercise.Name);
                    cmd.Parameters.AddWithValue("@description", exercise.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Exercise 
                                        SET MuscleGroupId = @muscleGroupId, 
                                            Name = @name, 
                                            Description = @description 
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@muscleGroupId", exercise.MuscleGroupId);
                    cmd.Parameters.AddWithValue("@name", exercise.Name);
                    cmd.Parameters.AddWithValue("@description", exercise.Description);
                    cmd.Parameters.AddWithValue("@id", exercise.Id);
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
                    cmd.CommandText = "DELETE FROM Exercise WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
