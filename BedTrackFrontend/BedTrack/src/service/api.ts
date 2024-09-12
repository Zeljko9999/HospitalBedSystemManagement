import { BedCard } from '../interfaces/BedCard';
import { Clinic } from '../interfaces/Clinic';
import { ClinicDepartment } from '../interfaces/ClinicDepartment';
import { Department } from '../interfaces/Department';
import { Patient } from '../interfaces/Patient';
import { User } from '../interfaces/User';
import serverClient from './http/serverClient';


export const login = async (email: string, password: string) => {
  try {
    const response = await serverClient.post<{ email: string; password: string }>(`/login?useCookies=true`,
       { email, password }
      );
    return response;   
  } catch (error) {
    console.error('Failed to login. ', error);
    throw error;
  }
};

export const logout = async () => {
  try {
    await serverClient.post(`/api/Account/logout`,
       null
      );
  } catch (error) {
    console.error('Failed to login. ', error);
    throw error;
  }
};

export const getUserDetails = async (email: string) => {
  try {
    const userDetailsResponse = await serverClient.get(`/api/User/userdetail/${email}`);
    return userDetailsResponse;
  } catch (error) {
    console.error('Failed to get user data. ', error);
    throw error;
  }
};

export const getUserData = async (id: string): Promise<User> => {
  try {
    const userResponse = await serverClient.get<User>(`/api/User/${id}`);
    return userResponse.data;
  } catch (error) {
    console.error('Error getting user data', error);
    throw error;
  }
};

export const getAllClinicsAndDepartments = async (): Promise<Clinic[]> => {
  try {
    const clinicsResponse = await serverClient.get<Clinic[]>(`/api/Clinic`);
    return clinicsResponse.data;
  } catch (error) {
    console.error('Error getting clinics', error);
    throw error;
  }
};

export const getAllDepartments = async (): Promise<Department[]> => {
  try {
    const departmentsResponse = await serverClient.get<Department[]>(`/api/Department`);
    return departmentsResponse.data;
  } catch (error) {
    console.error('Error getting departments', error);
    throw error;
  }
};

export const getAllClinicDepartments = async (id: number): Promise<ClinicDepartment[]> => {
  try {
    const departmentsResponse = await serverClient.get<ClinicDepartment[]>(`/ClinicDepartments/${id}`);
    return departmentsResponse.data;
  } catch (error) {
    console.error('Error getting clinic departments', error);
    throw error;
  }
};

export const getAllBedsForClinicDep = async (clinicId: number, departmentId: number): Promise<any[]> => {
  try {
    const bedsResponse = await serverClient.get<any[]>(`/AllBeds/${clinicId}&${departmentId}`);
    return bedsResponse.data;
  } catch (error) {
    console.error('Error getting beds', error);
    throw error;
  }
};

export const getBedData = async (bedId: string) => {
  try {
    const bedResponse = await serverClient.get(`/api/ClinicDepartmentBed/${bedId}`);
    return bedResponse.data;
  } catch (error) {
    console.error('Error getting bed data', error);
    throw error;
  }
};

export const getPatientsWithoutBed = async (): Promise<Patient[]> => {
  try {
    const patientsResponse = await serverClient.get<Patient[]>(`/api/Patient/without-beds`);
    return patientsResponse.data;
  } catch (error) {
    console.error('Error getting patients without bed', error);
    throw error;
  }
};

export const editBed = async (id: string, formData: BedCard) => {
  try {
     await serverClient.patch<{id: string, formData: BedCard}>(`/api/ClinicDepartmentBed/edit/${id}`,
      formData
     );
  } catch (error) {
    console.error('Error while editing bed', error);
    throw error;
  }
};

export const editUser = async (id: string, formData: User) => {
  try {
     await serverClient.patch<{id: string, formData: User}>(`/api/User/edit/${id}`,
      formData
     );
  } catch (error) {
    console.error('Error while editing user', error);
    throw error;
  }
};

export const getFilteredPatients = async (selectedFilter: string, clinicId: number, departmentId: number): Promise<Patient[]> => {

  let url = "/api/Patient";

  if (selectedFilter === "option2") {
    url += "?filter=option2";
  } else if (selectedFilter === "option3") {
    url += `/department-patients${clinicId}&${departmentId}`;
  } else if (selectedFilter === "option4") {
    url += "?filter=option4";
  }
  try {
    const patientsResponse = await serverClient.get<Patient[]>(url);
    return patientsResponse.data;
  } catch (error) {
    console.error('Error getting beds', error);
    throw error;
  }
};

export const createPatient = async (formData: Patient) => {
  try {
     await serverClient.post<{id: string, formData: Patient}>(`/api/Patient`,
      formData
     );
  } catch (error) {
    console.error('Error while creating patient', error);
    throw error;
  }
};




// export outside api.ts

export default {
  login,
  logout,
  getUserDetails,

  getUserData,
  editUser,

  getAllClinicsAndDepartments,
  getAllDepartments,
  getAllClinicDepartments,
  getAllBedsForClinicDep,
  
  getBedData,
  getPatientsWithoutBed,
  editBed,

  getFilteredPatients,
  createPatient
};
