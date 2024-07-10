export interface BedCard {
      id: number;
      isAvailable: boolean;
      status: string;
      clinic: string;
      department: string;
      clinicDepartmentId: number;
      bed: string;
      bedId: number;
      patient?: string | null;
      patientId?: number | null;
  }