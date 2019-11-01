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
    public class CohortController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortController(IConfiguration config)
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
        public async Task<IActionResult> Get(string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (q != null)
                    {
                        cmd.CommandText = @"SELECT Id, Name FROM Cohort
                                            WHERE Name LIKE @searchString";
                        cmd.Parameters.Add(new SqlParameter("@searchString", "%" + q + "%"));
                        SqlDataReader reader = cmd.ExecuteReader();

                        List<Cohort> cohorts = new List<Cohort>();
                        while (reader.Read())
                        {
                            Cohort cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),

                            };
                            cohorts.Add(cohort);
                        }
                        reader.Close();

                        return Ok(cohorts);
                    }
                    else
                    {
                        cmd.CommandText = @"SELECT s.Id AS TheStudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack, 
                                        c.Id AS CohortId, c.Name AS CohortName, 
                                        i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack,
                                        se.ExerciseId, se.StudentId,
                                        e.Name AS ExerciseName, e.Language
                                        FROM Cohort c 
                                        INNER JOIN Student s 
                                        ON s.CohortId = c.Id
                                        INNER Join Instructor i
                                        ON i.CohortId = c.Id
                                        LEFT JOIN StudentExercise se ON se.StudentId = s.Id
                                        LEFT JOIN Exercise e ON se.ExerciseId = e.Id;";
                        SqlDataReader reader = cmd.ExecuteReader();

                        Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();

                        while (reader.Read())
                        {
                            int cohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                            if (!cohorts.ContainsKey(cohortId))
                            {
                                Cohort cohort = new Cohort()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    Name = reader.GetString(reader.GetOrdinal("CohortName")),

                                };
                                cohorts.Add(cohortId, cohort);
                            }

                            Cohort fromDictionary = cohorts[cohortId];
                            if (!reader.IsDBNull(reader.GetOrdinal("TheStudentId")))
                            {
                                if (!fromDictionary.Students.Any(s => s.Id == reader.GetInt32(reader.GetOrdinal("TheStudentId"))))
                                {
                                    Student student = new Student()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("TheStudentId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("StudentFirst")),
                                        LastName = reader.GetString(reader.GetOrdinal("StudentLast")),
                                        SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack"))
                                    };
                                    fromDictionary.Students.Add(student);
                                }

                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("InstructorId")))
                            {
                                if (!fromDictionary.Instructors.Any(i => i.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
                                {
                                    Instructor instructor = new Instructor()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("InstructorFirst")),
                                        LastName = reader.GetString(reader.GetOrdinal("InstructorLast")),
                                        SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack"))
                                    };
                                    fromDictionary.Instructors.Add(instructor);
                                }

                            }

                        }
                        reader.Close();

                        return Ok(cohorts.Values);
                    }
                }
            }
        }
    }
}