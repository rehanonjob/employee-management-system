import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILeave } from '../types/leave';
import { environment } from '../../environments/environment';
import { IAdminLeaves } from '../types/AdminLeaves';

@Injectable({
  providedIn: 'root',
})
export class Leave {
  constructor(private http: HttpClient) {}

  APPLYLEAVE(model: ILeave) {
    return this.http.post<ILeave>(environment.apiUrl + '/Leave/apply', model);
  }

  GETLEAVESLIST() {
    return this.http.get<ILeave[]>(environment.apiUrl + '/Leave/myleavelist');
  }

  GETALLLEAVES() {
    return this.http.get<IAdminLeaves[]>(environment.apiUrl + '/Leave/leaves-stack');
  }

  UPDATELEAVESTATUS(data: { id: number; status: string }) {
    return this.http.post(environment.apiUrl + '/Leave/update-status', data);
  }

  CANCELLEAVE(id:number){
    return this.http.post(environment.apiUrl+'/Leave/cancel/'+ id,{});
  }
}
