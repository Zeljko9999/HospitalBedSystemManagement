import React from 'react';
import styled from 'styled-components';
import { NavLink, useNavigate } from 'react-router-dom';
import { useUser } from './UserContext';
import axios from 'axios';

const Container = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  background-color: #007bff;
  padding: 10px 20px;
  border: 3px solid #0f51c4;

`;
const StyledNavLink = styled(NavLink)`
  color: white;
  text-decoration: none;
  margin-right: 20px;
  margin-left: 20px;

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
  border-radius: 4px;
  cursor: pointer;

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

const Navbar: React.FC = () => {
  const navigate = useNavigate();
  const { user, setUser } = useUser();

  const handleLogout = async () => {
    try {
       await axios.post(
        'https://localhost:5262/api/Account/logout', null,
        { withCredentials: true }
      );
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
      <StyledNavLink to="/home">BedTrack</StyledNavLink>
        <StyledNavLink to="/clinic">Klinika</StyledNavLink>
      </NavElement>
      <User>
      {user ? (
        <>
          <StyledNavLink to="/profile">{user.name}</StyledNavLink>
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