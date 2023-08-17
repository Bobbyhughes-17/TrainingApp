using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TrainingApp.Models;

namespace TrainingApp.Repositories
{
    public class SetLogRepository : ISetLogRepository
    {
        private readonly IConfiguration _config;

        public SetLogRepository(IConfiguration config)
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
        public List<SetLog> GetByUserIdAndDate(int userId, DateTime? date)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, UserId, ExerciseId, Weight, Repetitions, Date 
                                FROM SetLog
                                WHERE UserId = @userId AND Date = @date";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@date", date);

                    var reader = cmd.ExecuteReader();
                    var setLogs = new List<SetLog>();

                    while (reader.Read())
                    {
                        setLogs.Add(new SetLog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Weight = (float)reader.GetDouble(reader.GetOrdinal("Weight")),
                            Repetitions = reader.GetInt32(reader.GetOrdinal("Repetitions")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                        });
                    }

                    reader.Close();
                    return setLogs;
                }
            }
        }

        public List<SetLog> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, UserId, ExerciseId, Weight, Repetitions, Date FROM SetLog";
                    var reader = cmd.ExecuteReader();
                    var setLogs = new List<SetLog>();
                    while (reader.Read())
                    {
                        setLogs.Add(new SetLog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Weight = (float)reader.GetDouble(reader.GetOrdinal("Weight")),
                            Repetitions = reader.GetInt32(reader.GetOrdinal("Repetitions")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                        });
                    }
                    reader.Close();
                    return setLogs;
                }
            }
        }

        public SetLog GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT UserId, ExerciseId, Weight, Repetitions, Date FROM SetLog WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new SetLog
                        {
                            Id = id,
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                            Weight = (float)reader.GetDouble(reader.GetOrdinal("Weight")),
                            Repetitions = reader.GetInt32(reader.GetOrdinal("Repetitions")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                        };
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public int Add(SetLog setLog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO SetLog (UserId, ExerciseId, Weight, Repetitions, Date) 
                                        OUTPUT INSERTED.ID 
                                        VALUES (@userId, @exerciseId, @weight, @repetitions, @date)";
                    cmd.Parameters.AddWithValue("@userId", setLog.UserId);
                    cmd.Parameters.AddWithValue("@exerciseId", setLog.ExerciseId);
                    cmd.Parameters.AddWithValue("@weight", setLog.Weight);
                    cmd.Parameters.AddWithValue("@repetitions", setLog.Repetitions);
                    cmd.Parameters.AddWithValue("@date", setLog.Date);

                    int id = (int)cmd.ExecuteScalar();
                    return id;
                }
            }
        }

        public void Update(SetLog setLog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE SetLog 
                                        SET UserId = @userId, 
                                            ExerciseId = @exerciseId, 
                                            Weight = @weight, 
                                            Repetitions = @repetitions, 
                                            Date = @date 
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", setLog.Id);
                    cmd.Parameters.AddWithValue("@userId", setLog.UserId);
                    cmd.Parameters.AddWithValue("@exerciseId", setLog.ExerciseId);
                    cmd.Parameters.AddWithValue("@weight", setLog.Weight);
                    cmd.Parameters.AddWithValue("@repetitions", setLog.Repetitions);
                    cmd.Parameters.AddWithValue("@date", setLog.Date);

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
                    cmd.CommandText = "DELETE FROM SetLog WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
