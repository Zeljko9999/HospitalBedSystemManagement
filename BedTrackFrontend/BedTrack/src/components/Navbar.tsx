import React from 'react';
import styled from 'styled-components';
import { NavLink, useNavigate } from 'react-router-dom';
import { useUser } from './UserContext';
import { FaHospitalSymbol } from "react-icons/fa";
import api from '../service/api';

const Container = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  background-color: #007bff;
  padding: 10px 20px;
  border: 3px solid #0f51c4;
  position: fixed;
  top: 0;
  right: 0;
  left: 0;
  box-shadow: 0 0 20px rgba(0, 0, 0, .1);
  font-family: sans-serif;
`;
const StyledNavLinkHome = styled(NavLink)`
  color: white;
  text-decoration: none;
  margin-right: 10px;
  font-family: fantasy;
  font-size: 35px;

`;

const StyledNavLinkUser = styled(NavLink)`
  color: white;
  text-decoration: none;
  margin-right: 20px;
  font-size: 19px;

  &:hover {
    text-decoration: underline;
    color: red;
  }

   &.active {
    color: green;
  }
`;

const StyledNavLink= styled(NavLink)`
  color: white;
  text-decoration: none;
  margin-left: 40px;

  &:hover {
    text-decoration: underline;
    color: red;
  }

   &.active {
    color: green;
  }
`;


const Button = styled.button`
  background-color: #ff4d4d;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;

  &:hover {
    background-color: #e60000;
  }
`;
const NavElement = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  font-size: 30px;
`;

const User = styled.div`

`

const StyledHospitalSymbol = styled(FaHospitalSymbol)`
  fill: white;
  width: 3rem;
  height: 3rem;
`;

const Navbar: React.FC = () => {
  const navigate = useNavigate();
  const { user, setUser } = useUser();

  const handleLogout = async () => {
    try {
       await api.logout();
    }
    catch (error) {
    console.error('Error while logging out:', error);
    }
    setUser(null);
    localStorage.removeItem('user');
    navigate('/login');
  };

  return (
    <Container>
      <NavElement>
        <StyledNavLinkHome to="/home"><StyledHospitalSymbol /> </StyledNavLinkHome>
        <StyledNavLinkHome to="/home"> BedTrack</StyledNavLinkHome>
        <StyledNavLink to="/clinic">Klinika</StyledNavLink>
        <StyledNavLink to="/patients">Pacijenti</StyledNavLink>
      </NavElement>
      <User>
      {user ? (
        <>
          <StyledNavLinkUser to="/profile">{user.name}</StyledNavLinkUser>
          <Button onClick={handleLogout}>Odjava</Button>
        </>
      ) : (
        <StyledNavLink to="/login">Prijava</StyledNavLink>
      )}
      </User>
    </Container>
  );
};

export default Navbar;