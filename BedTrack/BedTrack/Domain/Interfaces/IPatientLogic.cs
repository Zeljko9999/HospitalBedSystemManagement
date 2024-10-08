﻿using BedTrack.Application.NewDTO;
using BedTrack.Application.DTO;
using BedTrack.Domain.Models;

namespace BedTrack.Domain.Interfaces
{
    public interface IPatientLogic
    {
        Task CreateNewPatient(NewPatientDTO? patient);
        Task UpdatePatient(int id, NewPatientDTO? updatedPatient);
        Task DeletePatient(int id);
        Task<IEnumerable<PatientDTO>> GetAllPatients();
        public Task<IEnumerable<PatientDTO>> GetPatientsOnClinicDepartment(int clinicId, int departmentId);
        Task<PatientDTO> GetPatient(int id);
        Task<IEnumerable<PatientDTO>> GetPatientsWithoutBed();
    }
}
