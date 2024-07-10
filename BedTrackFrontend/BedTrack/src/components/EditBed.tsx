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
  font-size: 0.8rem;
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
`;

const SuccessMessage = styled.div`
  color: rgb(20, 190, 20);
`;

const BackButton = styled(Link)`
  display: inline-block;
  margin: 20px;

  button {
    margin-top: 10px;
    cursor: pointer;
    font-weight: 500;
    border-radius: 12px;
    font-size: 0.8rem;
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
  width: 110px;
  font-weight: bold;
  transition: all 0.25s ease;

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

const EditBed: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { user } = useUser();

  const [patients, setPatients] = useState<Clinic[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const [showPatientSelect, setShowPatientSelect] = useState(false);

  const [formData, setFormData] = useState<BedCard>({
    id: 0,
    isAvailable: true,
    status: "",
    clinic: "",
    department: "",
    clinicDepartmentId: 0,
    bed: "",
    bedId: 0,
    patient: "",
    patientId: 0
  });

  useEffect(() => {
    const fetchBedData = async () => {
      try {
        const response = await axios.get(`https://localhost:5262/api/ClinicDepartmentBed/${id}`, { withCredentials: true });
        const fetchedData = response.data;
        fetchedData.isAvailable = Boolean(fetchedData.isAvailable);
        setFormData(fetchedData);
      } catch (error) {
        console.error('Error fetching bed data:', error);
      }
    };

    fetchBedData();
  }, [id]);

  useEffect(() => {
    axios.get("https://localhost:5262/api/Patient/without-beds", { withCredentials: true })
      .then(res => {
        setPatients(res.data);
      })
      .catch(err => console.log(err.message));
  }, []);

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
      await axios.patch(`https://localhost:5262/api/ClinicDepartmentBed/edit/${id}`, formData, { withCredentials: true });
      setShowSuccessMessage(true);
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

  return (
    <>
      <BackButton to="/clinic">
        <button>Natrag</button>
      </BackButton>
      <FormContainer onSubmit={handleFormSubmit}>
        <FormSection>
          <SelectField
            name="isAvailable"
            value={String(formData.isAvailable)}
            onChange={handleInputChange}
            required
          >
            <option value="">Odaberi dostupnost</option>
            <option value="true">Da</option>
            <option value="false">Ne</option>
          </SelectField>
          <InputField
            name="status"
            placeholder="Status"
            value={formData.status}
            onChange={handleInputChange}
          />

          {user && user.role === 'Clerk' && (
            <>
              <AddPatientButton type="button" onClick={handleAddPatientClick}>
                + Dodaj pacijenta
              </AddPatientButton>
              {showPatientSelect && (
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
              )}
            </>
          )}

          <SubmitButton type="submit">Spremi</SubmitButton>
          {showSuccessMessage && (
            <SuccessMessage>
              Krevet je uspješno uređen!
            </SuccessMessage>
          )}
        </FormSection>
      </FormContainer>
    </>
  );
};

export default EditBed;