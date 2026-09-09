export interface StudentProfileResponseDto {
  rollNo: string;
  email: string;
  name: string;
  phoneNumber: string;
  branch: string;
  gender: string;
  dateOfBirth: string;
  tenthPercentage: number;
  twelfthPercentage: number;
  sgpaSem1: number | null;
  sgpaSem2: number | null;
  sgpaSem3: number | null;
  sgpaSem4: number | null;
  sgpaSem5: number | null;
  sgpaSem6: number | null;
  sgpaSem7: number | null;
  sgpaSem8: number | null;
  cgpa: number;
  resumeUrl: string;
  isProfileCompleted: boolean;
}
