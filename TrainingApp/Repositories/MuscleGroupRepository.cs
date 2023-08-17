using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TrainingApp.Models;
using TrainingApp.Repositories;

namespace TrainingApp.Repositories
{
    public class MuscleGroupRepository : IMuscleGroupRepository
    {
        private readonly IConfiguration _config;
        public MuscleGroupRepository(IConfiguration config)
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

        public List<MuscleGroup> GetAll()
        {
            List<MuscleGroup> muscleGroups = new List<MuscleGroup>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM MuscleGroup";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        MuscleGroup muscleGroup = new MuscleGroup
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        muscleGroups.Add(muscleGroup);
                    }
                }
            }

            return muscleGroups;
        }

        public MuscleGroup GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM MuscleGroup WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        MuscleGroup muscleGroup = new MuscleGroup
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        return muscleGroup;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void Add(MuscleGroup muscleGroup)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO MuscleGroup (Name) VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", muscleGroup.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(MuscleGroup muscleGroup)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE MuscleGroup SET Name = @name WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", muscleGroup.Name);
                    cmd.Parameters.AddWithValue("@id", muscleGroup.Id);
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
                    cmd.CommandText = "DELETE FROM MuscleGroup WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
