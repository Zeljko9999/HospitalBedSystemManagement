import React, { useState, ChangeEvent, FormEvent } from 'react';
import styled from 'styled-components';
import axios from 'axios';

// Styled-components for the login form
const LoginForm = styled.form`
  display: flex;
  flex-direction: column;
  align-items: center;
  background-color: #f0f0f0;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
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

const LoginComponent: React.FC = () => {
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [message, setMessage] = useState<string>('');
  const [isError, setIsError] = useState<boolean>(false);

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
        const response = await axios.post('https://localhost:5262/login?useCookies=true', {
          email,
          password
        }, { withCredentials: true });

        setMessage(response.data.message);
        setIsError(!response.data.success);
      } catch (error) {
        console.error('Error while logging in:', error);
        setMessage('An error occurred while logging in. Please try again.');
        setIsError(true);
      }
    }
  };

  return (
    <LoginForm onSubmit={handleLogin}>
      <h2>Login</h2>
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
      <Button type="submit">Login</Button>
      {message && <Message error={isError}>{message}</Message>}
    </LoginForm>
  );
};

export default LoginComponent;