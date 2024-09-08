import React, { useState, useEffect, ChangeEvent } from 'react';
import styled from 'styled-components';
import { useUser } from './UserContext';
import { Clinic as ClinicType } from '../interfaces/Clinic';
import { ClinicDepartment } from '../interfaces/ClinicDepartment';
import Bed from './Bed';
import api from '../service/api';

const Header = styled.h2`
  text-align: center;
  font-size: 40px;
  margin-bottom: 50px;
  font-family: "Arsenal SC", sans-serif;
  font-weight: 700;
  font-style: normal;
`;

const Filter = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: center;
`;

const BedSection = styled.div`
  margin-top: 20px;
  padding: 10px;
  background-color: #f1f1f1;
  border: 2px solid #1959c8;
  border-radius: 4px;
`;

const SelectField = styled.select`
  width: 370px;
  padding: 7px;
  margin: 15px;
  border: 2px solid black;
  font-size: 16px;
`;

//Information lines above beds
const InfoSection = styled.div`
  display: flex;
  flex-direction: row;
  margin-bottom: 10px;
  margin-top: 10px;
  font-weight: bold;
  font-size: 25px;
  justify-content: center;
  font-family: sans-serif;
`;

const InfoLine = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  margin-bottom: 10px;
  margin-right: 20px; 
`;

const Label = styled.span`
`;

const Value = styled.span`
  margin-left: 10px;
  color: #888;
`;

const Beds = styled.div`
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
`;

interface FormData {
  clinicId: number;
  departmentId: number;
}

const Clinic: React.FC = () => {
  const { user } = useUser();
  const [clinics, setClinics] = useState<ClinicType[]>([]);
  const [departments, setDepartments] = useState<ClinicDepartment[]>([]);
  const [formData, setFormData] = useState<FormData>({ clinicId: 0, departmentId: 0 });
  const [beds, setBeds] = useState<any[]>([]);
  const [loadingBeds, setLoadingBeds] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (user) {
      if (user.role === 'Admin' || user.role === 'Boss') {
        fetchAllClinicsAndDepartments();
      } else if (user.role === 'Clerk') {
        fetchDepartmentsForClinic(user.clinicId);
        setFormData({ clinicId: user.clinicId, departmentId: 0 });
      } else if (user.role === 'Doctor' || user.role === 'Nurse') {
        setFormData({ clinicId: user.clinicId, departmentId: user.departmentId });
      }
    }
  }, [user]);

  useEffect(() => {
    if (formData.clinicId !== 0) {
      fetchDepartmentsForClinic(formData.clinicId);
    }
  }, [formData.clinicId]);

  useEffect(() => {
    if (formData.clinicId !== 0 && formData.departmentId !== 0) {
      fetchBedsData(formData.clinicId, formData.departmentId);
    }
  }, [formData]);

  const fetchAllClinicsAndDepartments = async () => {
    try {
      const clinicsResponse = await api.getAllClinicsAndDepartments();
      setClinics(clinicsResponse);
    } catch (error) {
      console.error('Error fetching clinics:', error);
      setError('Error fetching clinics');
    }
  };

  const fetchDepartmentsForClinic = async (clinicId: number) => {
    try {
      const departmentsResponse = await api.getAllClinicDepartments(clinicId);
      setDepartments(departmentsResponse);
    } catch (error) {
      console.error('Error fetching departments:', error);
      setError('Error fetching departments');
    }
  };

  const fetchBedsData = async (clinicId: number, departmentId: number) => {
    setLoadingBeds(true);
    try {
      const bedsResponse = await api.getAllBedsForClinicDep(clinicId, departmentId);
      setBeds(bedsResponse);
      setLoadingBeds(false);
    } catch (error) {
      console.error('Error fetching beds:', error);
      setError('Error fetching beds');
      setLoadingBeds(false);
    }
  };

  const handleInputChange = (event: ChangeEvent<HTMLSelectElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: Number(value),
    });
    if (name === 'clinicId') {
      fetchDepartmentsForClinic(Number(value));
    }
  };

  return (
    <>
      <Header>Pregled kreveta prema klinikama i odjelima</Header>
      <Filter>
        <h2>Klinika:</h2>
        <SelectField
          name="clinicId"
          value={formData.clinicId}
          onChange={handleInputChange}
          required
          disabled={!!(user && (user.role === 'Clerk' || user.role === 'Doctor' || user.role === 'Nurse'))}
        >
          <option value=''>Klinika</option>
          {clinics.map(clinic => (
            <option key={clinic.id} value={clinic.id}>
              {clinic.name}
            </option>
          ))}
        </SelectField>
        <h2 style={ {marginLeft: '10px'}}>Odjel:</h2>
        <SelectField
          name="departmentId"
          value={formData.departmentId}
          onChange={handleInputChange}
          required
          disabled={!!(user && (user.role === 'Doctor' || user.role === 'Nurse'))}
        >
          <option value=''>Odjel</option>
          {departments.map(department => (
            <option key={department.departmentId} value={department.departmentId}>
              {department.department}
            </option>
          ))}
        </SelectField>
      </Filter>
      {error && <p>{error}</p>}
      {formData.clinicId !== 0 && formData.departmentId !== 0 && (
        <BedSection>
          <InfoSection>
          <InfoLine>
            <Label>Klinika: </Label>
            <Value>{departments.find(cli => cli.clinicId === formData.clinicId)?.clinic}</Value>
          </InfoLine>
          <InfoLine>
            <Label>Odjel: </Label>
            <Value>{departments.find(dep => dep.departmentId === formData.departmentId)?.department}</Value>
          </InfoLine>
          </InfoSection>
          <Beds>
          {loadingBeds ? (
            <p>Loading beds...</p>
          ) : (
            <>
              {beds.length > 0 ? (
                beds.map((bed, index) => (
                  <Bed key={index} bed={bed} />
                ))
              ) : (
                <p>Kreveti nisu pronađeni.</p>
              )}
            </>
          )}
          </Beds>
        </BedSection>
      )}
    </>
  );
};

export default Clinic;