import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';

const ProfileContainer = styled.div`
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 2px solid #1959c8;
  border-radius: 8px;
  background-color: white;
`;

const ProfileHeading = styled.h2`
  text-align: center;
  margin: auto;
  font-size: 30px;
  font-family: system-ui;
`;

const TopContainer = styled.div`
    display: flex;
    margin-top: 10px;
    margin-bottom: 50px;
    align-items: center;
`;

const EditLink = styled.a`
  color: #007bff;
  cursor: pointer;
  margin-right: 20px;
  font-size: 20px;

  &:hover {
    text-decoration: underline;
  }
`;

const ProfileItem = styled.div`
  margin-bottom: 10px;
  display: flex;
`;

const ProfileLabel = styled.span`
  font-weight: bold;
  margin-right: 10px;
  font-size: 19px;
  margin-top: 10px;
`;

const ProfileValue = styled.span`
  color: black;
  margin-left: 10px;
  font-size: 19px;
  margin-top: 10px;
`;



const Profile: React.FC = () => {
  const [userData, setUserData] = useState<any>(null);

  useEffect(() => {
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      setUserData(JSON.parse(storedUser));
    }
  }, []); 
  
  
  if (!userData) {
    return null;
  }

  return (
    <ProfileContainer>
      <TopContainer>
      <ProfileHeading>Profil korisnika</ProfileHeading>
      <Link to={`/profile/edit/${userData.id}`}> <EditLink>Uredi</EditLink></Link>
      </TopContainer>
        <ProfileItem>
            <ProfileLabel>Ime:</ProfileLabel>
            <ProfileValue>{userData.name}</ProfileValue>
        </ProfileItem>
        <ProfileItem>
            <ProfileLabel>Email:</ProfileLabel>
            <ProfileValue>{userData.email}</ProfileValue>
        </ProfileItem>
        <ProfileItem>
            <ProfileLabel>Rola:</ProfileLabel>
            <ProfileValue>{userData.role}</ProfileValue>
        </ProfileItem>
        <ProfileItem>
            <ProfileLabel>Status:</ProfileLabel>
            <ProfileValue>{userData.status}</ProfileValue>
        </ProfileItem>
        <ProfileItem>
            <ProfileLabel>Klinika:</ProfileLabel>
            <ProfileValue>{userData.clinic}</ProfileValue>
        </ProfileItem>
        <ProfileItem>
            <ProfileLabel>Odjel:</ProfileLabel>
            <ProfileValue>{userData.department}</ProfileValue>
        </ProfileItem>
    </ProfileContainer>
  );
};

export default Profile;