import React, { useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import './App.css';
import Login from './components/Login';
import Home from './components/Home';
import Navbar from './components/Navbar';
import { UserProvider, useUser } from './components/UserContext';
import Profile from './components/Profile';
import EditProfile from './components/EditProfile';
import Clinic from './components/Clinic';
import EditBed from './components/EditBed';

//required packages:
//npm install styled-components
//npm install styled-components @types/styled-components
//npm install axios
//npm install cors
//npm install react-router-dom

const App: React.FC = () => {
  return (
    <UserProvider>
      <Router>
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

  return (
    <>
      {user && <Navbar />}
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/home" element={user ? <Home /> : <Navigate to="/login" />} />
        <Route path="/clinic" element={user ? <Clinic /> : <Navigate to="/login" />} />
        <Route path="/profile" element={user ? <Profile /> : <Navigate to="/login" />} />
        <Route path="/profile/edit/:id" element={user ? <EditProfile /> : <Navigate to="/login" />} />
        <Route path="/beds/edit/:id" element={user ? <EditBed /> : <Navigate to="/login" />} />
        <Route path="*" element={<Navigate to={user ? "/home" : "/login"} />} />
      </Routes>
    </>
  );
};

export default App;