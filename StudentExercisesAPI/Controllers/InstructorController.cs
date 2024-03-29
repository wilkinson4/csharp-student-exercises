﻿using System;
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
    public class InstructorController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InstructorController(IConfiguration config)
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
                        cmd.CommandText = @"SELECT Id, FirstName, LastName, SlackHandle, CohortId 
                                          FROM Instructor
                                          WHERE FirstName LIKE @searchString
                                          OR LastName LIKE @searchString
                                          OR SlackHandle LIKE @searchString";
                        cmd.Parameters.Add(new SqlParameter("@searchString", "%" + q + "%"));
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<Instructor> exercises = new List<Instructor>();

                        while (reader.Read())
                        {
                            Instructor exercise = new Instructor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                            };
                            exercises.Add(exercise);
                        }
                        reader.Close();

                        return Ok(exercises);
                    }
                    else
                    {
                        cmd.CommandText = "SELECT Id, FirstName, LastName, SlackHandle, CohortId " +
                                          "FROM Instructor";
                        SqlDataReader reader = cmd.ExecuteReader();
                        List<Instructor> exercises = new List<Instructor>();

                        while (reader.Read())
                        {
                            Instructor exercise = new Instructor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                            };
                            exercises.Add(exercise);
                        }
                        reader.Close();

                        return Ok(exercises);
                    }
                }
            }
        }

        //[HttpGet("{id}", Name = "GetExercise")]
        //public async Task<IActionResult> Get([FromRoute] int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT
        //                    Id, Name, Language
        //                FROM Exercise
        //                WHERE Id = @id";
        //            cmd.Parameters.Add(new SqlParameter("@id", id));
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            Exercise exercise = null;

        //            if (reader.Read())
        //            {
        //                exercise = new Exercise
        //                {
        //                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                    Name = reader.GetString(reader.GetOrdinal("Name")),
        //                    Language = reader.GetString(reader.GetOrdinal("Language"))
        //                };
        //            }
        //            reader.Close();
        //            if (exercise == null)
        //            {
        //                return NotFound();
        //            }
        //            return Ok(exercise);
        //        }
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete([FromRoute] int id)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"DELETE FROM StudentExercise Where ExerciseId = @id
        //                                    DELETE FROM Exercise WHERE Id = @id";
        //                cmd.Parameters.Add(new SqlParameter("@id", id));

        //                int rowsAffected = cmd.ExecuteNonQuery();
        //                if (rowsAffected > 0)
        //                {
        //                    return new StatusCodeResult(StatusCodes.Status204NoContent);
        //                }
        //                throw new Exception("No rows affected");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        if (!ExerciseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
        private bool InstructorExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName, SlackHandle, CohortId
                        FROM Instructor
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}