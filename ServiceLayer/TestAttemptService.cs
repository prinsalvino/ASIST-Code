using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.RepoInterfaces;
using Domain;
using ServiceLayer.Formula;
using ServiceLayer.ServiceInterfaces;

namespace ServiceLayer
{
    public class TestAttemptService: ITestAttemptService
    {
        private readonly ITestAttemptRepository _testAttemptRepository;

        public TestAttemptService(ITestAttemptRepository testAttemptRepository)
        {
            _testAttemptRepository = testAttemptRepository;
        }

        public IEnumerable<TestAttempt> GetAllTestsAttemptedByStudentId(long studentId)
        {
            try
            {
                var testsAttempted = _testAttemptRepository.GetAllTestsAttemptedByStudentId(studentId).ToList();
                if (testsAttempted == null || !testsAttempted.Any())
                {
                    throw new Exception("No tests attempted by student");
                }
                return testsAttempted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddTestAttempt(Student student, int age, int tiger, int sprint, int ballHandling, int rolling, int agility)
        {
            try
            {
                TestAttempt testAttempt = new TestAttempt();
                FinalScoreFormula finalScoreFormula = new FinalScoreFormula();
                int finalScore = finalScoreFormula.FinalScore(student.Gender, age, tiger, sprint, ballHandling, rolling, agility);

                testAttempt.StudentId = student.UserId;
                testAttempt.DateOfTest = DateTime.Now;
                testAttempt.FinalScore = finalScore;

                _testAttemptRepository.Add(testAttempt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RemoveAllTestsAttemptedByStudentId(long studentId)
        {
            try
            {
                _testAttemptRepository.RemoveAllTestAttemptsByStudentId(studentId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}