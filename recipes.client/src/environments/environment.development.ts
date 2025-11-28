import { Environment } from './environment.interface';

export const environment: Environment = {
  production: false,
  apiUrl: '' // Empty string means relative path, proxy will handle routing
};
