import { Environment } from './environment.interface';

export const environment: Environment = {
  production: true,
  apiUrl: '' // Empty string for same-domain deployment, or use full URL like 'https://api.example.com'
};
