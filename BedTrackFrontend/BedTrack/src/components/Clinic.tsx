import React, { useState, useEffect, ChangeEvent } from 'react';
import styled from 'styled-components';
import axios from 'axios';
import { useUser } from './UserContext';
import { Clinic as ClinicType } from '../interfaces/Clinic';
import { ClinicDepartment } from '../interfaces/ClinicDepartment';
import Bed from './Bed';

const Container = styled.div`
  padding: 20px;
  background-color: #67b6df;
`;

const Header = styled.h2`
  color: red;
  text-align: center;
  font-size: 35px;
  margin-bottom: 50px;
`;

const Filter = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: center;
`;

const NewElement = styled.div`
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
  font-weight: bold;
  font-size: 25px;
  justify-content: center;
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
      const clinicsResponse = await axios.get<ClinicType[]>('https://localhost:5262/api/Clinic', { withCredentials: true });
      setClinics(clinicsResponse.data);
    } catch (error) {
      console.error('Error fetching clinics:', error);
      setError('Error fetching clinics');
    }
  };

  const fetchDepartmentsForClinic = async (clinicId: number) => {
    try {
      const response = await axios.get<ClinicDepartment[]>(`https://localhost:5262/ClinicDepartments/${clinicId}`, { withCredentials: true });
      setDepartments(response.data);
    } catch (error) {
      console.error('Error fetching departments:', error);
      setError('Error fetching departments');
    }
  };

  const fetchBedsData = async (clinicId: number, departmentId: number) => {
    setLoadingBeds(true);
    try {
      const response = await axios.get<any[]>(`https://localhost:5262/AllBeds/${clinicId}&${departmentId}`, { withCredentials: true });
      setBeds(response.data);
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
    <Container>
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
        <h2>Odjel:</h2>
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
        <NewElement>
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
            <div>
              {beds.length > 0 ? (
                beds.map((bed, index) => (
                  <Bed key={index} bed={bed} />
                ))
              ) : (
                <p>Kreveti nisu pronađeni.</p>
              )}
            </div>
          )}
          </Beds>
        </NewElement>
      )}
    </Container>
  );
};

export default Clinic;