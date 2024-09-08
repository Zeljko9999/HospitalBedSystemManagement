import React, { useState, useEffect, ChangeEvent } from 'react';
import styled from 'styled-components';
import { Patient } from '../interfaces/Patient';
import PatientCard from './PatientCard';
import api from '../service/api';

const Header = styled.h2`
  text-align: center;
  font-size: 40px;
  margin-bottom: 50px;
  font-family: "Arsenal SC", sans-serif;
  font-weight: 700;
  font-style: normal;
`;

const FilterContainer = styled.div`
  display: flex;
  flex-direction: row;
  padding: 50px;
  margin-bottom: 10px;
  justify-content: center;
`;

const FilterRow = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
`;

const FilterLabel = styled.label`
  font-size: 18px;
  font-weight: bold;
  padding-right: 15px;
  margin: 10 px;
`;

const FilterTitle = styled.span`
  display: inline-block;
  font-family: cursive;
  color: rgb(89, 89, 191);
  font-size: 20px;
  padding-right: 20px;
`;

const ButtonContainer = styled.div`
  display: flex;
  justify-content: flex-end;
  width: 100%;
  margin-top: 15px;
`;

const AddPatientButton = styled.button`
  color: #e0bf09;
  border: 2px solid #e0bf09;
  margin-right: 15px;
  margin-bottom: 10px;
  width: 120px;
  font-weight: bold;
  transition: all 0.25s ease;
  font-size: 16px;

  &:hover{
    box-shadow: 0 10px 20px -10px rgba(228, 114, 123, 0.6);
    transform: translateY(-5px);
    background: #f4f2f2;
    border: 2px solid #e0bf09;
  }

   &:focus {
    outline: none;
    border: 2px solid #e0bf09;
  }
`;

const PatientSection = styled.div`
  display: flex;
  flex-direction: column;
  margin-top: 20px;
  padding: 10px;
  background-color: #f1f1f1;
  border: 2px solid #1959c8;
  border-radius: 4px;
`;

const Patients: React.FC = () => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [selectedFilter, setSelectedFilter] = useState<string>("all");
  const [loadingPatients, setLoadingPatients] = useState<boolean>(false);

  useEffect(() => {

    fetchPatients();
  }, [selectedFilter]);
  
  const fetchPatients = async () => {
    try {
      setLoadingPatients(true);
      const patientsResponse = await api.getFilteredPatients(selectedFilter);
      setPatients(patientsResponse);
      
    } catch (error) {
      console.error("Error fetching patients", error);
    } finally {
      setLoadingPatients(false);
    }
  };
  

  const handleFilterChange = (event: ChangeEvent<HTMLInputElement>) => {
    setSelectedFilter(event.target.value);
  };


  return (
    <>
      <Header>Pacijenti</Header>
      <FilterContainer>
        <FilterTitle>Filtriraj pacijente:</FilterTitle>
        <FilterRow>
          <FilterLabel>
            <input 
              type="radio" 
              value="all" 
              checked={selectedFilter === "all"} 
              onChange={handleFilterChange} 
            />
            Svi pacijenti
          </FilterLabel>
          <FilterLabel>
            <input 
              type="radio" 
              value="option2" 
              checked={selectedFilter === "option2"} 
              onChange={handleFilterChange} 
            />
            Klinika
          </FilterLabel>
          <FilterLabel>
            <input 
              type="radio" 
              value="option3" 
              checked={selectedFilter === "option3"} 
              onChange={handleFilterChange} 
            />
            Odjel
          </FilterLabel>
          <FilterLabel>
            <input 
              type="radio" 
              value="option4" 
              checked={selectedFilter === "option4"} 
              onChange={handleFilterChange} 
            />
            Moji pacijenti
          </FilterLabel>
        </FilterRow>
      </FilterContainer>
      <PatientSection>
        <ButtonContainer>
        <AddPatientButton>
          + Dodaj novog pacijenta
        </AddPatientButton>
        </ButtonContainer>
          {loadingPatients ? (
            <p>Loading beds...</p>
            ) : (
            <>
              {patients.length > 0 ? (
                patients.map((patient, index) => (
                  <PatientCard key={index} patient={patient} />
                ))
              ) : (
                <p>Pacijenti nisu pronađeni.</p>
              )}
            </>
          )}
      </PatientSection>
    </>
  );
};

export default Patients;