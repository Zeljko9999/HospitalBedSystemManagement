﻿using BedTrack.Application.DTO;
using BedTrack.Domain.Models;

namespace BedTrack.DAL.Interfaces
{
    public interface IClinicDepartmentBedRepository
    {
        Task AddClinicDepartmentBed(ClinicDepartmentBed? clinicDepartmentBed);

        public Task<ClinicDepartmentBed> GetClinicDepartmentBed(int id);

        public Task<ClinicDepartmentBed> GetClinicDepartmentForBed(int clinicDepartmentId, int bedId);

        public Task<ClinicDepartmentBed> GetClinicDepartmentForPatient(int patientId);

        Task UpdateClinicDepartmentBed(ClinicDepartmentBed? updatedClinicDepartmentBed);

        Task DeleteClinicDepartmentBed(int id);
    }
}