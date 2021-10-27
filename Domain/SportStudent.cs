using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Domain
{
    public class SportStudent
    {
        [OpenApiProperty(Description = "gets or sets the student and sport id")]
        public long SportStudentId { get; set; }
        
        public long StudentId { get; set; }
        [ForeignKey("StudentId")]
        [OpenApiProperty(Description = "gets or sets the student")]
        public virtual Student Student { get; set; }
        
        public long? SportId { get; set; }
        [ForeignKey("SportId")]
        [OpenApiProperty(Description = "gets or sets the sport")]
        public virtual Sport Sport { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the date for the sport advices")]
        [Required]
        public DateTime DateOfSportAdvices { get; set; }
        
        [OpenApiProperty(Description = "gets or sets the score of skill")]
        public int Score { get; set; }
        public SportStudent()
        {

        }
        public SportStudent( long studentId, Student student, long? sportId, Sport sport, DateTime dateOfSportAdvices, int score)
        {
            StudentId = studentId;
            Student = student;
            SportId = sportId;
            Sport = sport;
            DateOfSportAdvices = dateOfSportAdvices;
            Score = score;
        }
    }
}