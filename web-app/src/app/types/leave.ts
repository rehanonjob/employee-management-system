export interface ILeave {
  leaveType: string;
  startDate: Date;
  endDate: Date;
  reason: string; 
  id?: number;
  status?: string;
}
