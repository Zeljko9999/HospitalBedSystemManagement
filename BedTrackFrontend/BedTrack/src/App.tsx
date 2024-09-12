import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import { createGlobalStyle } from 'styled-components';
import Login from './components/Login';
import Home from './components/Home';
import Navbar from './components/Navbar';
import { UserProvider, useUser } from './components/UserContext';
import Profile from './components/Profile';
import EditProfile from './components/EditProfile';
import Clinic from './components/Clinic';
import EditBed from './components/EditBed';
import Patients from './components/Patients';
import NewPatient from './components/NewPatient';

const GlobalStyle = createGlobalStyle`
  a {
    text-decoration: none;
    cursor: pointer;
    color: #3079c7;

    &:hover {
      text-decoration: underline;
    }
  }
`;


const App: React.FC = () => {
  return (
    <UserProvider>
      <Router>
        <GlobalStyle />
        <AppContent />
      </Router>
    </UserProvider>
  );
};
 
const AppContent: React.FC = () => {
  const { user, setUser } = useUser();

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
  }, [setUser]);

//<Route path="/patients/edit/:id" element={user ? <EditPatient /> : <Navigate to="/login" />} />

  return (
    <>
      {user && <Navbar />}
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/home" element={user ? <Home /> : <Navigate to="/login" />} />
        <Route path="/clinic" element={user ? <Clinic /> : <Navigate to="/login" />} />
        <Route path="/patients" element={user ? <Patients /> : <Navigate to="/login" />} />
        <Route path="/create-patient" element={user ? <NewPatient /> : <Navigate to="/login" />} />
        <Route path="/profile" element={user ? <Profile /> : <Navigate to="/login" />} />
        <Route path="/profile/edit/:id" element={user ? <EditProfile /> : <Navigate to="/login" />} />
        <Route path="/beds/edit/:id" element={user ? <EditBed /> : <Navigate to="/login" />} />

        <Route path="*" element={<Navigate to={user ? "/home" : "/login"} />} />
      </Routes>
    </>
  );
};

export default App;