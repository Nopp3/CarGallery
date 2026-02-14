export interface User {
  id: string;
  role_id: number;
  username: string;
  email: string;
  password: string;
}

export interface Login {
  username: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  tokenType: string;
  expiresAtUtc: string;
  userId: string;
  username: string;
  role: string;
}

export interface AuthUserResponse {
  userId: string;
  username: string;
  role: string;
}
