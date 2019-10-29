using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises.Models
{
    public class Instructor
    {
        public Instructor(string first, string last, Cohort cohort)
        {
            FirstName = first;
            LastName = last;
            InstructorsCohort = cohort;

        }
        public string FirstName { get; }
        public string LastName { get; }
        public string SlackHandle { get; }

        public Cohort InstructorsCohort { get; set; }
        public string Specialty { get; set; }

        public void AssignExercise(Student student, Exercise exercise)
        {
            student.Exercises.Add(exercise);
            exercise.Students.Add(student);
        }
    }
}
