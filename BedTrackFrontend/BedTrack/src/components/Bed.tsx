import React from 'react';
import styled from 'styled-components';
import { BedCard } from '../interfaces/BedCard';
import { Link } from 'react-router-dom';

const CardContainer = styled.div`
  display: inline-block;
  margin: 40px;
  margin-left: 60px;
  padding: 10px;
  background-color: #f9f9f9;
  border-radius: 4px;
  text-align: center;
  max-width: 300px;
  height: 360px;
  border: 2px solid #c8c4c4;
`;

const BedTitle = styled.h3`
  color: #007bff;
  max-width: 250px;
  margin: 0 auto;
  font-size: 22px;
  font-family: math;
  margin-left: 95px;
`;

const BedImage = styled.img`
    height: 220px;
    width: 280px;
`;

const BedInfo = styled.div`
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

const AvailabilityText = styled.p<{ isAvailable: boolean }>`
  color: ${(props) => (props.isAvailable ? 'green' : 'red')};
  max-width: 200px;
  margin: 10px auto;
  font-weight: bold;
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

interface BedProps {
  bed: BedCard;
}

const Bed: React.FC<BedProps> = ({ bed }) => {
  return (
    <CardContainer>
      <TopContainer>
      <BedTitle>{bed.bed}</BedTitle>
      <Link to={`/beds/edit/${bed.id}`}> <EditLink>Uredi</EditLink></Link>
      </TopContainer>
      {bed.isAvailable ? <BedImage src="src/images/bedIconEmpty.png" alt="Bed image" /> :  <BedImage src="src/images/bedIconFull.png" alt="Bed image" /> }
      <BedInfo>
        <AvailabilityText isAvailable={bed.isAvailable}>{bed.isAvailable ? 'Slobodan' : 'Zauzet'}</AvailabilityText>
        {bed.patient && <BedValue>Pacijent: {bed.patient ? bed.patient : ' '}</BedValue>}
        <BedValue>{bed.status}</BedValue>
      </BedInfo>
    </CardContainer>
  );
};

export default Bed;