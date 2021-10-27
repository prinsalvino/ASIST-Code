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
    public class SkillStudentService: ISkillStudentService
    {
        private readonly ISkillStudentRepository _skillRepository;
        
        public SkillStudentService(ISkillStudentRepository repository)
        {
            _skillRepository = repository;
        }

        public IEnumerable<SkillStudent> GetAll(long studentId)
        {
            try
            {
                var skillStudents = _skillRepository.GetAll().Where(e => e.StudentId == studentId).ToList();
                CheckIfSkillStudentListIsEmpty(skillStudents);
                return skillStudents;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddSkillLapTimes(List<SkillStudent> skillStudents, Gender gender, int age, int tiger, int sprint, int ballHandling, int rolling, int agility)
        {
            try
            {
                SkillCalculation skillCalculation = new SkillCalculation();
                var skillScores = skillCalculation.SkillsCalculation(gender, age, tiger, sprint, ballHandling, rolling, agility);

                for (int i = 0; i < skillScores.Length; i++)
                {
                    skillStudents[i].Score = Convert.ToInt32(skillScores[i]);
                }

                skillStudents = skillStudents.OrderByDescending(x => x.Score).ToList();
                _skillRepository.AddMany(skillStudents);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool CheckIfSkillStudentListIsEmpty(IEnumerable<SkillStudent> skillStudents)
        {
            try
            {
                if (skillStudents == null || !skillStudents.Any())
                {
                    throw new Exception("No skills for a student found");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void RemoveAllSkillsPerformedByStudentId(long studentId)
        {
            try
            {
                _skillRepository.RemoveAllSkillsPerformedByStudentId(studentId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}