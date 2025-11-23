export interface LoginDto {
  user_name: string;
  password: string;
}

export interface TokenDto {
  access_token: string;
  token_type: string;
}
