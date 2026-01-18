import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IAttendenceList } from '../types/IattendenceList';
import { ITodayAttendanceStatus } from '../types/AttendanceStatus';
import { IPaySlips } from '../types/PaySlips';
import { IAddPaySlip } from '../types/AddPaySlip';

@Injectable({
  providedIn: 'root',
})
export class Attendence {
  http = inject(HttpClient);

  SCANIN() {
    return this.http.post(environment.apiUrl + '/Attendence/scan-in', {});
  }

  SCANOFF() {
    return this.http.post(environment.apiUrl + '/Attendence/scan-off', {});
  }

  GETALL() {
    return this.http.get<IAttendenceList[]>(environment.apiUrl + '/Attendence/get-all-list');
  }

  GETATTENDTODY() {
    return this.http.get<ITodayAttendanceStatus>(environment.apiUrl + '/Attendence/today-status');
  }

  // ------------------------------------------------------------------------------------------------------------------------------------

  GETPAYSLIP(empId: number) {
    return this.http.get<IPaySlips>(environment.apiUrl + '/Attendence/GetPaySlip/' + empId);
  }

  ADDSLIP(data: IAddPaySlip) {
    return this.http.post(environment.apiUrl + '/Attendence/AddPAySlip', data);
  }
}
