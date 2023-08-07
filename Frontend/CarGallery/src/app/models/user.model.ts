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
