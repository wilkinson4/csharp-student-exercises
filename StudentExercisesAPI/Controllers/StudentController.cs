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
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentController(IConfiguration config)
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
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    if (include == "exercise")
                    {
                        cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, se.ExerciseId, c.Name AS CohortName, e.Name AS ExerciseName, e.Language 
                                        FROM Student s 
                                        INNER JOIN Cohort c 
                                        ON s.CohortId = c.Id
                                        LEFT JOIN StudentExercise se ON se.StudentId = s.Id
                                        LEFT JOIN Exercise e ON se.ExerciseId = e.Id;";

                        SqlDataReader reader = cmd.ExecuteReader();


                        //Get All Students and store them in a dictionary
                        Dictionary<int, Student> students = new Dictionary<int, Student>();
                        while (reader.Read())
                        {
                            int studentId = reader.GetInt32(reader.GetOrdinal("Id"));
                            if (!students.ContainsKey(studentId))
                            {
                                students.Add(studentId, GetStudent(reader, studentId));
                            }
                            Student fromDictionary = students[studentId];
                            if (!reader.IsDBNull(reader.GetOrdinal("ExerciseId")))
                            {
                                Exercise exercise = new Exercise()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                    Name = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                    Language = reader.GetString(reader.GetOrdinal("Language"))
                                };
                                fromDictionary.Exercises.Add(exercise);
                            }
                        }
                        reader.Close();

                        return Ok(students.Values);
                    }
                    else if (q != null)
                    {
                        cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name AS CohortName 
                                        FROM Student s 
                                        INNER JOIN Cohort c 
                                        ON s.CohortId = c.Id
                                        WHERE s.FirstName LIKE @searchString
                                        OR s.LastName LIKE @searchString
                                        OR s.SlackHandle LIKE @searchString";

                        cmd.Parameters.Add(new SqlParameter("@searchString", "%" + q + "%"));

                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Student> students = new Dictionary<int, Student>();
                        while (reader.Read())
                        {
                            int studentId = reader.GetInt32(reader.GetOrdinal("Id"));
                            if (!students.ContainsKey(studentId))
                            {
                                students.Add(studentId, GetStudent(reader, studentId));
                            }
                        }

                        reader.Close();

                        return Ok(students.Values);
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name AS CohortName 
                                        FROM Student s 
                                        INNER JOIN Cohort c 
                                        ON s.CohortId = c.Id";

                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Student> students = new Dictionary<int, Student>();
                        while (reader.Read())
                        {
                            int studentId = reader.GetInt32(reader.GetOrdinal("Id"));
                            if (!students.ContainsKey(studentId))
                            {
                                students.Add(studentId, GetStudent(reader, studentId));
                            }
                        }

                        reader.Close();

                        return Ok(students.Values);
                    }
                }
            }
        }
        private Student GetStudent(SqlDataReader reader, int studentId)
        {

            Student student = new Student()
            {
                Id = studentId,
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                Cohort = new Cohort()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                    Name = reader.GetString(reader.GetOrdinal("CohortName"))
                },

            };
            return student;
        }
    }
}