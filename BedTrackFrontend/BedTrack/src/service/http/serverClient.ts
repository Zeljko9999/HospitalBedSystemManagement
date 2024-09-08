import axios, { AxiosResponse } from 'axios';
import { AccessDeniedError } from '../../error/AccessDeniedError';


const baseURL = import.meta.env.VITE_API_BASE_URL;
if (!baseURL) {
  throw new Error(
    'Missing required environment variable for serverClient configuration!'
  );
}

const serverClient = axios.create({
  withCredentials: true,
  baseURL,
  headers: {
    'Cache-Control': 'no-cache, no-store, must-revalidate',
    'Content-Type': 'application/json',
    Pragma: 'no-cache',
    Expires: '0'
  }
});

serverClient.interceptors.request.use(async (config) => {
  return config;
});

serverClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    if (error.response && error.response.status === 400) {
      return Promise.reject(new Error(error.response.data));
    } else if (error.response && error.response.status === 403) {
      return Promise.reject(new AccessDeniedError());
    } else if (axios.isCancel(error)) {
      return Promise.reject(new Error());
    } else {
      return Promise.reject(new Error(error));
    }
  }
);

export default serverClient;
