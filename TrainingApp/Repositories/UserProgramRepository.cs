using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public class UserProgramRepository : IUserProgramRepository
    {
        private readonly IConfiguration _config;

        public UserProgramRepository(IConfiguration config)
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

        public UserProgram GetByUserId(int userId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, UserId, TrainingProgramId, StartDate, CurrentDay FROM UserProgram WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new UserProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            CurrentDay = reader.GetInt32(reader.GetOrdinal("CurrentDay"))
                        };
                    }
                    reader.Close();
                    return null;
                }
            }
        }


        public List<UserProgram> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, UserId, TrainingProgramId, StartDate, CurrentDay FROM UserProgram";
                    var reader = cmd.ExecuteReader();
                    var userPrograms = new List<UserProgram>();
                    while (reader.Read())
                    {
                        userPrograms.Add(new UserProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            CurrentDay = reader.GetInt32(reader.GetOrdinal("CurrentDay"))
                        });
                    }
                    reader.Close();
                    return userPrograms;
                }
            }
        }

        public UserProgram GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT UserId, TrainingProgramId, StartDate, CurrentDay FROM UserProgram WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new UserProgram
                        {
                            Id = id,
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            TrainingProgramId = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            CurrentDay = reader.GetInt32(reader.GetOrdinal("CurrentDay"))
                        };
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public int Add(UserProgram userProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProgram (UserId, TrainingProgramId, StartDate, CurrentDay) 
                                        OUTPUT INSERTED.ID 
                                        VALUES (@userId, @trainingProgramId, @startDate, @currentDay)";
                    cmd.Parameters.AddWithValue("@userId", userProgram.UserId);
                    cmd.Parameters.AddWithValue("@trainingProgramId", userProgram.TrainingProgramId);
                    cmd.Parameters.AddWithValue("@startDate", userProgram.StartDate);
                    cmd.Parameters.AddWithValue("@currentDay", userProgram.CurrentDay);

                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
            }
        }

        public void Update(UserProgram userProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE UserProgram 
                                        SET UserId = @userId, 
                                            TrainingProgramId = @trainingProgramId, 
                                            StartDate = @startDate, 
                                            CurrentDay = @currentDay 
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", userProgram.Id);
                    cmd.Parameters.AddWithValue("@userId", userProgram.UserId);
                    cmd.Parameters.AddWithValue("@trainingProgramId", userProgram.TrainingProgramId);
                    cmd.Parameters.AddWithValue("@startDate", userProgram.StartDate);
                    cmd.Parameters.AddWithValue("@currentDay", userProgram.CurrentDay);

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
                    cmd.CommandText = "DELETE FROM UserProgram WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
