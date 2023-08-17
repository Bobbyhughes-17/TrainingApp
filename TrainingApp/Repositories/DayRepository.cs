using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public class DayRepository : IDayRepository
    {
        private readonly IConfiguration _config;

        public DayRepository(IConfiguration config)
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

        public List<Day> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, TrainingProgramId, DayNumber, Title FROM Day";
                    var reader = cmd.ExecuteReader();
                    var days = new List<Day>();
                    while (reader.Read())
                    {
                        days.Add(new Day
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            DayNumber = reader.GetInt32(reader.GetOrdinal("DayNumber")),
                            Title = reader.GetString(reader.GetOrdinal("Title"))
                        });
                    }
                    reader.Close();
                    return days;
                }
            }
        }

        public Day GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT TrainingProgramId, DayNumber, Title FROM Day WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Day
                        {
                            Id = id,
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            DayNumber = reader.GetInt32(reader.GetOrdinal("DayNumber")),
                            Title = reader.GetString(reader.GetOrdinal("Title"))
                        };
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public List<Day> GetDaysByTrainingProgramId(int trainingProgramId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, TrainingProgramId, DayNumber, Title FROM Day WHERE TrainingProgramId = @trainingProgramId";
                    cmd.Parameters.AddWithValue("@trainingProgramId", trainingProgramId);
                    var reader = cmd.ExecuteReader();

                    var days = new List<Day>();
                    while (reader.Read())
                    {
                        days.Add(new Day
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            DayNumber = reader.GetInt32(reader.GetOrdinal("DayNumber")),
                            Title = reader.GetString(reader.GetOrdinal("Title"))
                        });
                    }
                    reader.Close();
                    return days;
                }
            }
        }


        public int Add(Day day)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Day (TrainingProgramId, DayNumber, Title) 
                                OUTPUT INSERTED.ID 
                                VALUES (@trainingProgramId, @dayNumber, @title)";
                    cmd.Parameters.AddWithValue("@trainingProgramId", day.TrainingProgramId);
                    cmd.Parameters.AddWithValue("@dayNumber", day.DayNumber);
                    cmd.Parameters.AddWithValue("@title", day.Title);

                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
            }
        }

        public void Update(Day day)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Day 
                                SET TrainingProgramId = @trainingProgramId, 
                                    DayNumber = @dayNumber, 
                                    Title = @title
                                WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", day.Id);
                    cmd.Parameters.AddWithValue("@trainingProgramId", day.TrainingProgramId);
                    cmd.Parameters.AddWithValue("@dayNumber", day.DayNumber);
                    cmd.Parameters.AddWithValue("@title", day.Title);

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
                    cmd.CommandText = "DELETE FROM Day WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
