// import { DecimalPipe } from "@angular/common";

export interface IEmployee {
  id: number;
  name: string;
  email: string;
  phone: string;
  jobTitle: string;
  gender: number;
  departmentId: number | null;
  joiningDate: string;
  lastWorkingDate: string | null;
  dateOfBirth: string;
  basicSalary: number;
}

export enum Gender{
    Male = 1,
    Female = 2,
}