import axios from "axios";
import { useState, useEffect, ChangeEvent, FormEvent } from "react";
import { useParams, Link } from 'react-router-dom';
import styled from 'styled-components';
import { useUser } from './UserContext';
import { Clinic } from '../interfaces/Clinic';
import { Department } from '../interfaces/Department';
import { User } from '../interfaces/User';

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


const EditProfile: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  const [clinics, setClinics] = useState<Clinic[]>([]);
  const [departments, setDepartments] = useState<Department[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const { setUser } = useUser();

  const [formData, setFormData] = useState<User>({
    id: 0,
    name: "",
    email: "",
    role: "",
    status: "",
    clinicId: 0,
    clinic: "",
    departmentId: 0,
    department: "",
  });

  useEffect(() => {
    axios.get(`https://localhost:5262/api/User/${id}`, { withCredentials: true })
      .then(res => setFormData(res.data))
      .catch(err => console.log(err.message));
  }, [id]);

  useEffect(() => {
    Promise.all([
      axios.get("https://localhost:5262/api/Clinic", { withCredentials: true }),
      axios.get("https://localhost:5262/api/Department", { withCredentials: true }),
    ])
      .then(([resClinics, resDepartments]) => {
        setClinics(resClinics.data);
        setDepartments(resDepartments.data);
      })
      .catch(err => console.log(err.message));
  }, []);

  const handleInputChange = (event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    setFormData({
      ...formData,
      [name]: name === 'clinicId' || name === 'departmentId' ? Number(value) : value
    });
  };

  const handleFormSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await axios.patch(`https://localhost:5262/api/User/edit/${id}`, formData, { withCredentials: true });
      setShowSuccessMessage(true);
      setUser(formData); // Update user context
      localStorage.setItem('user', JSON.stringify(formData)); // Sync local storage
      setTimeout(() => {
        setShowSuccessMessage(false);
      }, 3000);
    } catch (error) {
      console.error('Error while updating data:', error);
    }
  };

  return (
    <>
      <BackButton to="/profile">
        <button>Natrag</button>
      </BackButton>
      <FormContainer onSubmit={handleFormSubmit}>
        <FormSection>
          <InputField
            name="name"
            type="text"
            placeholder="Ime"
            value={formData.name}
            onChange={handleInputChange}
            required
          />
           <InputField
            name="email"
            type="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleInputChange}
            required
          />
          <InputField
            name="status"
            placeholder="Status"
            value={formData.status}
            onChange={handleInputChange}
          />
          <SelectField
            name="clinicId"
            value={formData.clinicId}
            onChange={handleInputChange}
            required
          >
            <option value=''>Klinika</option>
            {clinics.map(clinic => (
              <option key={clinic.id} value={clinic.id}>
                {clinic.name}
              </option>
            ))}
          </SelectField>
          <SelectField
            name="departmentId"
            value={formData.departmentId}
            onChange={handleInputChange}
            required
          >
            <option value=''>Odjel</option>
            {departments.map(department => (
              <option key={department.id} value={department.id}>
                {department.name}
              </option>
            ))}
          </SelectField>
          <SubmitButton type="submit">Spremi</SubmitButton>
          {showSuccessMessage && (
            <SuccessMessage>
              Profil je uspješno uređen!
            </SuccessMessage>
          )}
        </FormSection>
      </FormContainer>
    </>
  );
};

export default EditProfile;