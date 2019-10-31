﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle { get; set; }

        public int CohortId { get; set; }
        public Cohort Cohort { get; set; } = new Cohort();
        public void AssignExercise(Student student, Exercise exercise)
        {
            student.Exercises.Add(exercise);
            exercise.Students.Add(student);
        }
    }
}
