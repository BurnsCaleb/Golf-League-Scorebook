namespace Core.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseLocation { get; set; }
        public double CourseRating { get; set; }
        public double CourseSlope { get; set; }
        public int NumHoles { get; set; }
        public int CoursePar { get; set; }

        public virtual ICollection<Hole> Holes { get; set; } = new List<Hole>();
        public virtual ICollection<League> Leagues { get; set; } = new List<League>();
        public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();

        public override string ToString()
        {
            return CourseName;
        }
    }
}
