using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises.Models
{
    public class Student
    {
        public Student(string first, string last, string slack, Cohort cohort)
        {
            FirstName = first;
            LastName = last;
            SlackHandle = slack;
            Exercises = new List<Exercise>();
            StudentsCohort = cohort;
        }
        public string FirstName { get; }
        public string LastName { get; }
        public string SlackHandle { get; }

        public Cohort StudentsCohort { get; }

        public List<Exercise> Exercises { get; }
    }
}
