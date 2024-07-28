import axios from "axios";
import { useState, useEffect, ChangeEvent, FormEvent } from "react";
import { useParams, Link } from 'react-router-dom';
import styled from 'styled-components';
import { useUser } from './UserContext';
import { Clinic } from '../interfaces/Clinic';
import { BedCard } from '../interfaces/BedCard';

const FormContainer = styled.form`
  display: flex;
  flex-direction: row;
  max-width: 600px;
  margin: 50px 200px;
  justify-content: space-between;
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

const SelectField = styled.select`
  width: 370px;
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
  font-size: 19px;
  font-family: sans-serif;
`;

const BackButton = styled(Link)`
  display: inline-block;
  margin: 20px;

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

const AddPatientButton = styled.button`
  color: #e0bf09;
  border: 2px solid #e0bf09;
  margin-left: 20px;
  margin-bottom: 10px;
  margin-top: 10px;
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

const EditLabel = styled.label`
  margin-left: 20px;
  margin-top: 10px;
  font-weight: bold;
  font-size: 19px;
  font-family: sans-serif;
`;

const Patient = styled.div`
  margin-top: 20px;
  margin-bottom: 20px;
  font-size: 19px;
  font-family: sans-serif;
`;

const EditBed: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { user } = useUser();

  const [patients, setPatients] = useState<Clinic[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const [showPatientSelect, setShowPatientSelect] = useState(false);
  const [isPatientAdded, setIsPatientAdded] = useState(false);
  const [message, setMessage] = useState("");

  const [formData, setFormData] = useState<BedCard>({
    id: -1,
    isAvailable: true,
    status: "",
    clinic: "",
    department: "",
    clinicDepartmentId: -1,
    bed: "",
    bedId: -1,
    patient: "",
    patientId: null
  });

  useEffect(() => {
    const fetchBedData = async () => {
      try {
        const response = await axios.get(`https://localhost:5262/api/ClinicDepartmentBed/${id}`, { withCredentials: true });
        const fetchedData = response.data;
        fetchedData.isAvailable = Boolean(fetchedData.isAvailable);
        setFormData(fetchedData);
        setIsPatientAdded(fetchedData.patientId !== null);
      } catch (error) {
        console.error('Error fetching bed data:', error);
      }
    };
    fetchBedData();
  }, [id, isPatientAdded]);

  useEffect(() => {
    axios.get("https://localhost:5262/api/Patient/without-beds", { withCredentials: true })
      .then(res => {
        setPatients(res.data);
      })
      .catch(err => console.log(err.message));
  }, [isPatientAdded]);


  const handleInputChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: name === 'isAvailable' ? (value === 'true') : value
    });
  };

  const handleFormSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.patch(
        `https://localhost:5262/api/ClinicDepartmentBed/edit/${id}`,
        formData.patientId !== null
          ? { ...formData, isAvailable: false }
          : { ...formData },
        { withCredentials: true }
      );      
      setMessage("Krevet je uspješno uređen!");
      setShowSuccessMessage(true);
      setIsPatientAdded(true);
      setTimeout(() => {
        setShowSuccessMessage(false);
      }, 3000);
    } catch (error) {
      console.error('Error while updating data:', error);
    }
  };

  const handleAddPatientClick = () => {
    setShowPatientSelect(!showPatientSelect);
  };

  const handleDeletePatientClick = async () => {
    try {
      await axios.patch(`https://localhost:5262/api/ClinicDepartmentBed/edit/${id}`, {
        ...formData,
        isAvailable: true,
        patientId: null
      }, { withCredentials: true });
      setFormData({
        ...formData,
        isAvailable: true,
        patient: "",
        patientId: null
      });
      setMessage("Pacijent je uspješno otpušten!");
      setIsPatientAdded(false);
      setShowSuccessMessage(true);
      setTimeout(() => {
        setShowSuccessMessage(false);
      }, 3000);
    } catch (error) {
      console.error('Error while updating data:', error);
    }
  };

  return (
    <>
      <BackButton to="/clinic">
        <button>Natrag</button>
      </BackButton>
      <FormContainer onSubmit={handleFormSubmit}>
        <FormSection>
          <EditLabel>Dostupnost kreveta:</EditLabel>
          <SelectField
            name="isAvailable"
            value={String(formData.isAvailable)}
            onChange={handleInputChange}
            required
            disabled
          >
            <option value="">Odaberi dostupnost</option>
            <option value="true">Dostupan</option>
            <option value="false">Nedostupan</option>
          </SelectField>
          <EditLabel>Status kreveta:</EditLabel>
          <InputField
            name="status"
            placeholder="Status"
            value={formData.status}
            onChange={handleInputChange}
          />
          <Patient>
          <EditLabel>Pacijent: </EditLabel><label>{formData.patient}</label>
          </Patient>
          {user && user.role === 'Clerk' && !isPatientAdded && (
            <>     
              <AddPatientButton type="button" onClick={handleAddPatientClick}>
                + Dodaj pacijenta
              </AddPatientButton>
              {showPatientSelect && (
                <>
                <SelectField
                  name="patientId"
                  value={formData.patientId || ""}
                  onChange={handleInputChange}
                >
                  <option value="">Odaberi pacijenta</option>
                  {patients.map(patient => (
                    <option key={patient.id} value={patient.id}>
                      {patient.name}
                    </option>
                  ))}
                </SelectField>
                </>
              )}
            </>
          )}

          {user && user.role === 'Clerk' && isPatientAdded && (
            <>     
              <AddPatientButton type="button" onClick={handleDeletePatientClick}>
                - Otpusti pacijenta
              </AddPatientButton>
            </>      
              )}

          <SubmitButton type="submit">Spremi</SubmitButton>
          {showSuccessMessage && (
            <SuccessMessage>
             {message}
            </SuccessMessage>
          )}
        </FormSection>
      </FormContainer>
    </>
  );
};

export default EditBed;