import { useState, ChangeEvent, FormEvent } from "react";
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import api from "../service/api";
import { Patient } from "../interfaces/Patient";

// Styled components
const FormContainer = styled.form`
  display: flex;
  flex-direction: row;
  max-width: 600px;
  margin: 50px 200px;
  justify-content: space-between;
  background-color: white;
  padding: 20px;
  border: 2px solid #1959c8;
  border-radius: 8px;
  background-color: white;
`;

const FormSection = styled.div`
  display: flex;
  flex-direction: column;
  margin-right: 100px;
`;

const InputField = styled.input`
  width: 350px;
  padding: 7px;
  margin: 15px;
  border: 2px solid black;
  font-size: 16px;
`;

const SubmitButton = styled.button`
  margin-top: 10px;
  cursor: pointer;
  font-weight: 500;
  border-radius: 12px;
  font-size: 1rem;
  border: none;
  color: #fff;
  background: #ff3e4e;
  transition: all 0.25s ease;
  width: 100px;
  margin: 20px;
  height: 40px;

  &:hover {
    box-shadow: 0 10px 20px -10px rgba(255, 62, 78, 0.6);
    transform: translateY(-5px);
    background: #ff3e4e;
  }
`;

const SuccessMessage = styled.div`
  color: rgb(20, 190, 20);
`;

const BackButton = styled(Link)`
  display: inline-block;
  margin: 20px;
  font-size: 15px;

  button {
    margin-top: 10px;
    cursor: pointer;
    font-weight: 500;
    border-radius: 12px;
    font-size: 0.9rem;
    border: none;
    color: #fff;
    background: #ff3e4e;
    transition: all 0.25s ease;
    width: 100px;
    margin: 20px;
    height: 35px;

    &:hover {
      box-shadow: 0 10px 20px -10px rgba(255, 62, 78, 0.6);
      transform: translateY(-5px);
      background: #ff3e4e;
    }
  }
`;

const EditLabel = styled.label`
  margin-left: 20px;
  margin-top: 10px;
  font-weight: bold;
  font-size: 19px;
  font-family: sans-serif;
`;

const NewPatient: React.FC = () => {
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);

  // Initialize form data with appropriate patient fields
  const [formData, setFormData] = useState<Patient>({
    id: -1,  // This should be handled by the backend when creating a new patient
    name: "",
    age: 0,
    nationality: "",
    insurance: "",
    healthRecord: "",
    healthHistory: "",
  });

  // Handle input changes
  const handleInputChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: name === 'age' ? Number(value) : value,  // Convert 'age' to a number
    });
  };

  // Handle form submission
  const handleFormSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await api.createPatient(formData); 
      setShowSuccessMessage(true);
      setTimeout(() => {
        setShowSuccessMessage(false);
      }, 3000);
    } catch (error) {
      console.error('Error while creating patient:', error);
    }
  };

  return (
    <>
      <BackButton to="/patients">
        <button>Natrag</button>
      </BackButton>
      <FormContainer onSubmit={handleFormSubmit}>
        <FormSection>
          <EditLabel>Ime i prezime*:</EditLabel>
          <InputField
            name="name"
            type="text"
            placeholder="Ime i prezime"
            value={formData.name}
            onChange={handleInputChange}
            required
          />
          <EditLabel>Nacionalnost:</EditLabel>
          <InputField
            name="nationality"
            type="text"
            placeholder="Nacionalnost"
            value={formData.nationality}
            onChange={handleInputChange}
            required
          />
          <EditLabel>Starost:</EditLabel>
          <InputField
            name="age"
            type="number"
            placeholder="Starost"
            value={formData.age}
            onChange={handleInputChange}
          />
          <EditLabel>Osiguranje:</EditLabel>
          <InputField
            name="insurance"
            type="text"
            placeholder="Osiguranje"
            value={formData.insurance}
            onChange={handleInputChange}
          />
          <EditLabel>Opis zdravstvenog stanja:</EditLabel>
          <InputField
            name="healthRecord"
            type="text"
            placeholder="Zdravstveno stanje"
            value={formData.healthRecord}
            onChange={handleInputChange}
          />
          <EditLabel>Povijest bolesti:</EditLabel>
          <InputField
            name="healthHistory"
            type="text"
            placeholder="Povijest bolesti"
            value={formData.healthHistory}
            onChange={handleInputChange}
          />
          <SubmitButton type="submit">Spremi</SubmitButton>
          {showSuccessMessage && (
            <SuccessMessage>
              Pacijent je uspješno kreiran!
            </SuccessMessage>
          )}
        </FormSection>
      </FormContainer>
    </>
  );
};

export default NewPatient;
