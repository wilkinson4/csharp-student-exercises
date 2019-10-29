using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises.Models
{
    public class Cohort
    {
        public Cohort(string name)
        {
            Name = name;
            Students = new List<Student>();
            Instructors = new List<Instructor>();
        }
        public string Name { get; }

        public List<Student> Students { get; }
        public List<Instructor> Instructors { get; }
    }
}
