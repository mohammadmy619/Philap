﻿using Domain.Persons.Leader.Exception;


namespace Domain.Persons.Leader
{
    public class Leader : Person
    {
        #region Properties  

        public string Title { get; private set; }
        public string Department { get; private set; }
        public DateTime JoiningDate { get; private set; }
        public List<string> Skills { get; private set; }
        public string Bio { get; private set; }

        #endregion

        #region Constructor  

        public Leader(List<Guid> tripIds, string name, string lastName, string email, string phoneNumber,
            DateTime dateOfBirth, Gender gender, Address address, string nationality,
            string title, string department, DateTime joiningDate, List<string> skills, string bio)
            : base(tripIds, name, lastName, email, phoneNumber, dateOfBirth, gender, address, nationality)
        {
            GuardAgainstTitle(title);
            GuardAgainstDepartment(department);
            GuardAgainstJoiningDate(joiningDate);
            GuardAgainstSkills(skills);

            Title = title;
            Department = department;
            JoiningDate = joiningDate;
            Skills = skills;
            Bio = bio;
        }

        #endregion
        #region Method
        public void Update(
         List<Guid> tripIds,
         string Name,
         string LastName,
         string Email,
         string PhoneNumber,
         DateTime DateOfBirth,
         Gender Gender,
         Address Address,
         string Nationality,
         string Title,
         string Department,
         DateTime newJoiningDate,
         List<string> newSkills,
         string newBio)
        {

            UpdatePerson(tripIds,Name,LastName,Email,PhoneNumber,DateOfBirth, Gender, Address,Nationality);
         

            // Validate and update Leader-specific properties
            GuardAgainstTitle(Title);
            GuardAgainstDepartment(Department);
            GuardAgainstJoiningDate(newJoiningDate);
            GuardAgainstSkills(newSkills);

            this.Title = Title;
            Department = Department;
            JoiningDate = newJoiningDate;
            Skills = newSkills;
            Bio = newBio;
        }
        #endregion
        #region Guard Methods  

        private void GuardAgainstTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new TitleIsNullException();
            }
        }

        private void GuardAgainstDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
            {
                throw new DepartmentIsNullException();
            }
        }

        private void GuardAgainstJoiningDate(DateTime joiningDate)
        {
            if (joiningDate >= DateTime.Now)
            {
                throw new JoiningDateIsInvalidException();
            }
        }

        private void GuardAgainstSkills(List<string> skills)
        {
            if (skills == null || skills.Count == 0)
            {
                throw new SkillsAreEmptyException();
            }
        }

        #endregion
    }
}
