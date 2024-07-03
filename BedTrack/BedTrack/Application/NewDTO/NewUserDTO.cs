﻿using BedTrack.Domain.Models;

namespace BedTrack.Application.NewDTO
{
    public class NewUserDTO
    {
        public string Name { get; set; }
        public string? Status { get; set; }
        public int ClinicId { get; set; }
        public int DepartmentId { get; set; }

        public User ToModel()
        {
            return new User
            {
                Name = Name,
                Status = Status,
                ClinicId = ClinicId,
                DepartmentId = DepartmentId,
                AlarmEvents = null,
                Patients = null
            };
        }
    }
}
