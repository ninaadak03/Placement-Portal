export interface StudentOpeningResponseDto {
  openingId: number;
  companyName: string;
  role: string;
  stipend: number | null;
  ctc: number | null;
  minCGPA: number;
  minTenthPercentage: number;
  minTwelfthPercentage: number;
  allowedBranches: string | null;
  maxAge: number | null;
  applicationDeadline: string;
  isEligible: boolean;
  hasApplied: boolean;
}
