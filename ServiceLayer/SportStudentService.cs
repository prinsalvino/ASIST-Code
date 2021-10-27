using System;
using System.Collections.Generic;
using System.Linq;
using DAL.RepoInterfaces;
using Domain;
using ServiceLayer.Formula;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer
{
    public class SportStudentService: ISportStudentService
    {
        private readonly ISportStudentRepository _repository;
        
        public SportStudentService(ISportStudentRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<SportStudent> GetAllSportAdvicesByStudentId(long studentId)
        {
            try
            {
                var sportAdvices = _repository.GetAllSportAdvicesByStudentId(studentId).ToList();
                if (sportAdvices == null || !sportAdvices.Any())
                {
                    throw new Exception("No sport advices found for student");
                }
                return sportAdvices;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public SportStudent GetSportAdviceByStudentIdAndSportId(long studentId, long sportId)
        {
            try
            {
                var sportAdvice = _repository.GetSportAdviceByStudentIdAndSportId(studentId, sportId);

                if (sportAdvice == null)
                {
                    throw new Exception("No sport advice found for this student");
                }
                return sportAdvice;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddSportAdvices(Student student, int age, int tiger, int sprint, int ballHandling, int rolling, int agility)
        {
            try
            {
                List<SportStudent> sportAdvices = new List<SportStudent>();
                SportAdvicesFormula sportAdvicesFormula = new SportAdvicesFormula();
                
                
                var sportScores = sportAdvicesFormula.MQScan(student.Gender, age, tiger, sprint, ballHandling, rolling, agility);

                int count = 1;
                for (int i = 0; i < sportScores.Count; i++)
                {
                    SportStudent sportAdvice = new SportStudent();
                    var keyValuePair = sportScores.Single(x => x.Key == count);
                    sportAdvice.SportId = keyValuePair.Key;
                    sportAdvice.Score = keyValuePair.Value;
                    sportAdvice.StudentId = student.UserId;
                    sportAdvice.DateOfSportAdvices = DateTime.Now;
                    sportAdvices.Add(sportAdvice);
                    count++;
                }

                sportAdvices = sportAdvices.OrderByDescending(x => x.Score).ToList();
                
                if (sportAdvices == null || !sportAdvices.Any())
                {
                    throw new Exception("Sport advices list is empty");
                }
                else
                {
                    
                    _repository.AddMany(sportAdvices);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void RemoveSportAdvicesByStudentId(long studentId)
        {
            try
            {
                _repository.RemoveAllSportAdvicesByStudentId(studentId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}