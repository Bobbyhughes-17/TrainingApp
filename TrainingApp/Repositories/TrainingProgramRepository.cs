using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Repositories
{
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly IConfiguration _config;
        public TrainingProgramRepository(IConfiguration config)
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

        public List<TrainingProgram> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, DaysPerWeek FROM TrainingProgram";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> trainingPrograms = new List<TrainingProgram>();
                    while (reader.Read())
                    {
                        TrainingProgram trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            DaysPerWeek = reader.GetInt32(reader.GetOrdinal("DaysPerWeek"))
                        };

                        trainingPrograms.Add(trainingProgram);
                    }

                    reader.Close();
                    return trainingPrograms;
                }
            }
        }

        public TrainingProgram GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, DaysPerWeek FROM TrainingProgram WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        TrainingProgram trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            DaysPerWeek = reader.GetInt32(reader.GetOrdinal("DaysPerWeek"))
                        };

                        reader.Close();
                        return trainingProgram;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public void Add(TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO TrainingProgram (Name, DaysPerWeek) 
                                OUTPUT INSERTED.ID
                                VALUES (@name, @daysPerWeek)";
                    cmd.Parameters.AddWithValue("@name", trainingProgram.Name);
                    cmd.Parameters.AddWithValue("@daysPerWeek", trainingProgram.DaysPerWeek);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE TrainingProgram 
                                SET Name = @name, DaysPerWeek = @daysPerWeek 
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", trainingProgram.Name);
                    cmd.Parameters.AddWithValue("@daysPerWeek", trainingProgram.DaysPerWeek);
                    cmd.Parameters.AddWithValue("@id", trainingProgram.Id);

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
                    cmd.CommandText = "DELETE FROM TrainingProgram WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
