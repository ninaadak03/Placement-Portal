export interface AuthResponseDto {
  success: boolean;
  message?: string;
  token?: string;
  role?: string;
  isProfileCompleted: boolean;
}
