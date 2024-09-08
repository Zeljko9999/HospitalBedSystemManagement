import React, { useState, ChangeEvent, FormEvent } from 'react';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';
import { useUser } from './UserContext';
import api from '../service/api';

const LoginForm = styled.form`
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: #f0f0f0;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  max-width: 300px;
  margin: auto;
  margin-top: 200px;
`;

const Input = styled.input`
  width: 100%;
  padding: 10px;
  margin: 10px 0;
  border: 1px solid #ccc;
  border-radius: 4px;
`;

const Button = styled.button`
  width: 100%;
  padding: 10px;
  margin-top: 10px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;

  &:hover {
    background-color: #0056b3;
  }
`;

const Message = styled.div<{ error?: boolean }>`
  color: ${({ error }) => (error ? 'red' : 'green')};
  margin-top: 10px;
`;

const Login: React.FC = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [message, setMessage] = useState<string>('');
  const [isError, setIsError] = useState<boolean>(false);
  const navigate = useNavigate();
  const { setUser } = useUser();

  const handleEmailChange = (event: ChangeEvent<HTMLInputElement>) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event: ChangeEvent<HTMLInputElement>) => {
    setPassword(event.target.value);
  };

  const handleLogin = async (event: FormEvent) => {
    event.preventDefault();
    if (email === '' || password === '') {
      setMessage('Please fill in both fields.');
      setIsError(true);
    } else {
      setMessage('');
      try {
        const loginResponse = await api.login(email, password);
        
        if (loginResponse.status === 200) {
          const userDetailsResponse = await api.getUserDetails(email);     
          if (userDetailsResponse.status === 200) {
            setUser(userDetailsResponse.data);
            navigate('/home');
          } else {
            setMessage('Failed to fetch user details.');
            setIsError(true);
          }
        } else {
          setMessage('Login failed. Please check your credentials.');
          setIsError(true);
        }
      } catch (error) {
        console.error('Error while logging in:', error);
        setMessage('An error occurred while logging in. Please try again.');
        setIsError(true);
      }
    }
  };

  return (
    <LoginForm onSubmit={handleLogin}>
      <h2>BedTrack prijava</h2>
      <Input
        type="email"
        placeholder="Email"
        value={email}
        onChange={handleEmailChange}
      />
      <Input
        type="password"
        placeholder="Password"
        value={password}
        onChange={handlePasswordChange}
      />
      <Button type="submit">Prijava</Button>
      {message && <Message error={isError}>{message}</Message>}
    </LoginForm>
  );
};

export default Login;