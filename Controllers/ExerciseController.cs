using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercises.Models;

namespace StudentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ExerciseController(IConfiguration config)
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

        [HttpGet]
        public async Task<IActionResult> Get(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "student")
                    {
                        cmd.CommandText = @"SELECT e.Id AS TheExerciseId, e.Name, e.Language, s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle 
                                            FROM Exercise e
                                            INNER JOIN StudentExercise se
                                            ON e.Id = se.ExerciseId
                                            INNER JOIN Student s
                                            ON s.Id = se.StudentId";
                        SqlDataReader reader = cmd.ExecuteReader();
                        Dictionary<int, Exercise> exercises = new Dictionary<int, Exercise>();

                        while (reader.Read())
                        {
                            int exerciseId = reader.GetInt32(reader.GetOrdinal("TheExerciseId"));
                            if (!exercises.ContainsKey(exerciseId))
                            {
                                Exercise exercise = GetExercise(reader, exerciseId);
                                exercises.Add(exerciseId, exercise);
                            }
                            Exercise fromDictionary = exercises[exerciseId];
                            if (!reader.IsDBNull(reader.GetOrdinal("StudentId")))
                            {
                                Student student = new Student()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                };
                                fromDictionary.Students.Add(student);
                            }
                        }
                        reader.Close();

                        return Ok(exercises.Values);
                    }
                    else if (q != null)
                    {
                        cmd.CommandText = @"SELECT Id AS ExerciseId, Name, Language
                                            FROM Exercise
                                            WHERE Name LIKE @searchString
                                            OR Language LIKE @searchString;";

                        cmd.Parameters.Add(new SqlParameter("@searchString", "%" + q + "%"));

                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Exercise> exercises = new Dictionary<int, Exercise>();

                        while (reader.Read())
                        {
                            int exerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                            if (!exercises.ContainsKey(exerciseId))
                            {
                                Exercise exercise = GetExercise(reader, exerciseId);
                                exercises.Add(exerciseId, exercise);
                            }
                        }
                        reader.Close();
                        return Ok(exercises.Values);
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT Id AS ExerciseId, Name, Language
                                        FROM Exercise";
                        SqlDataReader reader = cmd.ExecuteReader();
                        Dictionary<int, Exercise> exercises = new Dictionary<int, Exercise>();

                        while (reader.Read())
                        {
                            int exerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                            if (!exercises.ContainsKey(exerciseId))
                            {
                                Exercise exercise = GetExercise(reader, exerciseId);
                                exercises.Add(exerciseId, exercise);
                            }
                        }
                        reader.Close();

                        return Ok(exercises.Values);
                    }

                }
            }
        }

        [HttpGet("{id}", Name = "GetExercise")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id, Name, Language
                        FROM Exercise
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Exercise exercise = null;

                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };
                    }
                    reader.Close();
                    if (exercise == null)
                    {
                        return NotFound();
                    }
                    return Ok(exercise);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM StudentExercise Where ExerciseId = @id
                                            DELETE FROM Exercise WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Exercise exercise)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Exercise
                                            SET Name = @name, Language = @language
                                            WHERE id = @id";

                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                        cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok(exercise);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!ExerciseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        private bool ExerciseExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, Language
                        FROM Exercise
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO EXERCISE (Name, Language)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));
                    int newId = (int)cmd.ExecuteScalar();
                    exercise.Id = newId;
                    return CreatedAtRoute("GetExercise", new { id = newId }, exercise);
                }
            }
        }

        private Exercise GetExercise(SqlDataReader reader, int exerciseId)
        {

            Exercise exercise = new Exercise()
            {
                Id = exerciseId,
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Language = reader.GetString(reader.GetOrdinal("Language"))
            };
            return exercise;
        }
    }
}