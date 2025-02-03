import { JwtPayload } from 'jwt-decode';

export interface MyJwtPayload extends JwtPayload {
  id: number;
  email: string;
  role: string;
}
