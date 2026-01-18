export interface IAttendenceList {
  empId: number;
  empName: string;
  date: Date;
  scanInTime: Date;
  scanOffTime?: Date;
  status: string;
  createdAt: Date;
}
