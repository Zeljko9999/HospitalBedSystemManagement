import React from 'react';
import styled from 'styled-components';
import { Patient } from '../interfaces/Patient';
import { Link } from 'react-router-dom';

const CardContainer = styled.div`
  margin-top: 40px;
  padding: 10px;
  background-color: white;
  border-radius: 4px;
  text-align:center;
  border: 3px solid #c8c4c4;
`;

const Name = styled.h3`
  color: black;
  max-width: 250px;
  margin: 0 auto;
  font-size: 22px;
  font-family: math;
`;

const PatientImage = styled.img`
    height: 200px;
    width: 200px;
    margin-left: 20px;
    border: 1px solid gray;
    border-radius: 4px;
`;

const PatientInfo = styled.div`
  display: flex;
  flex-direction: row;
  text-align: center;
  margin: 10px;
  font-size: 18px;
  font-family: sans-serif;
`;

const FirstInfo = styled.div`
  text-align: center;
  max-width: 250px;
  margin: 10px auto;
  font-size: 18px;
  font-family: sans-serif;
`;

const BedValue = styled.p`
  max-width: 200px;
  margin: 10px auto;
`;

const TopContainer = styled.div`
    display: flex;
    align-items: center;
`;

const EditLink = styled.a`
  &&{
    font-size: 18px;
  }
`;

interface PatientProps {
  patient: Patient;
}

const PatientCard: React.FC<PatientProps> = ({ patient }) => {
  return (
    <CardContainer>
      <TopContainer>
      <Name>{patient.name}</Name>
      <Link to={`/patients/edit/${patient.id}`}> <EditLink>Uredi</EditLink></Link>
      </TopContainer>

      <PatientInfo>
        <PatientImage src="src/images/avatar.jpg" alt="Bed image" />
        <FirstInfo>
          <BedValue>{patient.age}</BedValue>
          <BedValue>{patient.nationality}</BedValue>
          <BedValue>{patient.insurance}</BedValue>
        </FirstInfo>
      </PatientInfo>
    </CardContainer>
  );
};

export default PatientCard;